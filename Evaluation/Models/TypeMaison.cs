using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

[Table("type_maison")]
public class TypeMaison
{
    [Key]
    [Column("id")]
    public string? Id { get; set; }

    [Column("nom")]
    public string? Nom { get; set; }

    [Column("duree_travaux")]
    public double? DureeTravaux { get; set; }

    [Column("surface")]
    public double? Surface { get; set; }

    [Column("description")]
    public string? Description { get; set; }
    public virtual ICollection<Devis>? Devis { get; set; }


}
