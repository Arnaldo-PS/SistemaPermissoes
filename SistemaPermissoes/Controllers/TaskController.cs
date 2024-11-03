using Microsoft.AspNetCore.Mvc;

public class TaskController : Controller
{
    public IActionResult Index()
    {
        // Aqui você pode retornar uma view que lista usuários, papéis, ou tarefas
        return View();
    }
}
