using Microsoft.Extensions.Logging;
using MotorcycleClient.Services.Notifications;
using MotorcycleClient.Services.Auth;
using MotorcycleClient.Services.Tasks;
using MotorcycleClient.Services.ServicesManagements;

namespace MotorcycleClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            ConfigureServices(builder.Services);

            return builder.Build();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
#if WINDOWS
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<ITaskService, TaskService>();
            services.AddSingleton<IServiceManagementService, ServiceManagementService>();
            services.AddTransient<MainPage>();
#endif
        }
    }
}
