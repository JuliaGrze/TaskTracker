using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class TasksService : ITasksService
    {
        //private fields
        private readonly TaskDbContext _db;
        public TasksService(TaskDbContext taskDbContext)
        {
            _db = taskDbContext;
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
            _db.Tasks.Add(task);
            _db.SaveChanges();

            return task.ToTaskResponse();
        }

        public bool DeleteTask(Guid? taskId)
        {
            //check if taskID is not null
            if(taskId == null) 
                throw new ArgumentNullException(nameof(taskId));

            //Get the matching Task object from List<Task>
            TaskEntity? matchingTask = _db.Tasks.FirstOrDefault(task => task.TaskID == taskId);

            if(matchingTask == null)
                return false;

            //Delete the matching Task from List<Task>
            _db.Tasks.Remove(matchingTask);
            _db.SaveChanges();
            return true;
        }

        public List<TaskResponse> GetAllTasks()
        {
            // Wykonanie zapytania SQL do procedury składowanej GetAllTasks
            var tasks = _db.Tasks
                           .FromSqlRaw("EXEC GetAllTasks")  // execute stored procedure
                           .ToList();

            return tasks.Select(task => task.ToTaskResponse()).ToList();
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
                case nameof(TaskResponse.Title):
                    matchingTasks = allTasks.Where(task => 
                        !string.IsNullOrEmpty(task.Title) &&
                        task.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
                //Description
                case nameof(TaskResponse.Description):
                    matchingTasks = allTasks.Where(task => 
                        !string.IsNullOrEmpty(task.Description) &&
                        task.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    break;
                //CreatedDate
                case nameof(TaskResponse.CreatedDate):
                    if(DateTime.TryParse(searchString, out DateTime createdDate))
                    {
                        matchingTasks = allTasks.Where(task =>
                            task.CreatedDate.Date == createdDate.Date)
                            .ToList();
                    }
                    break;
                case nameof(TaskResponse.Status):
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
            if(sortBy == null || sortOrder == null)
                return allTasks;

            List<TaskResponse> sortedTasks = (sortBy, sortOrder) switch
            {
                //Title
                (nameof(TaskResponse.Title), SortOrderEnum.ASC) =>
                    allTasks.OrderBy(task => task.Title, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(TaskResponse.Title), SortOrderEnum.DESC) =>
                    allTasks.OrderByDescending(task => task.Title, StringComparer.OrdinalIgnoreCase).ToList(),

                //Description
                (nameof(TaskResponse.Description), SortOrderEnum.ASC) =>
                    allTasks.OrderBy(task => task.Description, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(TaskResponse.Description), SortOrderEnum.DESC) =>
                    allTasks.OrderByDescending(task => task.Description, StringComparer.OrdinalIgnoreCase).ToList(),

                //CreatedDate
                (nameof(TaskResponse.CreatedDate), SortOrderEnum.ASC) =>
                    allTasks.OrderBy(task => task.CreatedDate).ToList(),
                (nameof(TaskResponse.CreatedDate), SortOrderEnum.DESC) =>
                    allTasks.OrderByDescending(task => task.CreatedDate).ToList(),

                //Status
                (nameof(TaskResponse.Status), SortOrderEnum.ASC) =>
                    allTasks.OrderBy(task => (int)task.Status).ToList(),
                (nameof(TaskResponse.Status), SortOrderEnum.DESC) =>
                    allTasks.OrderByDescending(task => (int)task.Status).ToList()
            };

            return sortedTasks;
        }

        public TaskResponse? GetTaskById(Guid? taskId)
        {
            //Validation: if taskID is null
            if(taskId == null)
                return null;

            //Get matching TaskResponse from List<TaskEntity> based on taskID
            TaskResponse? taskResponse = _db.Tasks.FirstOrDefault(temp => temp.TaskID == taskId)?.ToTaskResponse();

            if(taskResponse == null) return null;

            return taskResponse;
        }


        public TaskResponse UpdateTask(TaskUpdateRequest? taskUpdateRequest)
        {
            if (taskUpdateRequest == null)
                throw new ArgumentNullException(nameof(taskUpdateRequest));

            // Validation
            ValidationHelpers.ModelValidation(taskUpdateRequest);

            // Get the matching TaskEntity object from the list based on TaskID
            TaskEntity? matchingTask = _db.Tasks.FirstOrDefault(task => task.TaskID == taskUpdateRequest.TaskID);

            // Check if the matching task is null
            if (matchingTask == null)
                throw new ArgumentException("Given Task ID doesn't exist!");

            // Update the details in the TaskEntity object
            matchingTask.Title = taskUpdateRequest.Title;
            matchingTask.Description = taskUpdateRequest.Description;
            matchingTask.Status = taskUpdateRequest.Status.ToString();

            //Ecectue Update in Database
            _db.SaveChanges();

            // Return the updated TaskResponse
            return matchingTask.ToTaskResponse();
        }

    }
}
