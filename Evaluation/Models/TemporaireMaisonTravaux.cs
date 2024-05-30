using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using CsvHelper;

[Table("temp_maison_travaux")]
public class TemporaireMaisonTravaux
{
    [Column("type_maison")]
    public string? TypeMaison { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("surface")]
    public double? Surface { get; set; }

    [Column("code_travaux")]
    public int CodeTravaux { get; set; }

    [Column("type_travaux")]
    public string? TypeTravaux { get; set; }

    [Column("unite")]
    public string? Unite { get; set; }

    [Column("prix_unitaire")]
    public double? PrixUnitaire { get; set; }

    [Column("quantite")]
    public double? Quantite { get; set; }

    [Column("duree_travaux")]
    public double? DureeTravaux { get; set; }
     public static TemporaireMaisonTravaux MapTemporaireMaisonTravaux(CsvReader csv)
    {
        return new TemporaireMaisonTravaux
        {
            TypeMaison = csv.GetField<string>("type_maison"),
            Description = csv.GetField<string>("description"),
            Surface = Convert.ToDouble(csv.GetField<string>("surface").Replace(',', '.'), CultureInfo.InvariantCulture),
            CodeTravaux = csv.GetField<int>("code_travaux"),
            TypeTravaux = csv.GetField<string>("type_travaux"),
            Unite = csv.GetField<string>("unité"),
            PrixUnitaire = Convert.ToDouble(csv.GetField<string>("prix_unitaire").Replace(',', '.'), CultureInfo.InvariantCulture),
            Quantite = Convert.ToDouble(csv.GetField<string>("quantite").Replace(',', '.'), CultureInfo.InvariantCulture),
            DureeTravaux = csv.GetField<double?>("duree_travaux")
        };
    }
    
}
