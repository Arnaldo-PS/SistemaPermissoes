using SistemaPermissoes.Models;
using System.ComponentModel.DataAnnotations;

public class Usuario
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [Required]
    [StringLength(255)]
    public string Senha { get; set; }

    public virtual ICollection<UsuarioPapel> UsuarioPapel { get; set; }
}
