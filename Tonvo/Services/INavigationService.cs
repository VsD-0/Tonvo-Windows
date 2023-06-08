namespace Tonvo.Services
{
    /// <summary>
    /// Интерфейс сервиса для навигации по страницам.
    /// </summary>
    public interface INavigationService
    {
        public event Action<UserControl>? onUserControlChanged;
        public void ChangePage(UserControl userControl);
    }
}
