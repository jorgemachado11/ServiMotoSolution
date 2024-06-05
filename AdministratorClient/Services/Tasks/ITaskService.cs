namespace AdministratorClient.Services.Tasks
{
    public interface ITaskService
    {
        System.Threading.Tasks.Task CreateNewTaskAsync(MobilityService.MobilityServiceClient client, string serviceId);
    }
}