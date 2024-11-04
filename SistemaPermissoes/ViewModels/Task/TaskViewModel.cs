using System.ComponentModel.DataAnnotations;


    public class TaskViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da tarefa é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome da tarefa não pode exceder 100 caracteres.")]
        public string Nome { get; set; }
    }

