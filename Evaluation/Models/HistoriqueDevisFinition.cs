using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

[Table("historique_devis_finition")]
public class HistoriqueDevisFinition
{
    [Key]
    [Column("id")]
    public string? Id { get; set; }

    [Column("id_devis")]
    public string? IdDevis { get; set; }

    [Column("id_finition")]
    public string? IdFinition { get; set; }

    [Column("pourcentage")]
    public double? Pourcentage { get; set; }

}
