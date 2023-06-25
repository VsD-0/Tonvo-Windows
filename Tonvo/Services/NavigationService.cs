namespace Tonvo.Services
{
    
    public interface INavigationServiceForBrowse
    {
        event Action<UserControl>? onUserControlChanged;
        void ChangePage(UserControl userControl);
    }

    public interface INavigationServiceForControl
    {
        event Action<UserControl>? onUserControlChanged;
        void ChangePage(UserControl userControl);
    }
    /// <summary>
    /// Сервис навигации по страницам.
    /// </summary>
    public class NavigationService : INavigationServiceForBrowse, INavigationServiceForControl, INavigationService
    {
        public event Action<UserControl>? onUserControlChanged;

        public void ChangePage(UserControl userControl)
        {
            onUserControlChanged?.Invoke(userControl);
        }
    }

}
