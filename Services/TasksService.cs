using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace Services
{
    public class TasksService : ITasksService
    {
        //private fields
        private List<TaskEntity> _tasks;
        public TasksService()
        {
            _tasks = new List<TaskEntity>();
        }

        public TaskResponse AddTask(TaskAddRequest? taskAddRequest)
        {
            //Validation: taskAddRequest can't be null
            if (taskAddRequest == null)
                throw new ArgumentNullException(nameof(taskAddRequest));

            //Validation: title can't be null
            if(String.IsNullOrEmpty(taskAddRequest.Title))
                throw new ArgumentException(nameof(taskAddRequest.Title));

            //Validation: title length can't be shorter than 3
            if(taskAddRequest.Title.Length < 3)
                throw new ArgumentException(nameof(TaskAddRequest.Title));

            //convert TaskAddRequest into Task type
            TaskEntity task = taskAddRequest.ToTask();

            //generate TaskID
            task.TaskID = Guid.NewGuid();

            //Add Task Entity obkect into _tasks list
            _tasks.Add(task);

            return task.ToTaskResponse();
        }

        public bool DeleteTask(Guid? taskId)
        {
            throw new NotImplementedException();
        }

        public List<TaskResponse> GetAllTasks()
        {
            return _tasks.Select(task => task.ToTaskResponse()).ToList();
        }

        public List<TaskResponse> GetFilteredTasks(string searchBy, string? searchString)
        {
            List<TaskResponse> allTasks = GetAllTasks();
            List<TaskResponse> matchingTasks = allTasks;

            if(string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
                return matchingTasks;

            switch (searchBy)
            {
                //Title
                case nameof(TaskEntity.Title):
                    matchingTasks = allTasks.Where(task => 
                        !string.IsNullOrEmpty(task.Title) &&
                        task.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
                //Description
                case nameof(TaskEntity.Description):
                    matchingTasks = allTasks.Where(task => 
                        !string.IsNullOrEmpty(task.Description) &&
                        task.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
                //CreatedDate
                case nameof(TaskEntity.CreatedDate):
                    if(DateTime.TryParse(searchString, out DateTime createdDate))
                    {
                        matchingTasks = allTasks.Where(task =>
                            task.CreatedDate.Date == createdDate.Date)
                            .ToList();
                    }
                    break;
                case nameof(TaskEntity.Status):
                    matchingTasks = allTasks.Where(task =>
                        task.Status.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
                default:
                    matchingTasks = allTasks;
                    break;
            }
            return matchingTasks;
        }

        public List<TaskResponse> GetSortedTasks(List<TaskResponse> allTasks, string sortBy, SortOrderEnum sortOrder)
        {
            throw new NotImplementedException();
        }

        public TaskResponse? GetTaskById(Guid? taskId)
        {
            //Validation: if taskID is null
            if(taskId == null)
                return null;

            //Get matching TaskResponse from List<TaskEntity> based on taskID
            TaskResponse? taskResponse = _tasks.FirstOrDefault(temp => temp.TaskID == taskId)?.ToTaskResponse();

            if(taskResponse == null) return null;

            return taskResponse;
        }


        public TaskResponse UpdateTask(TaskUpdateRequest? taskUpdateRequest)
        {
            throw new NotImplementedException();
        }
    }
}
