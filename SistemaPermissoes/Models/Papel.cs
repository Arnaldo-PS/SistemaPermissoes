using SistemaPermissoes.Models;
using System.ComponentModel.DataAnnotations;

public class Papel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; }

    public virtual ICollection<UsuarioPapel> UsuarioPapel { get; set; }
    public virtual ICollection<PapelTarefa> PapelTarefa { get; set; }
}
