using MotorcycleClient.Models;

namespace MotorcycleClient.Services.Notifications
{
    public interface INotificationService
    {
#if WINDOWS
        void SubscribeToTaskNotifications(string clientId, Action<TaskModel> onTaskReceived);
#endif
    }
}