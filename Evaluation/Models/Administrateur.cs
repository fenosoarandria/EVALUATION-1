using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

[Table("administrateur")]
public class Administrateur
{
    [Key]
    [Column("id")]
    public string? Id { get; set; }

    [Column("nom")]
    public string? Nom { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("mot_de_passe")]
    public string? MotDePasse { get; set; }

}
