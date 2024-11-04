public class UserViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public List<string> Papeis { get; set; } // Nova propriedade para armazenar os papéis
}
