using System.Configuration;

namespace Tonvo.ViewModels
{
    internal class ApplicantAccountViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ApplicantService _applicantService;
        private readonly Frame _mainFrame;
        public string userID { get; set; }
        [Reactive] public Applicant CurrentApplicant { get; set; }

        public ReactiveCommand<Unit, Unit> ExitAccount { get; }
        public ApplicantAccountViewModel(INavigationService navigationService, ApplicantService applicantService, Frame mainFrame)
        {
            _navigationService = navigationService;
            _applicantService = applicantService;
            _mainFrame = mainFrame;

            userID = System.Configuration.ConfigurationManager.AppSettings["UserID"];

            Task.Run(async () => { CurrentApplicant = await _applicantService.GetByIdAsync(int.Parse(userID)); });

            ExitAccount = ReactiveCommand.Create(() =>
            {
                Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["UserID"].Value = "";
                config.AppSettings.Settings["UserType"].Value = "";
                config.Save(ConfigurationSaveMode.Modified);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                _navigationService.NavigateToPage(_mainFrame, "ApplicantControlPanelView");
            });
        }
    }
}
