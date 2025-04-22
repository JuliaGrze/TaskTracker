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

        //It compares the current object to another object of TaskResponse type and return true, if both vales are same; otherwise returns false
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(TaskResponse)) return false;
            TaskResponse taskResponse = (TaskResponse)obj;
            return this.TaskID == taskResponse.TaskID && this.Title == taskResponse.Title && this.Description == taskResponse.Description && this.CreatedDate == taskResponse.CreatedDate && this.Status == taskResponse.Status;
        }
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
