using Microsoft.AspNetCore.Mvc;

public class DashboardController : Controller
{
    public IActionResult Index()
    {
        // Aqui você pode retornar uma view que mostra o dashboard
        return View();
    }
}
