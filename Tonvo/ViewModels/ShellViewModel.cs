using ReactiveUI.Fody.Helpers;

namespace Tonvo.ViewModels
{
    static class UnsafeNativeMethods { [DllImport("user32.dll")] public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam); }

    internal partial class ShellViewModel : ReactiveObject
    {
        private readonly INavigationService _navigationService;
        private Frame _mainFrame;

        #region Properties
        public Frame MainFrame
        {
            get { return _mainFrame; }
        }

        public ReactiveCommand<Unit, Unit> MoveWindowCommand { get; }
        public ReactiveCommand<Unit, Unit> ShutdownWindowCommand { get; }
        public ReactiveCommand<Unit, Unit> MaximizeWindowCommand { get; }
        public ReactiveCommand<Unit, Unit> MinimizeWindowCommand { get; }
        public ReactiveCommand<Unit, Unit> ControlBarMouseEnter { get; }
        #endregion Properties

        public ShellViewModel(INavigationService navigationService, Frame mainFrame)
        {
            _navigationService = navigationService;
            _mainFrame = mainFrame;
            _navigationService.NavigateToPage(_mainFrame, "RootView");

            // Перемещение окна
            MoveWindowCommand = ReactiveCommand.Create(() =>
            {
                WindowInteropHelper helper = new(Application.Current.MainWindow);
                UnsafeNativeMethods.SendMessage(helper.Handle, 161, new IntPtr(2), IntPtr.Zero);
            });

            // Для нормальной работы на компьютерах с несколькими мониторами
            ControlBarMouseEnter = ReactiveCommand.Create(() =>
            {
                Application.Current.MainWindow.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            });

            // Завершение работы приложения
            ShutdownWindowCommand = ReactiveCommand.Create(() =>
            {
                Application.Current.Shutdown();
            });

            // Приложение на весь экран
            MaximizeWindowCommand = ReactiveCommand.Create(() =>
            {
                if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
                    Application.Current.MainWindow.WindowState = WindowState.Normal;
                else
                    Application.Current.MainWindow.WindowState = WindowState.Maximized;
            });

            // Свернуть приложение в панель задач
            MinimizeWindowCommand = ReactiveCommand.Create(() =>
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            });
        }
    }
}
