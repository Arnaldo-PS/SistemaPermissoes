public class AssignPermissionsViewModel
{
    public int UsuarioId { get; set; }
    public string NomeUsuario { get; set; }
    public List<PapelViewModel> Papeis { get; set; } // Lista de papéis disponíveis
}

public class PapelViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public bool IsAssigned { get; set; } // Indica se o papel já está atribuído
}
