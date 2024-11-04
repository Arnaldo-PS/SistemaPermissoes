using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SistemaPermissoes.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Exibe a página de login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Processa o login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string senha)
        {
            try
            {
                var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

                if (usuario == null)
                {
                    throw new Exception("E-mail não encontrado.");
                }

                if (usuario.Senha != senha)
                {
                    throw new Exception("Senha incorreta.");
                }

                // Se chegou aqui, o usuário e a senha estão corretos
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Email),
                    new Claim("Id", usuario.Id.ToString()) // Adiciona o Handle como claim
                };

                // Cria a identidade do usuário
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Realiza o login do usuário
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Adiciona o erro específico ao ModelState para exibir na tela
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }

        // Realiza o logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
