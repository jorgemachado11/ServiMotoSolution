using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ServiMotoServer;
using ServiMotoServer.Data;
using ServiMotoServer.Messaging.Dtos;
using ServiMotoServer.Models;

namespace ServiMotoServer.Services;

public class MobilityServiceImpl : MobilityService.MobilityServiceBase
{
    private readonly ApplicationDbContext _context;
    private readonly TaskPublisher _taskPublisher;

    public MobilityServiceImpl(ApplicationDbContext context, TaskPublisher taskPublisher)
    {
        _context = context;
        _taskPublisher = taskPublisher;
    }

    public override async Task<AuthResponse> Authenticate(AuthRequest request, ServerCallContext context)
    {
        try
        {
            Console.WriteLine($"Authentication attempt for user: {request.Username}");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || user.Password != request.Password)
            {
                Console.WriteLine("User not found.");
                return new AuthResponse { Success = false, Message = "Authentication failed" };
            }

            Console.WriteLine($"User found: {user.Username}, IsAdministrator: {user.IsAdministrator}");

            if (user.IsAdministrator)
            {
                var serviceAssignment = await _context.ServiceAssignments.FirstOrDefaultAsync(sa => sa.UserId == user.Id);
                if (serviceAssignment == null)
                {
                    Console.WriteLine("Administrator not associated with any service.");
                    return new AuthResponse { Success = false, Message = "Administrator not associated with any service" };
                }

                var service = await _context.Services.FindAsync(serviceAssignment.ServiceId);
                if (service == null)
                {
                    Console.WriteLine("Service not found.");
                    return new AuthResponse { Success = false, Message = "Service not found" };
                }

                Console.WriteLine($"Administrator associated with service: {service.ServiceName}");
                return new AuthResponse
                {
                    Success = true,
                    Message = "Authenticated",
                    UserId = user.Id.ToString(),
                    ServiceId = service.Id.ToString(),
                    ServiceName = service.ServiceName
                };
            }

            Console.WriteLine("User authenticated successfully.");
            return new AuthResponse { Success = true, Message = "Authenticated" };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during authentication: {ex.Message}");
            return new AuthResponse { Success = false, Message = "Authentication failed due to an error" };
        }
    }

    public override async Task<AuthResponse> AuthenticateMotorcycle(AuthRequest request, ServerCallContext context)
    {
        try
        {
            Console.WriteLine($"Authentication attempt for motorcycle: {request.Username}");

            var motorcycle = await _context.Motorcycles.FirstOrDefaultAsync(m => m.MotorcycleName == request.Username);
            if (motorcycle == null || motorcycle.Password != request.Password)
            {
                Console.WriteLine("Motorcycle not found or invalid password.");
                return new AuthResponse { Success = false, Message = "Authentication failed" };
            }

            var serviceAssignment = await _context.ServiceAssignments.FirstOrDefaultAsync(sa => sa.MotorcycleId == motorcycle.Id);
            if (serviceAssignment == null)
            {
                Console.WriteLine("Motorcycle not associated with any service.");
                return new AuthResponse { Success = false, Message = "Motorcycle not associated with any service" };
            }

            var service = await _context.Services.FindAsync(serviceAssignment.ServiceId);
            if (service == null)
            {
                Console.WriteLine("Service not found.");
                return new AuthResponse { Success = false, Message = "Service not found" };
            }

            Console.WriteLine($"Motorcycle associated with service: {service.ServiceName}");
            return new AuthResponse
            {
                Success = true,
                Message = "Authenticated",
                UserId = motorcycle.Id.ToString(),
                ServiceId = service.Id.ToString(),
                ServiceName = service.ServiceName
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during authentication: {ex.Message}");
            return new AuthResponse { Success = false, Message = "Authentication failed due to an error" };
        }
    }

    public override async Task<TaskResponse> CreateTask(CreateTaskRequest request, ServerCallContext context)
    {
        var task = new Models.Task
        {
            TaskName = request.TaskName,
            Description = request.Description,
            ServiceId = Guid.Parse(request.ServiceId),
            IsCompleted = false
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var taskDto = new TaskModelDto
        {
            TaskId = task.Id.ToString(),
            TaskName = task.TaskName,
            Description = task.Description,
            ServiceId = task.ServiceId.ToString(),
            IsCompleted = task.IsCompleted
        };

        _taskPublisher.PublishNewTask(taskDto);

        return new TaskResponse { Success = true, Message = "Task Created and Published" };
    }

    public override async Task<TaskResponse> AllocateTask(AllocateTaskRequest request, ServerCallContext context)
    {
        var task = await _context.Tasks.FindAsync(Guid.Parse(request.TaskId));
        if (task == null)
        {
            return new TaskResponse { Success = false, Message = "Task not found" };
        }

        var motorcycle = await _context.Motorcycles.FindAsync(Guid.Parse(request.ClientId));
        if (motorcycle == null)
        {
            return new TaskResponse { Success = false, Message = "Motorcycle not found" };
        }

        var taskAssignment = new TaskAssignment
        {
            TaskId = task.Id,
            MotorcycleId = motorcycle.Id,
            AssignedAt = DateTime.UtcNow
        };

        _context.TaskAssignments.Add(taskAssignment);
        await _context.SaveChangesAsync();

        return new TaskResponse { Success = true, Message = "Task Allocated" };
    }

    public override async Task<TaskResponse> CompleteTask(CompleteTaskRequest request, ServerCallContext context)
    {
        var task = await _context.Tasks.FindAsync(Guid.Parse(request.TaskId));
        if (task == null)
        {
            return new TaskResponse { Success = false, Message = "Task not found" };
        }

        task.IsCompleted = true;
        await _context.SaveChangesAsync();

        return new TaskResponse { Success = true, Message = "Task Completed" };
    }

    public override async Task<ServiceResponse> AssociateService(ServiceRequest request, ServerCallContext context)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.ClientId);
        if (user == null)
        {
            return new ServiceResponse { Success = false, Message = "User not found" };
        }

        var service = await _context.Services.FirstOrDefaultAsync(s => s.ServiceName == request.ServiceId);
        if (service == null)
        {
            return new ServiceResponse { Success = false, Message = "Service not found" };
        }

        var serviceAssignment = new ServiceAssignment
        {
            UserId = user.Id,
            ServiceId = service.Id
        };

        _context.ServiceAssignments.Add(serviceAssignment);
        await _context.SaveChangesAsync();

        return new ServiceResponse { Success = true, Message = "Service Associated" };
    }

    public override async Task<ServiceResponse> DisassociateService(ServiceRequest request, ServerCallContext context)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.ClientId);
        if (user == null)
        {
            return new ServiceResponse { Success = false, Message = "User not found" };
        }

        var service = await _context.Services.FirstOrDefaultAsync(s => s.ServiceName == request.ServiceId);
        if (service == null)
        {
            return new ServiceResponse { Success = false, Message = "Service not found" };
        }

        var serviceAssignment = await _context.ServiceAssignments
            .FirstOrDefaultAsync(sa => sa.UserId == user.Id && sa.ServiceId == service.Id);
        if (serviceAssignment == null)
        {
            return new ServiceResponse { Success = false, Message = "Service assignment not found" };
        }

        _context.ServiceAssignments.Remove(serviceAssignment);
        await _context.SaveChangesAsync();

        return new ServiceResponse { Success = true, Message = "Service Disassociated" };
    }

    public override async Task<AdminActionResponse> AdminAction(AdminActionRequest request, ServerCallContext context)
    {
        // Implement admin action logic
        return new AdminActionResponse { Success = true, Message = "Action Completed" };
    }

    public override async Task<TaskListResponse> ListTasks(TaskListRequest request, ServerCallContext context)
    {
        var tasks = await _context.Tasks
            .Where(t => t.ServiceId == Guid.Parse(request.ServiceId))
            .ToListAsync();

        var response = new TaskListResponse();
        foreach (var task in tasks)
        {
            var assignedMotorcycle = await _context.TaskAssignments
                .Where(ta => ta.TaskId == task.Id)
                .Select(ta => ta.Motorcycle != null ? ta.Motorcycle.Id.ToString() : string.Empty)
                .FirstOrDefaultAsync();

            response.Tasks.Add(new Task
            {
                Id = task.Id.ToString(),
                TaskName = task.TaskName,
                Description = task.Description,
                ServiceId = task.ServiceId.ToString(),
                ServiceName = _context.Services.FirstOrDefault(s => s.Id == task.ServiceId)?.ServiceName,
                IsCompleted = task.IsCompleted,
                AssignedTo = assignedMotorcycle ?? string.Empty
            });
        }
        return response;
    }

    public override async Task<ServiceListResponse> ListServices(ServiceListRequest request, ServerCallContext context)
    {
        var services = await _context.Services.ToListAsync();
        var response = new ServiceListResponse();
        response.Services.AddRange(services.Select(s => new Service
        {
            Id = s.Id.ToString(),
            ServiceName = s.ServiceName,
            Description = s.Description
        }));
        return response;
    }

    public override async Task<MotorcycleListResponse> ListMotorcycles(MotorcycleListRequest request, ServerCallContext context)
    {
        var motorcycles = await _context.Motorcycles.ToListAsync();
        var response = new MotorcycleListResponse();
        response.Motorcycles.AddRange(motorcycles.Select(m => new Motorcycle
        {
            Id = m.Id.ToString(),
            MotorcycleName = m.MotorcycleName,
            Description = m.Description
        }));
        return response;
    }

    public override async Task<ServiceResponse> UpdateService(UpdateServiceRequest request, ServerCallContext context)
    {
        var service = await _context.Services.FindAsync(Guid.Parse(request.ServiceId));
        if (service == null)
        {
            return new ServiceResponse { Success = false, Message = "Service not found" };
        }

        service.ServiceName = request.ServiceName;
        service.Description = request.Description;
        await _context.SaveChangesAsync();

        return new ServiceResponse { Success = true, Message = "Service Updated" };
    }

    public override async Task<ServiceResponse> QueryService(QueryServiceRequest request, ServerCallContext context)
    {
        var service = await _context.Services.FindAsync(Guid.Parse(request.ServiceId));
        if (service == null)
        {
            return new ServiceResponse { Success = false, Message = "Service not found" };
        }

        return new ServiceResponse { Success = true, Message = $"Service: {service.ServiceName}, Description: {service.Description}" };
    }

    public override async Task<AdminResponse> CreateAdmin(CreateAdminRequest request, ServerCallContext context)
    {
        var user = new Models.User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            Password = request.Password,
            IsAdministrator = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var serviceAssignment = new Models.ServiceAssignment
        {
            UserId = user.Id,
            ServiceId = Guid.Parse(request.ServiceId)
        };

        _context.ServiceAssignments.Add(serviceAssignment);
        await _context.SaveChangesAsync();

        return new AdminResponse { Success = true, Message = "Administrator created and associated with the service" };
    }

    public override async Task<MotorcycleResponse> CreateMotorcycle(CreateMotorcycleRequest request, ServerCallContext context)
    {
        var motorcycle = new Models.Motorcycle
        {
            Id = Guid.NewGuid(),
            MotorcycleName = request.MotorcycleName,
            Description = request.Description,
            Password = request.Password
        };

        _context.Motorcycles.Add(motorcycle);
        await _context.SaveChangesAsync();

        return new MotorcycleResponse { Success = true, Message = "Motorcycle created" };
    }
}