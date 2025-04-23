using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace TaskTracker.Controllers
{
    [Route("[controller]")]
    public class TaskController : Controller
    {
        //private fields
        private ITasksService _tasksService;

        //constructor
        public TaskController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        [Route("[action]")]
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

        [Route("[action]")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("[action]")]
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

        //EDIT
        [Route("[action]/{taskID}")]
        [HttpGet]
        public IActionResult Edit(Guid taskID)
        {
            TaskResponse? taskResponse = _tasksService.GetTaskById(taskID);
            if (taskResponse == null)
                return RedirectToAction("Index");

            TaskUpdateRequest taskUpdateRequest = taskResponse.ToTaskUpdateRequest();
            return View(taskUpdateRequest);
        }

        [Route("[action]/{taskID}")]
        [HttpPost]
        public IActionResult Edit(Guid taskID, TaskUpdateRequest taskUpdateRequest)
        {
            TaskResponse? taskResponse = _tasksService.GetTaskById(taskID);

            if (taskResponse == null)
                return RedirectToAction("Index");

            //check if inputs are valid
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return View(taskUpdateRequest);
            }

            //UPDATE
            _tasksService.UpdateTask(taskUpdateRequest);

            return RedirectToAction("Index");
        }

        [Route("[action]/{taskID}")]
        [HttpGet]
        public IActionResult Delete(Guid taskID)
        {
            TaskResponse? taskResponse = _tasksService.GetTaskById(taskID);

            //validation: check if taskresposne is null
            if (taskResponse == null)
                return RedirectToAction("Index");

            return View(taskResponse);
        }

        [Route("[action]/{taskID}")]
        [HttpPost]
        public IActionResult Delete(TaskResponse taskResponse)
        {
            //validation: check if taskresposne is null
            if (taskResponse == null)
                return RedirectToAction("Index");

            _tasksService.DeleteTask(taskResponse.TaskID);
            return RedirectToAction("Index");
        }
    }
}
