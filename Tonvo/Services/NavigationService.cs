namespace Tonvo.Services
{
    /// <summary>
    /// Сервис навигации по страницам.
    /// </summary>
    public class NavigationService  :INavigationService
    {
        public event Action<UserControl>? onUserControlChanged;

        public void ChangePage(UserControl userControl)
        {
            onUserControlChanged?.Invoke(userControl);
        }
    }
}
