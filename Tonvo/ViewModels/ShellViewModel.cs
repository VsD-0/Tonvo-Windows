using ReactiveUI.Fody.Helpers;
using System.Diagnostics;
using System.Reactive;
using System.Windows.Interop;
using Tonvo;
using System.Runtime.InteropServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Tonvo.ViewModels
{
    static class UnsafeNativeMethods { [DllImport("user32.dll")] public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam); }

    internal partial class ShellViewModel : ReactiveObject
    {
        public ViewModelBase Global { get; } = ViewModelBase.Instance;

        #region Properties
        public ReactiveCommand<Unit, Unit> MoveWindowCommand { get; }
        public ReactiveCommand<Unit, Unit> ShutdownWindowCommand { get; }
        public ReactiveCommand<Unit, Unit> MaximizeWindowCommand { get; }
        public ReactiveCommand<Unit, Unit> MinimizeWindowCommand { get; }
        public ReactiveCommand<Unit, Unit> ControlBarMouseEnter { get; }

        public RootViewModel RootVM { get; set; }
        [Reactive]
        public object CurrentView { get; set; }
        #endregion Properties

        public ShellViewModel()
        {
            RootVM = new RootViewModel();
            ViewModelBase.CurrentView = RootVM;
            CurrentView = ViewModelBase.CurrentView;

            // Приложение не перекрывает панель задач
            Application.Current.MainWindow.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

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

            ViewModelBase.onViewUpdate.Add(OnUpdate);
        }

        void OnUpdate()
        {
            CurrentView = ViewModelBase.CurrentView;
        }
    }
}
