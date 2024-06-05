namespace ServiMotoServer.Models
{
    public class ServiceAssignment
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public User User { get; set; }
        public Guid? MotorcycleId { get; set; }
        public Motorcycle Motorcycle { get; set; }
        public Guid ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
