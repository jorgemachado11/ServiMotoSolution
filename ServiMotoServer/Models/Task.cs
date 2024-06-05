namespace ServiMotoServer.Models
{
    public class Task
    {
        public Guid Id { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public Guid ServiceId { get; set; }
        public Service Service { get; set; }
        public bool IsCompleted { get; set; }
        public ICollection<TaskAssignment> TaskAssignments { get; set; }
    }
}
