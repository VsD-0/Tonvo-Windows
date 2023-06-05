namespace Tonvo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            XamlDisplay.Init();
            Bootstrapper.Init();
            ViewModelLocator.Init(Bootstrapper.Provider, Bootstrapper.Services);

            this.MainWindow = new ShellView();
            // Приложение не перекрывает панель задач
            MainWindow.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MainWindow.Show();
        }
    }
}