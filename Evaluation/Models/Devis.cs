using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

[Table("devis")]
public class Devis
{
    [Key]
    [Column("id")]
    public string? Id { get; set; }

    [Column("ref_devis")]
    public string? RefDevis { get; set; }

    [Column("id_client")]
    public string? IdClient { get; set; }

    [Column("id_type_maison")]
    public string? IdTypeMaison { get; set; }

    [Column("id_type_finition")]
    public string? IdTypeFinition { get; set; }

    [Column("date_creation")]
    public DateTime DateCreation { get; set; }

    [Column("date_debut_travaux")]
    public DateTime DateDebutTravaux { get; set; }

    [Column("montant_total")]
    public double? MontantTotal { get; set; }
    
    [Column("id_lieu")]
    public string? IdLieu { get; set; }

    public virtual TypeMaison? TypeMaison { get; set; }
    public virtual TypeFinition? TypeFinition { get; set; }
    public virtual Lieu? Lieu { get; set; }


}
