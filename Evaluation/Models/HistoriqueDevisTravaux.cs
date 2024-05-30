using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("historique_devis_travaux")]
public class HistoriqueDevisTravaux
{
    [Key]
    [Column("id")]
    public string? Id { get; set; }

    [Column("id_devis")]
    public string? IdDevis { get; set; }

    [Column("id_travaux")]
    public string? IdTravaux { get; set; }

    [Column("prix_unitaire")]
    public double? PrixUnitaire { get; set; }

    [Column("quantite")]
    public double? Quantite { get; set; }
    
    [Column("id_unite")]
    public string? IdUnite { get; set; }

    [ForeignKey("IdTravaux")]
    public virtual Travaux? Travaux { get; set; }

    [ForeignKey("IdUnite")]
    public virtual Unite? Unite { get; set; }
}
