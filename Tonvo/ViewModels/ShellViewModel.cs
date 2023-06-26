﻿using System.Windows.Controls;

namespace Tonvo.ViewModels
{
    static class UnsafeNativeMethods { [DllImport("user32.dll")] public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam); }

    internal partial class ShellViewModel : ReactiveObject
    {
        #region Fields
        private readonly INavigationService _navigationService;
        private Frame _mainFrame;
        #endregion Fields

        #region Properties
        public Frame MainFrame
        {
            get { return _mainFrame; }
        }

        [Reactive] public WindowState winState { get;  set; } = WindowState.Normal;

        /// <summary>
        /// Путь к иконке изменения состояния окна
        /// </summary>
        [Reactive] public string ChangeWindowStateIcon { get; set; } = @"\Resources\Icons\increase_window.png";

        /// <summary>
        /// Метод для перетаскивания окна
        /// </summary>
        public ReactiveCommand<Unit, Unit> MoveWindowCommand { get; }
        /// <summary>
        /// Метод для завершение работы приложения
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShutdownWindowCommand { get; }
        /// <summary>
        /// Метод для изменение состояния окна
        /// </summary>
        public ReactiveCommand<Unit, Unit> MaximizeWindowCommand { get; }
        /// <summary>
        /// Метод для сворачивания окна в панель задач
        /// </summary>
        public ReactiveCommand<Unit, Unit> MinimizeWindowCommand { get; }
        /// <summary>
        /// Метод для нормальной работы на компьютерах с несколькими мониторами
        /// </summary>
        public ReactiveCommand<Unit, Unit> ControlBarMouseEnter { get; }

        public ReactiveCommand<Unit, Unit> ShowVacanciesCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowApplicantsCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowPersonalAccountViewCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowSettingsViewCommand { get; }
        #endregion Properties


        public ShellViewModel(INavigationService navigationService, Frame mainFrame)
        {
            _navigationService = navigationService;
            _mainFrame = mainFrame;
            Task.Run(() => _navigationService.NavigateToPage(_mainFrame, "ApplicantControlPanelView"));

            this.WhenAnyValue(x => x.winState)
                .Subscribe(winState =>
                {
                    if (winState == WindowState.Maximized) ChangeWindowStateIcon = @"\Resources\Icons\decrease_window.png";
                    else ChangeWindowStateIcon = @"\Resources\Icons\increase_window.png";
                });

            //// Сохранение данных пользователя в app.config
            //Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //config.AppSettings.Settings["UserID"].Value = "Иван";
            //config.AppSettings.Settings["UserName"].Value = "Ivan";
            //config.AppSettings.Settings["Email"].Value = "ivan@mail.com";
            //// Другие свойства пользователя здесь
            //config.Save(ConfigurationSaveMode.Modified);
            //System.Configuration.ConfigurationManager.RefreshSection("appSettings");
            //string userID = System.Configuration.ConfigurationManager.AppSettings["UserID"];
            //string userName = System.Configuration.ConfigurationManager.AppSettings["UserName"];
            //string email = System.Configuration.ConfigurationManager.AppSettings["Email"];

            #region Commands
            ShowVacanciesCommand = ReactiveCommand.Create(() => { _navigationService.NavigateToPage(_mainFrame, "ApplicantControlPanelView"); });
            ShowApplicantsCommand = ReactiveCommand.Create(() => { _navigationService.NavigateToPage(_mainFrame, "CompanyControlPanelView"); });
            ShowPersonalAccountViewCommand = ReactiveCommand.Create(() => { _navigationService.NavigateToPage(_mainFrame, "PersonalAccountView"); });
            ShowSettingsViewCommand = ReactiveCommand.Create(() => { int a = 1; });

            MoveWindowCommand = ReactiveCommand.Create(() =>
            {
                WindowInteropHelper helper = new(Application.Current.MainWindow);
                UnsafeNativeMethods.SendMessage(helper.Handle, 161, new IntPtr(2), IntPtr.Zero);
            });
            ControlBarMouseEnter = ReactiveCommand.Create(() => { Application.Current.MainWindow.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight; });
            ShutdownWindowCommand = ReactiveCommand.Create(() => { Application.Current.Shutdown(); });
            MinimizeWindowCommand = ReactiveCommand.Create(() => { Application.Current.MainWindow.WindowState = WindowState.Minimized; });
            MaximizeWindowCommand = ReactiveCommand.Create(() =>
            {
                if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
                {
                    Application.Current.MainWindow.WindowState = WindowState.Normal;
                    //ChangeWindowStateIcon = @"\Resources\Icons\increase_window.png";
                }
                else
                {
                    Application.Current.MainWindow.WindowState = WindowState.Maximized;
                    //ChangeWindowStateIcon = @"\Resources\Icons\decrease_window.png";
                }
            });
            _mainFrame = mainFrame;
            #endregion Commands
        }
    }
}
