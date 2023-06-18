namespace Tonvo.Services
{
    /// <summary>
    /// Сервис навигации по страницам.
    /// </summary>
    public class NavigationService2  :INavigationService2
    {
        public event Action<UserControl>? onUserControlChanged;

        public void ChangePage(UserControl userControl)
        {
            onUserControlChanged?.Invoke(userControl);
        }
    }
}
