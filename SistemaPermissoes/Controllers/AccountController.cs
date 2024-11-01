﻿using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace SistemaPermissoes.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
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
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            // Verifica se o usuário existe e se a senha é igual à armazenada
            if (usuario != null && usuario.SenhaHash == senha) // Compare diretamente
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email)
                };

                // Adiciona os papéis do usuário como claims
                foreach (var papel in usuario.Papeis)
                {
                    claims.Add(new Claim(ClaimTypes.Role, papel.Nome));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    });

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Login inválido");
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
