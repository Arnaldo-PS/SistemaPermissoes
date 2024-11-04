using System.ComponentModel.DataAnnotations;

public class UserEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O email deve ser um endereço válido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string Senha { get; set; }

    [Compare("Senha", ErrorMessage = "As senhas não correspondem.")]
    public string ConfirmarSenha { get; set; }
}
