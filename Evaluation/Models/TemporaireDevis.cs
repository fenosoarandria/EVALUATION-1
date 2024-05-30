using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper;

[Table("temp_devis")]
public class TemporaireDevis
{    
    [Column("client")]
    public string? Client { get; set; }

    [Column("ref_devis")]
    public string? RefDevis { get; set; }

    [Column("type_maison")]
    public string? TypeMaison { get; set; }

    [Column("finition")]
    public string? Finition { get; set; }

    [Column("taux_finition")]
    public double? TauxFinition { get; set; }

    [Column("date_devis")]
    public DateTime? DateDevis { get; set; }

    [Column("date_debut")]
    public DateTime? DateDebut { get; set; }

    [Column("lieu")]
    public string? Lieu { get; set; }

 public static TemporaireDevis MapTemporaireDevis(CsvReader csv)
    {
        return new TemporaireDevis
        {
            Client = csv.GetField<string>("client"),
            RefDevis = csv.GetField<string>("ref_devis"),
            TypeMaison = csv.GetField<string>("type_maison"),
            Finition = csv.GetField<string>("finition"),
            TauxFinition = Convert.ToDouble(Contrainte.ApplyConstraints(csv.GetField<string>("taux_finition"))),
            DateDevis = Contrainte.ParseDate(csv.GetField<string?>("date_devis")),
            DateDebut = Contrainte.ParseDate(csv.GetField<string?>("date_debut")),
            Lieu = csv.GetField<string>("lieu")
        };
    }
}