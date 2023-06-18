namespace Tonvo.Services
{
    /// <summary>
    /// Интерфейс сервиса для навигации по страницам.
    /// </summary>
    public interface INavigationService2
    {
        public event Action<UserControl>? onUserControlChanged;
        public void ChangePage(UserControl userControl);
    }
}
