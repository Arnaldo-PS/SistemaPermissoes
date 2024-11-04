using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaPermissoes.Models;

public class UserController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Buscar todos os usuários do banco de dados e mapear para o UsuarioViewModel
        var usuarios = await _context.Usuarios
            .Include(u => u.UsuarioPapel) // Inclui os papéis associados ao usuário
                .ThenInclude(up => up.Papel) // Inclui a tabela de papéis
            .Select(u => new UserViewModel
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Papeis = u.UsuarioPapel.Select(up => up.Papel.Nome).ToList() // Coleta os nomes dos papéis
            })
            .ToListAsync();

        return View(usuarios);
    }

    // Ação para exibir o formulário de criação
    [HttpGet]
    public IActionResult Create()
    {
        return View(); // Retorna a view sem papéis
    }

    // Ação para processar o formulário de criação
    [HttpPost]
    public async Task<IActionResult> Create(UserCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            var usuario = new Usuario
            {
                Nome = model.Nome,
                Email = model.Email,
                Senha = model.Senha,
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        return View(model); // Retorna a view com o modelo para corrigir os erros, se houver
    }


    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }

        var viewModel = new UserEditViewModel
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UserEditViewModel model)
    {
        if (ModelState.IsValid)
        {
            var usuario = await _context.Usuarios.FindAsync(model.Id);
            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Nome = model.Nome;
            usuario.Email = model.Email;

            // Atualize a senha apenas se o usuário preencher os campos de senha
            if (!string.IsNullOrEmpty(model.Senha) && model.Senha == model.ConfirmarSenha)
            {
                usuario.Senha = model.Senha; // Adicione lógica de criptografia aqui, se necessário
            }

            _context.Update(usuario);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        return View(model); // Em caso de erro, retorna à view de edição
    }

    [HttpGet]
    public async Task<IActionResult> Permissions()
    {
        var usuarios = await _context.Usuarios
            .Include(u => u.UsuarioPapel)
                .ThenInclude(up => up.Papel)
            .Select(u => new UserViewModel
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Papeis = u.UsuarioPapel.Select(up => up.Papel.Nome).ToList()
            })
            .ToListAsync();

        return View(usuarios); // Retorna a view com a lista de usuários
    }

    [HttpGet]
    public async Task<IActionResult> AddPermissions(int id)
    {
        var usuario = await _context.Usuarios
        .Include(u => u.UsuarioPapel) // Incluir os papéis associados
        .ThenInclude(up => up.Papel) // Incluir a tabela de papéis
        .FirstOrDefaultAsync(u => u.Id == id);

        if (usuario == null)
        {
            return NotFound(); // Retornar 404 se o usuário não for encontrado
        }

        // Obter todos os papéis disponíveis
        var papeis = await _context.Papeis.ToListAsync();

        // Criar um ViewModel para passar os dados para a view
        var viewModel = new AssignPermissionsViewModel
        {
            UsuarioId = usuario.Id,
            NomeUsuario = usuario.Nome,
            Papeis = papeis.Select(p => new PapelViewModel
            {
                Id = p.Id,
                Nome = p.Nome,
                IsAssigned = usuario.UsuarioPapel.Any(up => up.PapelId == p.Id) // Verifica se o papel já está atribuído
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SavePermissions(int usuarioId, int[] papeis)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.UsuarioPapel) // Incluir os papéis associados
            .FirstOrDefaultAsync(u => u.Id == usuarioId);

        if (usuario == null)
        {
            return NotFound();
        }

        // Limpar papéis existentes
        usuario.UsuarioPapel.Clear();

        // Adicionar novos papéis
        foreach (var papelId in papeis)
        {
            var usuarioPapel = new UsuarioPapel
            {
                UsuarioId = usuarioId,
                PapelId = papelId
            };
            usuario.UsuarioPapel.Add(usuarioPapel);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Permissions"); // Redireciona de volta à lista de usuários
    }
}
