using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministratorClient.Services.Tasks
{
    public class TaskService : ITaskService
    {
        public async System.Threading.Tasks.Task CreateNewTaskAsync(MobilityService.MobilityServiceClient client, string serviceId)
        {
            Console.WriteLine("Enter Task Name:");
            var taskName = Console.ReadLine();
            Console.WriteLine("Enter Task Description:");
            var taskDescription = Console.ReadLine();

            var createTaskResponse = await client.CreateTaskAsync(new CreateTaskRequest
            {
                TaskName = taskName,
                Description = taskDescription,
                ServiceId = serviceId
            });

            if (createTaskResponse.Success)
            {
                Console.WriteLine("Task created successfully!");
            }
            else
            {
                Console.WriteLine("Failed to create task: " + createTaskResponse.Message);
            }
        }
    }
}
