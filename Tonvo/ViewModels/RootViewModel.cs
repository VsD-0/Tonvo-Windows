using Tonvo.Core;

namespace Tonvo.ViewModels
{
    public class RootViewModel : ViewModelBase
    {
        #region Fields
        private readonly INavigationService _navigationService;
        private readonly IMessageBus _messageBus;
        #endregion Fields

        #region Properties
        public UserControl? BrowseListSource { get; set; }
        public UserControl? ControlPanelSource { get; set; }
        #endregion Properties
        

        public RootViewModel(INavigationService navigationService, IMessageBus messageBus)
        {
            // TODO: Сделать отдельную страницу для вакансии/резюме
            
        }
    }
}
