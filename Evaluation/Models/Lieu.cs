using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("lieu")]
public class Lieu
{
    [Key]
    [Column("id")]
    public string? Id {get;set;}    
    [Column("nom")]
    public string? Nom {get;set;}
    public virtual ICollection<Devis>? Devis { get; set; }

}