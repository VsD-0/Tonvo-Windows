namespace Tonvo.Services
{
    /// <summary>
    /// Сервис навигации по страницам.
    /// </summary>
    internal class NavigationService : INavigationService
    {
        #region Fields
        /// <summary>
        /// Словарь с ключами и страницами.
        /// </summary>
        private readonly Dictionary<string, Type> _pages;

        /// <summary>
        /// Фрейм для отображения страниц.
        /// </summary>
        private Frame? _frame;
        #endregion Fields

        /// <summary>
        /// Создает новый экземпляр класса <see cref="PageService"/>.
        /// </summary>
        public NavigationService()
        {
            _pages = new Dictionary<string, Type>
            {
                { nameof(ApplicantControlPanelView), typeof(ApplicantControlPanelView) },
                { nameof(ApplicantFieldsView), typeof(ApplicantFieldsView) },
                { nameof(BrowseListView), typeof(BrowseListView) },
                { nameof(CompanyControlPanelView), typeof(CompanyControlPanelView) },
                { nameof(CompanyFieldsView), typeof(CompanyFieldsView) },
                { nameof(ApplicantAccountView), typeof(ApplicantAccountView) },
                { nameof(CompanyAccountView), typeof(CompanyAccountView) },
                { nameof(RootView), typeof(RootView) },
                { nameof(ShellView), typeof(ShellView) },
                { nameof(SignInView), typeof(SignInView) },
                { nameof(SignUpView), typeof(SignUpView) },
                { nameof(VacancyFieldsView), typeof(VacancyFieldsView) }
            };
        }
        #region Commands

        /// <summary>
        /// Переход на страницу с указанным ключом.
        /// </summary>
        /// <param name="frame">Фрейм для отображения страниц.</param>
        /// <param name="key">Ключ страницы.</param>
        public async Task NavigateToPage(Frame frame, string key)
        {
            _frame = frame;
            await Task.Run(() => {
                Application.Current.Dispatcher.Invoke(() => _frame?.Navigate((UserControl?)Activator.CreateInstance(_pages[key])));
            });
        }

        /// <summary>
        /// Получение типа страницы с указанным ключом.
        /// </summary>
        /// <param name="key">Ключ страницы.</param>
        /// <returns>Тип страницы.</returns>
        public Type GetPage(string key) { return _pages[key]; }
        #endregion Commands
    }

}
