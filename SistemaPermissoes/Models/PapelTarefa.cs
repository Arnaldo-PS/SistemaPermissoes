using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaPermissoes.Models
{
    public class PapelTarefa
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Papel")]
        public int PapelId { get; set; }
        public virtual Papel Papel { get; set; }

        [ForeignKey("Tarefa")]
        public int TarefaId { get; set; }
        public virtual Tarefa Tarefa { get; set; }
    }
}
