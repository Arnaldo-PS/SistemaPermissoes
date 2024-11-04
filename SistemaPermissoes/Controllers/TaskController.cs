using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class TaskController : Controller
{
    private readonly ApplicationDbContext _context;

    public TaskController(ApplicationDbContext context)
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

        var tarefas = await _context.Tarefas.ToListAsync();
        return View(tarefas);
    }

    public IActionResult Create()
    {
        return View(new TaskViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskViewModel taskViewModel)
    {
        if (ModelState.IsValid)
        {
            var tarefa = new Tarefa
            {
                Nome = taskViewModel.Nome
            };

            _context.Add(tarefa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(taskViewModel);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tarefa = await _context.Tarefas.FindAsync(id);
        if (tarefa == null)
        {
            return NotFound();
        }

        var taskViewModel = new TaskViewModel
        {
            Id = tarefa.Id,
            Nome = tarefa.Nome
        };

        return View(taskViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, TaskViewModel taskViewModel)
    {
        if (id != taskViewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var tarefa = await _context.Tarefas.FindAsync(id);
                if (tarefa == null)
                {
                    return NotFound();
                }

                tarefa.Nome = taskViewModel.Nome;
                _context.Update(tarefa);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(taskViewModel.Id))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(taskViewModel);
    }

    private bool TaskExists(int id)
    {
        return _context.Tarefas.Any(e => e.Id == id);
    }
}
