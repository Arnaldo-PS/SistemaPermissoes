using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaPermissoes.Models
{
    public class UsuarioPapel
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        [ForeignKey("Papel")]
        public int PapelId { get; set; }
        public virtual Papel Papel { get; set; }
    }
}
