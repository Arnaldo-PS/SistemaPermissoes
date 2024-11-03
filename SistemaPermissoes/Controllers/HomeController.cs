using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize] // Garante que apenas usuários autenticados acessem essa página
public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {

        var userEmail = User.Identity.Name; // ou o método apropriado para obter o ID do usuário

        return View(); // Retorna a view Index.cshtml dentro da pasta Views/Home
    }
}
