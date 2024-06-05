using Microsoft.Maui.Controls;
using MotorcycleClient.Services.Auth;
using MotorcycleClient.Services.Tasks;
using MotorcycleClient.Services.ServicesManagements;
using MotorcycleClient.Services.Notifications;
using MotorcycleClient.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

#if WINDOWS
using Grpc.Net.Client;
using ServiMotoServer;
#endif

namespace MotorcycleClient
{
    public partial class MainPage : ContentPage
    {
        private readonly IAuthService _authService;
        private readonly ITaskService _taskService;
        private readonly IServiceManagementService _serviceManagementService;
        private readonly INotificationService _notificationService;

#if WINDOWS
        private readonly MobilityService.MobilityServiceClient _client;
#endif

        private string _userId;
        private string _serviceId;
        private string _serviceName;
        public ObservableCollection<TaskModel> Tasks { get; private set; }
        public bool IsAuthenticated { get; private set; }
        public bool IsNotAuthenticated => !IsAuthenticated;

        public MainPage(
            IAuthService authService,
            ITaskService taskService,
            IServiceManagementService serviceManagementService,
            INotificationService notificationService)
        {
            try
            {
                InitializeComponent();

                _authService = authService;
                _taskService = taskService;
                _serviceManagementService = serviceManagementService;
                _notificationService = notificationService;

#if WINDOWS
                // Configure gRPC client
                var channel = GrpcChannel.ForAddress("http://localhost:8080");
                _client = new MobilityService.MobilityServiceClient(channel);
#endif

                Tasks = new ObservableCollection<TaskModel>();
                BindingContext = this;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in MainPage constructor: {ex.Message}");
                throw;
            }
        }

        private async void OnAuthenticateClicked(object sender, EventArgs e)
        {
#if WINDOWS
            var username = UsernameEntry.Text;
            var password = PasswordEntry.Text;

            var authResponse = await _authService.AuthenticateAsync(_client, username, password);
            if (authResponse.Success)
            {
                _userId = authResponse.UserId;
                _serviceId = authResponse.ServiceId;
                _serviceName = authResponse.ServiceName;
                IsAuthenticated = true;
                OnPropertyChanged(nameof(IsAuthenticated));
                OnPropertyChanged(nameof(IsNotAuthenticated));
                await DisplayAlert("Success", $"Authenticated to service: {_serviceName}", "OK");

                // Subscribe to task notifications for the authenticated service
                _notificationService.SubscribeToTaskNotifications(_serviceId, OnTaskReceived);

                await RefreshTasks();
            }
            else
            {
                await DisplayAlert("Error", authResponse.Message, "OK");
            }
#endif
        }

        private async void OnRefreshTasksClicked(object sender, EventArgs e)
        {
            await RefreshTasks();
        }

        private async System.Threading.Tasks.Task RefreshTasks()
        {
#if WINDOWS
            if (string.IsNullOrEmpty(_serviceId))
            {
                await DisplayAlert("Error", "Please authenticate first", "OK");
                return;
            }

            var response = await _taskService.ListTasksAsync(_client, _serviceId);
            Tasks.Clear();
            foreach (var task in response.Tasks)
            {
                Tasks.Add(new TaskModel
                {
                    Id = task.Id,
                    TaskName = task.TaskName,
                    Description = task.Description,
                    ServiceId = task.ServiceId,
                    IsCompleted = task.IsCompleted,
                    AssignedTo = task.AssignedTo
                });
            }
#endif
        }

        private async void OnTaskSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedTask = e.SelectedItem as TaskModel;
            if (selectedTask == null)
            {
                return;
            }

            string[] actions;

            if (!selectedTask.IsCompleted && string.IsNullOrEmpty(selectedTask.AssignedTo))
            {
                actions = new[] { "Allocate" };
            }
            else if (!selectedTask.IsCompleted && selectedTask.AssignedTo.ToLower() == _userId)
            {
                actions = new[] { "Complete" };
            }
            else
            {
                actions = new[] { "No actions available" };
            }

            await ContextMenu.Show(new ContextMenuOptions
            {
                Title = "Task Actions",
                Cancel = "Cancel",
                Actions = actions,
                OnActionSelected = async (action) =>
                {
                    switch (action)
                    {
                        case "Allocate":
                            await AllocateTask(selectedTask);
                            break;
                        case "Complete":
                            await CompleteTask(selectedTask);
                            break;
                    }
                }
            });

            // Deselect item
            TasksListView.SelectedItem = null;
        }

        private async System.Threading.Tasks.Task AllocateTask(TaskModel task)
        {
#if WINDOWS
            if (await _taskService.AllocateTaskAsync(_client, task.Id, _userId))
            {
                await DisplayAlert("Success", "Task Allocated", "OK");
                await RefreshTasks();
            }
            else
            {
                await DisplayAlert("Error", "Task Allocation failed", "OK");
            }
#endif
        }

        private async System.Threading.Tasks.Task CompleteTask(TaskModel task)
        {
#if WINDOWS
            if (await _taskService.CompleteTaskAsync(_client, task.Id, _userId))
            {
                await DisplayAlert("Success", "Task Completed", "OK");
                await RefreshTasks();
            }
            else
            {
                await DisplayAlert("Error", "Task Completion failed", "OK");
            }
#endif
        }

        private void OnTaskReceived(TaskModel task)
        {
            // Handle received task notification
            Dispatcher.Dispatch(async () =>
            {
                await DisplayAlert("New Task", $"Task: {task.TaskName}", "OK");
                await RefreshTasks();
            });
        }
    }
}
