using Microsoft.AspNetCore.Mvc;

namespace TaskTracker.Controllers
{
    public class TaskController : Controller
    {
        [Route("tasks/index")]
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
