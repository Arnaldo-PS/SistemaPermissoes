using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaPermissoes.Models;

public class RoleController : Controller
{
    private readonly ApplicationDbContext _context;

    public RoleController(ApplicationDbContext context)
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

        var papeis = await _context.Papeis
            .Include(u => u.PapelTarefa) // Inclui os papéis associados ao usuário
                .ThenInclude(up => up.Tarefa) // Inclui a tabela de papéis
            .Select(u => new RoleViewModel
            {
                Id = u.Id,
                Nome = u.Nome,
                Tarefas = u.PapelTarefa.Select(up => up.Tarefa.Nome).ToList() // Coleta os nomes dos papéis
            })
            .ToListAsync();

        return View(papeis);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        // Obter todas as tarefas disponíveis para seleção
        var tarefas = await _context.Tarefas.ToListAsync();

        var viewModel = new RoleCreateViewModel
        {
            Tarefas = tarefas.Select(t => new TarefaViewModel
            {
                TarefaId = t.Id,
                TarefaNome = t.Nome,
                IsAssigned = false
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(RoleCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Criar o papel sem definir a lista PapelTarefa ainda
            var papel = new Papel
            {
                Nome = model.Nome,
                PapelTarefa = new List<PapelTarefa>() // Inicializa a lista PapelTarefa
            };

            // Adicionar o papel ao contexto primeiro para gerar o ID
            _context.Papeis.Add(papel);
            await _context.SaveChangesAsync();

            // Adicionar tarefas selecionadas ao papel, agora com o ID do papel já definido
            papel.PapelTarefa = model.Tarefas
                .Where(t => t.IsAssigned)
                .Select(t => new PapelTarefa
                {
                    PapelId = papel.Id, // Atribui o ID gerado do papel
                    TarefaId = t.TarefaId
                })
                .ToList();

            // Atualizar o papel no contexto para salvar as associações PapelTarefa
            _context.Update(papel);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var papel = await _context.Papeis
            .Include(p => p.PapelTarefa)
            .ThenInclude(pt => pt.Tarefa)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (papel == null)
        {
            return NotFound();
        }

        var tarefas = await _context.Tarefas.ToListAsync();

        var viewModel = new RoleCreateViewModel
        {
            Id = papel.Id,
            Nome = papel.Nome,
            Tarefas = tarefas.Select(t => new TarefaViewModel
            {
                TarefaId = t.Id,
                TarefaNome = t.Nome,
                IsAssigned = papel.PapelTarefa.Any(pt => pt.TarefaId == t.Id)
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(RoleCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            var papel = await _context.Papeis
                .Include(p => p.PapelTarefa)
                .FirstOrDefaultAsync(p => p.Id == model.Id);

            if (papel == null)
            {
                return NotFound();
            }

            papel.Nome = model.Nome;

            // Limpar tarefas atuais do papel
            papel.PapelTarefa.Clear();

            // Adicionar novas tarefas e definir explicitamente o PapelId
            foreach (var tarefa in model.Tarefas.Where(t => t.IsAssigned))
            {
                var papelTarefa = new PapelTarefa
                {
                    PapelId = papel.Id, // Atribui o ID do papel atual
                    TarefaId = tarefa.TarefaId
                };
                papel.PapelTarefa.Add(papelTarefa);
            }

            _context.Update(papel);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        return View(model);
    }


    private bool RoleExists(int id)
    {
        return _context.Papeis.Any(e => e.Id == id);
    }
}
