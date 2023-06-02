namespace Tonvo.Services
{
    /// <summary>
    /// Интерфейс сервиса для навигации.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Асинхронная навигация к заданной странице.
        /// </summary>
        /// <param name="frame">Оболочка, в которой осуществляется навигация.</param>
        /// <param name="key">Ключ для определения страницы, к которой нужно перейти.</param>
        Task NavigateToPage(Frame frame, string key);

        /// <summary>
        /// Возвращает тип страницы по заданному ключу.
        /// </summary>
        /// <param name="key">Ключ для определения страницы.</param>
        /// <returns>Тип страницы.</returns>
        Type GetPage(string key);
    }
}
