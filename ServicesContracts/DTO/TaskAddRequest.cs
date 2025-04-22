using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class for adding a new task
    /// </summary>
    public class TaskAddRequest
    {
        public string Title { get; set; }
        public string? Description { get; set; }

        public TaskEntity ToTask()
        {
            return new TaskEntity() { Title = Title, Description = Description };
        }
    }
}
