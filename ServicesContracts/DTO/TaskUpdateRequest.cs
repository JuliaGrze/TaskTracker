using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    ///  Represents the DTO class that contains the task details to update
    /// </summary>
    public class TaskUpdateRequest
    {
        public Guid TaskID { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Taskstatus Status { get; set; }

        /// <summary>
        /// Converts the current object of TaskUpdateRequest into a new object of Task type
        /// </summary>
        /// <returns>Returns Task object</returns>

        public TaskEntity ToTask()
        {
            return new TaskEntity
            {
                TaskID = TaskID,
                Title = Title,
                Description = Description,
                Status = Convert.ToString(Status)
            };
        }
    }
}
