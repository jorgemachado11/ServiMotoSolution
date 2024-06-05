namespace ServiMotoServer.Models
{
    public class Service
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public ICollection<Task> Tasks { get; set; }
        public ICollection<ServiceAssignment> ServiceAssignments { get; set; }
    }
}
