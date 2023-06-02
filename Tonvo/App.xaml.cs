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
            ViewModelLocator.Init();
        }
    }
}