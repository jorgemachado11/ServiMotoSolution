namespace ServiMotoServer.Messaging.Dtos
{
    public class TaskModelDto
    {
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public string ServiceId { get; set; }
        public string ClientId { get; set; }
        public bool IsCompleted { get; set; }
    }
}
