using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that is used as return type for most of TaskService methods
    /// </summary>
    public class TaskResponse
    {
        public Guid TaskID { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public Taskstatus Status { get; set; }
    }

    public static class TaskExtensions
    {
        public static TaskResponse ToTaskResponse(this TaskEntity task)
        {
            return new TaskResponse { 
                TaskID = task.TaskID,
                Title = task.Title,
                Description = task.Description,
                CreatedDate = task.CreatedDate,
                Status = Enum.TryParse<Taskstatus>(task.Status, out Taskstatus status) ? status : Taskstatus.Pending
            };
        }
    }
}
