namespace Entities
{
    /// <summary>
    /// Domain model for Task
    /// </summary>
    public class TaskEntity
    {
        public Guid TaskID { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        //public Taskstatus Status { get; set; }
        public string Status { get; set; }
    }
}
