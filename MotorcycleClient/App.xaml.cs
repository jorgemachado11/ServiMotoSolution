using System.Diagnostics;

namespace MotorcycleClient
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

            MainPage = new AppShell(serviceProvider);
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;
            Debug.WriteLine($"Unhandled Exception: {exception.Message}");
            Debug.WriteLine($"Stack Trace: {exception.StackTrace}");
            // Optionally log the exception to a file or analytics service
        }

        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            var exception = e.Exception;
            Debug.WriteLine($"Unobserved Task Exception: {exception.Message}");
            Debug.WriteLine($"Stack Trace: {exception.StackTrace}");
            // Optionally log the exception to a file or analytics service
            e.SetObserved();
        }
    }
}
