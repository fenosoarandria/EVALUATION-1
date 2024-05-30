using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

[Table("travaux")]
public class Travaux
{
    [Key]
    [Column("id")]
    public string? Id { get; set; }

    [Column("id_unite")]
    public string? IdUnite { get; set; }

    [Column("code_travaux")]
    public int CodeTravaux { get; set; }

    [Column("designation")]
    public string? Designation { get; set; }

    [Column("prix_unitaire")]
    public double? PrixUnitaire { get; set; }
    public virtual Unite? Unite { get; set; }

    public virtual ICollection<HistoriqueDevisTravaux>? HistoriqueDevisTravaux { get; set; }
}
