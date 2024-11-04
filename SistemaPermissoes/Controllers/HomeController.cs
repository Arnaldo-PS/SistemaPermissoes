using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        // Obter o ID do usuário logado
        var usuarioId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

        // Carregar os papéis e tarefas do usuário logado
        var usuario = await _context.Usuarios
            .Include(u => u.UsuarioPapel)
                .ThenInclude(up => up.Papel)
                .ThenInclude(p => p.PapelTarefa)
                .ThenInclude(pt => pt.Tarefa)
            .FirstOrDefaultAsync(u => u.Id.ToString() == usuarioId);

        // Coletar as tarefas atribuídas ao usuário
        var tarefasDoUsuario = usuario.UsuarioPapel
            .SelectMany(up => up.Papel.PapelTarefa)
            .Select(pt => pt.Tarefa)
            .ToList();

        // Passar as tarefas para a View
        ViewBag.TarefasDoUsuario = tarefasDoUsuario;

        return View(); // Retorna a view Index.cshtml dentro da pasta Views/Home
    }
}
