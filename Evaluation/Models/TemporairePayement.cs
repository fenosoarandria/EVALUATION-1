using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper;

[Table("temp_payement")]
public class TemporairePayement
{
    [Column("ref_devis")]
    public string? RefDevis {get;set;}
    [Column("ref_payement")]
    public string? RefPayement {get;set;}
    [Column("date_payement")]
    public DateTime? DatePayement {get;set;}
    [Column("montant")]
    public double? Montant {get;set;}
 public static TemporairePayement MapTemporairePayement(CsvReader csv)
    {
        return new TemporairePayement
        {
            RefDevis = csv.GetField<string>("ref_devis"),
            RefPayement = csv.GetField<string>("ref_paiement"),
            DatePayement = Contrainte.ParseDate(csv.GetField<string?>("date_paiement")),
            Montant = csv.GetField<double?>("montant"),
        };
    }
}