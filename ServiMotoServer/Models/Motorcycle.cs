namespace ServiMotoServer.Models
{
    public class Motorcycle
    {
        public Guid Id { get; set; }
        public string MotorcycleName { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public ICollection<ServiceAssignment> ServiceAssignments { get; set; }
        public ICollection<TaskAssignment> TaskAssignments { get; set; }
    }
}
