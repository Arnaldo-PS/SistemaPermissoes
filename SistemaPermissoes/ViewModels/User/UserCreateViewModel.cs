using System.ComponentModel.DataAnnotations;

public class UserCreateViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string Senha { get; set; }

    [Required(ErrorMessage = "A confirmação da senha é obrigatória.")]
    [Compare("Senha", ErrorMessage = "As senhas não coincidem.")]
    public string ConfirmarSenha { get; set; }
}