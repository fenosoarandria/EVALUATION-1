using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

[Table("travaux_type_maison")]
public class TravauxTypeMaison
{
    [Key]
    [Column("id")]
    public string? Id { get; set; }

    [Column("id_travaux")]
    public string? IdTravaux { get; set; }

    [Column("id_type_maison")]
    public string? IdTypeMaison { get; set; }

    [Column("quantite")]
    public double? Quantite { get; set; }

}
