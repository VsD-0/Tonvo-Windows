namespace Tonvo.ViewModels
{
    public class RootViewModel : ViewModelBase
    {
        #region Fields
        private readonly INavigationServiceForBrowse _navigationServiceForBrowse;
        private readonly INavigationServiceForControl _navigationServiceForControl;
        private readonly IMessageBus _messageBus;
        #endregion Fields

        #region Properties
        public UserControl? BrowseListSource { get; set; }
        public UserControl? ControlPanelSource { get; set; }
        #endregion Properties

        private void ChangeControlPanel(UserControl userControl)
        {
            ControlPanelSource = userControl;
            _navigationServiceForControl.ChangePage(userControl);
        }
        

        public RootViewModel(INavigationServiceForBrowse navigationServiceForBrowse, INavigationServiceForControl navigationServiceForControl, IMessageBus messageBus)
        {
            _navigationServiceForBrowse = navigationServiceForBrowse;
            _navigationServiceForControl = navigationServiceForControl;
            _messageBus = messageBus;

            _navigationServiceForBrowse.onUserControlChanged += (usercontrol) => BrowseListSource = usercontrol;
            _navigationServiceForBrowse.ChangePage(new BrowseListView());

            ChangeControlPanel(new ApplicantControlPanelView());

            _messageBus.Listen<Messages>()
                       .DistinctUntilChanged()
                       .Where(message => message != null)
                       .Subscribe(message =>
                       {
                           if (message.SelectedList == 0) ChangeControlPanel(new CompanyControlPanelView());
                           else if (message.SelectedList == 1) ChangeControlPanel(new ApplicantControlPanelView());
                           else throw new Exception("ListNotFound");
                       });
        }
    }
}
