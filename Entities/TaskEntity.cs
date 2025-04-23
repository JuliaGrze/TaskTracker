using System.ComponentModel.DataAnnotations;

namespace Entities
{
    /// <summary>
    /// Domain model for Task
    /// </summary>
    public class TaskEntity
    {
        [Key]
        public Guid TaskID { get; set; }
        [StringLength(40)]
        public string Title { get; set; }
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        //public Taskstatus Status { get; set; }
        public string Status { get; set; }
    }
}
