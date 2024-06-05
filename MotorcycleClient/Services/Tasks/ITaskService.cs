namespace MotorcycleClient.Services.Tasks
{
    public interface ITaskService
    {
#if  WINDOWS
        System.Threading.Tasks.Task<bool> AllocateTaskAsync(MobilityService.MobilityServiceClient client, string taskId, string clientId);
        System.Threading.Tasks.Task<bool> CompleteTaskAsync(MobilityService.MobilityServiceClient client, string taskId, string clientId);
        System.Threading.Tasks.Task<TaskListResponse> ListTasksAsync(MobilityService.MobilityServiceClient client, string serviceId);
#endif
    }
}