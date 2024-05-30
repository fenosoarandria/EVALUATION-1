using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

[Table("unite")]
public class Unite
{
    [Key]
    [Column("id")]
    public string? Id { get; set; }

    [Column("nom")]
    public string? Nom { get; set; }
    public virtual ICollection<HistoriqueDevisTravaux>? HistoriqueDevisTravaux { get; set; }
    public virtual ICollection<Travaux>? Travauxes { get; set; }

}
