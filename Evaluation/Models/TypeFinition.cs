using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

[Table("type_finition")]
public class TypeFinition
{
    [Key]
    [Column("id")]
    public string? Id { get; set; }

    [Column("nom")]
    public string? Nom { get; set; }

    [Column("pourcentage")]
    public double? Pourcentage { get; set; }
    public virtual ICollection<Devis>? Devis { get; set; }


}
