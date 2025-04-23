using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;

namespace TaskTracker.Controllers
{
    public class TaskController : Controller
    {
        //private fields
        private ITasksService _tasksService;

        //constructor
        public TaskController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        [Route("tasks/index")]
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchString)
        {
           
            //Search dictionary
            //Actual Name - Display Name
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(TaskResponse.Title), "Title"},
                {nameof(TaskResponse.Description), "Description"},
                {nameof(TaskResponse.CreatedDate), "Created Date"},
                {nameof(TaskResponse.Status), "Status"},
            };

            //Save search fields adter searching
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //Filtered task list or all tasks
            List<TaskResponse> tasks = _tasksService.GetFilteredTasks(searchBy, searchString);

            return View(tasks);
        }
    }
}
