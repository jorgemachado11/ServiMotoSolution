namespace ServiMotoServer.Models
{
    public class TaskAssignment
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public Task Task { get; set; }
        public Guid MotorcycleId { get; set; }
        public Motorcycle Motorcycle { get; set; }
        public DateTime AssignedAt { get; set; }
    }
}
