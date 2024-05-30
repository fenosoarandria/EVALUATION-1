using Microsoft.EntityFrameworkCore;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
     
    public DbSet<Administrateur> _administrateur {get; set;}
 
    public DbSet<Unite> _unite {get; set;}
 
    public DbSet<Travaux> _travaux {get; set;}
 
    public DbSet<TravauxTypeMaison> _travaux_type_maison {get; set;}
 
    public DbSet<TypeMaison> _type_maison {get; set;}
 
    public DbSet<Client> _client {get; set;}
 
    public DbSet<Devis> _devis {get; set;}
 
    public DbSet<TypeFinition> _type_finition {get; set;}
 
    public DbSet<HistoriqueDevisTravaux> _historique_devis_travaux {get; set;}
 
    public DbSet<HistoriqueDevisFinition> _historique_devis_finition {get; set;}
 
    public DbSet<Payement> _payement {get; set;}
    public DbSet<TemporaireMaisonTravaux> _temp_maison_traveau {get; set;}
    public DbSet<TemporaireDevis> _temp_devis {get; set;}
    public DbSet<TemporairePayement> _temp_payement {get; set;}
    public DbSet<Lieu> _lieu {get; set;}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       base.OnModelCreating(modelBuilder);
        // Configuration de la séquence pour TypeProduits
        
            modelBuilder.Entity<Administrateur>()
                .Property(p => p.Id)
                .HasDefaultValueSql($"NEXT VALUE FOR seq_admin");

            modelBuilder.Entity<Unite>()
                .Property(p => p.Id)
                .HasDefaultValueSql($"NEXT VALUE FOR seq_unite");

            modelBuilder.Entity<Travaux>()
                .Property(p => p.Id)
                .HasDefaultValueSql($"NEXT VALUE FOR seq_travaux");

            modelBuilder.Entity<TravauxTypeMaison>()
                .Property(p => p.Id)
                .HasDefaultValueSql($"NEXT VALUE FOR seq_travaux_type_maison");

            modelBuilder.Entity<TypeMaison>()
                .Property(p => p.Id)
                .HasDefaultValueSql($"NEXT VALUE FOR seq_type_maison");

            modelBuilder.Entity<Client>()
                .Property(p => p.Id)
                .HasDefaultValueSql($"NEXT VALUE FOR seq_client");

            modelBuilder.Entity<Devis>()
                .Property(p => p.Id)
                .HasDefaultValueSql($"NEXT VALUE FOR seq_devis");

            modelBuilder.Entity<TypeFinition>()
                .Property(p => p.Id)
                .HasDefaultValueSql($"NEXT VALUE FOR seq_type_finition");

            modelBuilder.Entity<HistoriqueDevisTravaux>()
                .Property(p => p.Id)
                .HasDefaultValueSql($"NEXT VALUE FOR seq_historique_devis_travaux");

            modelBuilder.Entity<HistoriqueDevisFinition>()
                .Property(p => p.Id)
                .HasDefaultValueSql($"NEXT VALUE FOR seq_historique_devis_finition");

            modelBuilder.Entity<Payement>()
                .Property(p => p.Id)
                .HasDefaultValueSql($"NEXT VALUE FOR seq_payement");

            modelBuilder.Entity<Lieu>()
                .Property(p => p.Id)
                .HasDefaultValueSql($"NEXT VALUE FOR seq_lieu");
            
            modelBuilder.Entity<TemporaireMaisonTravaux>().HasNoKey();
            
            modelBuilder.Entity<TemporaireDevis>().HasNoKey();
            
            modelBuilder.Entity<TemporairePayement>().HasNoKey();

            modelBuilder.Entity<Devis>()
                .HasOne(d => d.TypeMaison)
                .WithMany(tm => tm.Devis)
                .HasForeignKey(d => d.IdTypeMaison);

            modelBuilder.Entity<Devis>()
                .HasOne(d => d.TypeFinition)
                .WithMany(tm => tm.Devis)
                .HasForeignKey(d => d.IdTypeFinition);

            modelBuilder.Entity<Devis>()
                .HasOne(d => d.Lieu)
                .WithMany(tm => tm.Devis)
                .HasForeignKey(d => d.IdLieu);

          modelBuilder.Entity<HistoriqueDevisTravaux>()
                .HasOne(d => d.Travaux)
                .WithMany(tm => tm.HistoriqueDevisTravaux)
                .HasForeignKey(d => d.IdTravaux);



            modelBuilder.Entity<HistoriqueDevisTravaux>()
                .HasOne(d => d.Unite)
                .WithMany(tm => tm.HistoriqueDevisTravaux)
                .HasForeignKey(d => d.IdUnite);

            modelBuilder.Entity<Travaux>()
                .HasOne(d => d.Unite)
                .WithMany(tm => tm.Travauxes)
                .HasForeignKey(d => d.IdUnite);

    }
}