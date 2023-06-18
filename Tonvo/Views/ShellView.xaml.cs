using Microsoft.Extensions.DependencyInjection;
using System;
using Wpf.Ui.Contracts;
using Wpf.Ui.Controls.Navigation;
namespace Tonvo.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView
    {
        internal Wpf.Ui.Controls.Navigation.NavigationView NavigationView;
        public ShellView(INavigationService navigationService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            navigationService.SetNavigationControl(NavigationView);
            NavigationView.SetServiceProvider(serviceProvider);
            NavigationView.Loaded += (_, _) => NavigationView.Navigate(typeof(ApplicantControlPanelView));
        }
    }
}
