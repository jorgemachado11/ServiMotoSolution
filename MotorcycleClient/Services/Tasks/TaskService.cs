#if  WINDOWS
using System.Threading.Tasks;
using ServiMotoServer;

namespace MotorcycleClient.Services.Tasks
{
    public class TaskService : ITaskService
    {
        public async System.Threading.Tasks.Task<bool> AllocateTaskAsync(MobilityService.MobilityServiceClient client, string taskId, string clientId)
        {
            var allocateTaskResponse = await client.AllocateTaskAsync(new AllocateTaskRequest { TaskId = taskId, ClientId = clientId });

            return allocateTaskResponse.Success;
        }

        public async System.Threading.Tasks.Task<bool> CompleteTaskAsync(MobilityService.MobilityServiceClient client, string taskId, string clientId)
        {
            var completeTaskResponse = await client.CompleteTaskAsync(new CompleteTaskRequest { TaskId = taskId, ClientId = clientId });

            return completeTaskResponse.Success;
        }

        public async System.Threading.Tasks.Task<TaskListResponse> ListTasksAsync(MobilityService.MobilityServiceClient client, string serviceId)
        {
            var taskListResponse = await client.ListTasksAsync(new TaskListRequest { ServiceId = serviceId });
            return taskListResponse;
        }
    }
}
#endif
