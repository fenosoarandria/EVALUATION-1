using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Transactions;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Npgsql;

public class Import
{
 private readonly ApplicationDbContext _dbContext;

    public Import(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void ImportCsvToDatabase<T>(string table,IFormFile file, Func<CsvReader, T> mapFunc, CsvConfiguration? csvConfig = null) where T : class
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("Le fichier est vide ou n'existe pas.");
        }

        if (csvConfig == null)
        {
            csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                BadDataFound = null,
                MissingFieldFound = null
            };
        }

        using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
        using (var csv = new CsvReader(reader, csvConfig))
        {
            csv.Read();
            csv.ReadHeader();

            var entities = new List<T>();

            while (csv.Read())
            {
                var entity = mapFunc(csv);
                entities.Add(entity);
            }

            // Bulk insert using raw SQL
            foreach (var entity in entities)
            {
                var sql = GetInsertSql<T>(table);
                var parameters = GetSqlParameters<T>(entity);
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
            }
        }
    }

    private string GetInsertSql<T>(string table)
    {
        var tableName = table; // Assume que le nom de la classe correspond au nom de la table
        var properties = typeof(T).GetProperties();
        var valueParams = string.Join(", ", properties.Select(p => $"@{p.Name.ToLower()}"));
        return $"INSERT INTO {tableName} VALUES ({valueParams})";
    }

    private NpgsqlParameter[] GetSqlParameters<T>(T entity)
    {
        var properties = typeof(T).GetProperties();
        var parameters = new List<NpgsqlParameter>();
        foreach (var property in properties)
        {
            parameters.Add(new NpgsqlParameter($"@{property.Name.ToLower()}", property.GetValue(entity) ?? DBNull.Value));
        }

        return parameters.ToArray();
    }

    public void InsertDataFromTempMaisonTravaux()
    {
        using (var transaction = _dbContext.Database.BeginTransaction())
        {
            try
            {
                // Insérer les données dans la table Unite
                _dbContext.Database.ExecuteSqlRaw("INSERT INTO unite (nom) SELECT DISTINCT unite FROM temp_maison_travaux");
                

                // Insérer les données dans la table TypeMaison
                _dbContext.Database.ExecuteSqlRaw(@"INSERT INTO type_maison (nom, duree_travaux, surface, description) 
                                                SELECT DISTINCT tmt.type_maison, CAST(tmt.duree_travaux AS numeric) , CAST(tmt.surface AS numeric) , tmt.description
                                                FROM temp_maison_travaux tmt");

                // Insérer les données dans la table Travaux
                _dbContext.Database.ExecuteSqlRaw(@"INSERT INTO travaux (id_unite, code_travaux, designation, prix_unitaire) 
                                                    SELECT DISTINCT u.id, tmt.code_travaux, tmt.type_travaux, CAST(tmt.prix_unitaire AS numeric)
                                                    FROM temp_maison_travaux tmt
                                                    JOIN unite u ON tmt.unite = u.nom
                                                    ");

                _dbContext.Database.ExecuteSqlRaw(@"INSERT INTO travaux_type_maison (id_travaux,id_type_maison,quantite) 
                                                    SELECT t.id as id_travaux,tm.id as id_type_maison ,CAST(tmt.quantite AS numeric) 
                                                    FROM temp_maison_travaux tmt
                                                    JOIN travaux t ON tmt.code_travaux = t.code_travaux
                                                    JOIN type_maison tm ON tmt.type_maison = tm.nom");
                transaction.Commit(); // Commit la transaction
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite lors de l'insertion des données : " + ex.Message);
                // La transaction sera rollback automatiquement car Commit() n'a pas été appelé
            }
        }
    }
    
    public void InsertDataFromDevis()
{
    using (var transaction = _dbContext.Database.BeginTransaction())
    {
        try
        {
            // Insérer les données initiales dans les tables pertinentes
            _dbContext.Database.ExecuteSqlRaw("INSERT INTO client (contact) SELECT DISTINCT client FROM temp_devis");
            _dbContext.Database.ExecuteSqlRaw(@"INSERT INTO type_finition (nom, pourcentage) 
                                                SELECT DISTINCT td.finition, td.taux_finition FROM temp_devis td");
            _dbContext.Database.ExecuteSqlRaw(@"INSERT INTO lieu (nom) 
                                                SELECT DISTINCT(lieu) FROM temp_devis");

            // Insérer les devis et récupérer les IDs nouvellement insérés
            _dbContext.Database.ExecuteSqlRaw(@"
                    INSERT INTO devis (ref_devis, id_client, id_type_maison, id_type_finition, date_creation, date_debut_travaux, montant_total, id_lieu) 
                    SELECT td.ref_devis, c.id as client, tm.id as type_maison, tf.id as finition, td.date_devis, td.date_debut, 0 as montant_total, l.id as lieu
                    FROM temp_devis td
                    JOIN client c ON td.client = c.contact
                    JOIN type_maison tm ON td.type_maison = tm.nom
                    JOIN type_finition tf ON td.finition = tf.nom
                    JOIN lieu l ON td.lieu = l.nom");

            transaction.Commit(); // Commit la transaction
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine("Une erreur s'est produite lors de l'insertion des données : " + ex.Message);
            // La transaction sera rollback automatiquement car Commit() n'a pas été appelé
        }
    }
}
    public void InsertDataFromPayement()
    {
        using (var transaction = _dbContext.Database.BeginTransaction())
        {
            try
            {
                // Insérer les données dans la table Unite
                _dbContext.Database.ExecuteSqlRaw(@"INSERT INTO payement (id_devis,id_client,date_payement,montant) 
                                                    SELECT tp.ref_devis,d.id_client,tp.date_payement,tp.montant
                                                    FROM temp_payement tp
                                                    JOIN devis d ON d.ref_devis = tp.ref_devis");
                
                transaction.Commit(); // Commit la transaction
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite lors de l'insertion des données : " + ex.Message);
                // La transaction sera rollback automatiquement car Commit() n'a pas été appelé
            }
        }
    }
    
    
}
