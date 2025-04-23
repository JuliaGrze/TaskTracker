using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class for adding a new task
    /// </summary>
    public class TaskAddRequest
    {
        [Required(ErrorMessage = "Title can't be blank")]
        [MinLength(3, ErrorMessage = "Title can't be shorter than 3 characters")]
        public string Title { get; set; }
        public string? Description { get; set; }
        public Taskstatus Status { get; set; } = Taskstatus.Pending;

        public TaskEntity ToTask()
        {
            return new TaskEntity() { Title = Title, Description = Description, Status = Status.ToString() };
        }
    }
}
