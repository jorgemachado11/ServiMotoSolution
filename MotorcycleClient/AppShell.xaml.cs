namespace MotorcycleClient
{
    public partial class AppShell : Shell
    {
        public AppShell(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            var mainPage = serviceProvider.GetRequiredService<MainPage>();
            Items.Add(new ShellContent
            {
                Title = "Home",
                Content = mainPage,
                Route = "MainPage"
            });
        }
    }
}
