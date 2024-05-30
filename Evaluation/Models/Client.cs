using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

[Table("client")]
public class Client
{
    [Key]
    [Column("id")]
    public string? Id { get; set; }

    [Column("contact")]
    public string? Contact { get; set; }

}
