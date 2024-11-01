using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize] // Garante que apenas usuários autenticados acessem essa página
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View(); // Retorna a view Index.cshtml dentro da pasta Views/Home
    }
}
