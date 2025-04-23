using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

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
        public IActionResult Index(string searchBy, string? searchString, string sortBy, SortOrderEnum? sortOrder)
        {
            List<TaskResponse> allTasks = _tasksService.GetAllTasks();

            // 1. Filtr
            if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchString))
            {
                allTasks = _tasksService.GetFilteredTasks(searchBy, searchString);
            }

            // 2. Sort
            if (!string.IsNullOrEmpty(sortBy) && sortOrder != null)
            {
                allTasks = _tasksService.GetSortedTasks(allTasks, sortBy, sortOrder.Value);
            }

            // Przekazanie wartości do ViewBag
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(TaskResponse.Title), "Title"},
                {nameof(TaskResponse.Description), "Description"},
                {nameof(TaskResponse.CreatedDate), "Created Date"},
                {nameof(TaskResponse.Status), "Status"},
            };
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;

            return View(allTasks);
        }

        [Route("tasks/create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("tasks/create")]
        [HttpPost]
        public IActionResult Create(TaskAddRequest taskAddRequest)
        {
            //check if inputs are valid
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return View(taskAddRequest);
            }


            //create new task
            _tasksService.AddTask(taskAddRequest);

            return  RedirectToAction("Index");
        }
    }
}
