using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdministratorClient.Services.Auth;
using AdministratorClient.Services.ServicesManagements;
using AdministratorClient.Services.Tasks;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    private static Dictionary<string, string> _taskNameToIdMap = new Dictionary<string, string>();
    private static string _serviceId;
    private static string _serviceName;

    static async System.Threading.Tasks.Task Main(string[] args)
    {
        // Configure DI
        var serviceProvider = CreateDIServices();

        var authService = serviceProvider.GetService<IAuthService>();
        var taskService = serviceProvider.GetService<ITaskService>();
        var serviceManagementService = serviceProvider.GetService<IServiceManagementService>();

        // Configure gRPC client
        var channel = GrpcChannel.ForAddress("http://localhost:8080");
        var client = new MobilityService.MobilityServiceClient(channel);

        // Authenticate Administrator
        var authResponse = await authService.AuthenticateAsync(client);
        if (!authResponse.Success)
        {
            Console.WriteLine("Authentication failed.");
            return;
        }

        _serviceId = authResponse.ServiceId;
        _serviceName = authResponse.ServiceName;
        Console.WriteLine($"Authenticated as administrator for service: {_serviceName}");

        // Load tasks for the service
        await LoadTasks(client);

        // Main loop for Administrator commands
        bool running = true;
        while (running)
        {
            Console.WriteLine("Choose an action:");
            Console.WriteLine("1. List Service Information");
            Console.WriteLine("2. Update Service Information");
            Console.WriteLine("3. Create New Task");
            Console.WriteLine("4. Create New Administrator");
            Console.WriteLine("5. Create New Motorcycle");
            Console.WriteLine("6. Exit");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await ListServiceInformation(client);
                    break;
                case "2":
                    await UpdateServiceInformation(client);
                    break;
                case "3":
                    await taskService.CreateNewTaskAsync(client, _serviceId);
                    break;
                case "4":
                    await CreateNewAdministrator(client);
                    break;
                case "5":
                    await CreateNewMotorcycle(client);
                    break;
                case "6":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static ServiceProvider CreateDIServices()
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IAuthService, AuthService>()
            .AddSingleton<ITaskService, TaskService>()
            .AddSingleton<IServiceManagementService, ServiceManagementService>()
            .BuildServiceProvider();

        return serviceProvider;
    }

    private static async System.Threading.Tasks.Task LoadTasks(MobilityService.MobilityServiceClient client)
    {
        var response = await client.ListTasksAsync(new TaskListRequest { ServiceId = _serviceId });
        _taskNameToIdMap.Clear();
        foreach (var task in response.Tasks)
        {
            _taskNameToIdMap[task.TaskName] = task.Id;
        }
    }

    private static async System.Threading.Tasks.Task ListServiceInformation(MobilityService.MobilityServiceClient client)
    {
        var response = await client.QueryServiceAsync(new QueryServiceRequest { ServiceId = _serviceId });
        Console.WriteLine(response.Message);
    }

    private static async System.Threading.Tasks.Task UpdateServiceInformation(MobilityService.MobilityServiceClient client)
    {
        Console.WriteLine("Enter new Service Description:");
        var description = Console.ReadLine();

        var response = await client.UpdateServiceAsync(new UpdateServiceRequest { ServiceId = _serviceId, ServiceName = _serviceName, Description = description });
        Console.WriteLine(response.Message);
    }

    private static async System.Threading.Tasks.Task CreateNewAdministrator(MobilityService.MobilityServiceClient client)
    {
        Console.WriteLine("Enter Administrator Username:");
        var username = Console.ReadLine();
        Console.WriteLine("Enter Email:");
        var email = Console.ReadLine();
        Console.WriteLine("Enter Password:");
        var password = Console.ReadLine();

        var response = await client.CreateAdminAsync(new CreateAdminRequest
        {
            Username = username,
            Email = email,
            Password = password,
            ServiceId = _serviceId
        });

        Console.WriteLine(response.Message);
    }

    private static async System.Threading.Tasks.Task CreateNewMotorcycle(MobilityService.MobilityServiceClient client)
    {
        Console.WriteLine("Enter Motorcycle Name:");
        var motorcycleName = Console.ReadLine();

        Console.WriteLine("Enter Motorcycle Password:");
        var password = Console.ReadLine();

        var response = await client.CreateMotorcycleAsync(new CreateMotorcycleRequest
        {
            MotorcycleName = motorcycleName,
            Password = password
        });

        Console.WriteLine(response.Message);
    }
}
