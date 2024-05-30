using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

[Table("payement")]
public class Payement
{
    [Key]
    [Column("id")]
    public string? Id { get; set; }

    [Column("id_devis")]
    public string? IdDevis { get; set; }

    [Column("id_client")]
    public string? IdClient { get; set; }

    [Column("montant")]
    public double? Montant { get; set; }

    [Column("date_payement")]
    public DateTime? DatePayement { get; set; }

}
