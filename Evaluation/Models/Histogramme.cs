using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

[Keyless]
[Table("histogramme_montant")]
public class Histogramme
{
    [Column("year")]
    public int Year { get; set; }

    [Column("month")]
    public int Month { get; set; }

    [Column("total_devis")]
    public int TotalDevis { get; set; }

    [Column("total_montant")]
    public double TotalMontant { get; set; }
}
