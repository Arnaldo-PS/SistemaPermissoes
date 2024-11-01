using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// String de Conexão ao MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configuração do DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql("server=localhost;database=SistemaPermissoes;user=root;password=lojalolis",
    new MySqlServerVersion(new Version(9, 1, 0))));

// Configuração de Autenticação usando Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // Defina o caminho para a página de login
        options.LogoutPath = "/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

// Configuração de Autorização
builder.Services.AddAuthorization();

// Adiciona suporte ao MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuração do pipeline de middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Autenticação
app.UseAuthorization();  // Autorização

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
