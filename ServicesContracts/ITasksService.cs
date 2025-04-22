using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    /// <summary>
    /// Interface for the Task service that provides task-related operations.
    /// </summary>
    public interface ITasksService
    {
        /// <summary>
        /// Adds a new task to the system.
        /// </summary>
        /// <param name="taskAddRequest">The details of the task to be added (Title, Description, etc.)</param>
        /// <returns>A response containing the task details after it has been added, including the task ID and creation date.</returns>
        TaskResponse AddTask(TaskAddRequest? taskAddRequest);

        /// <summary>
        /// Returns all tasks in the system.
        /// </summary>
        /// <returns>A list of task responses containing details of all tasks in the system.</returns>
        List<TaskResponse> GetAllTasks();

        /// <summary>
        /// Retrieves a task by its unique ID.
        /// </summary>
        /// <param name="taskId">The ID of the task to retrieve.</param>
        /// <returns>A response containing the task details, or null if the task is not found.</returns>
        TaskResponse? GetTaskById(Guid? taskId);

        /// <summary>
        /// Updates an existing task with new details.
        /// </summary>
        /// <param name="taskUpdateRequest">The task details to update (TaskID, Title, Description, Status, etc.)</param>
        /// <returns>A response containing the updated task details.</returns>
        TaskResponse UpdateTask(TaskUpdateRequest? taskUpdateRequest);

        /// <summary>
        /// Deletes a task from the system by its ID.
        /// </summary>
        /// <param name="taskId">The ID of the task to delete.</param>
        /// <returns>True if the task was successfully deleted, otherwise false.</returns>
        bool DeleteTask(Guid taskId);

        /// <summary>
        /// Retrieves tasks filtered by search criteria and status.
        /// </summary>
        /// <param name="searchBy">The search string to filter tasks by title or description.</param>
        /// <param name="taskstatus">The status of the tasks to filter (Pending, InProgress, Completed).</param>
        /// <returns>A list of task responses that match the search and status criteria.</returns>
        List<TaskResponse> GetFilteredTasks(string searchBy, Taskstatus taskstatus);

        /// <summary>
        /// Retrieves tasks sorted by a specified field and order.
        /// </summary>
        /// <param name="allTasks">A list of tasks to be sorted.</param>
        /// <param name="sortBy">The field to sort by (e.g., Title, CreatedDate, Status).</param>
        /// <param name="sortOrder">The sort order (ascending or descending).</param>
        /// <returns>A sorted list of task responses based on the specified criteria.</returns>
        List<TaskResponse> GetSortedTasks(List<TaskResponse> allTasks, string sortBy, SortOrderEnum sortOrder);
    }
}
 