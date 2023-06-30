using System.Collections.ObjectModel;
using Tonvo.DataBase.Context;
using Tonvo.DataBase.Entity;
using Tonvo.Services;

namespace Tonvo.ViewModels
{
    internal class CompanyControlPanelViewModel : ViewModelBase
    {
        private readonly ApplicantService _applicantService;
        private readonly CompanyService _companyService;
        private readonly DbTonvoContext _dbTonvoContext;

        [Reactive] public ObservableCollection<ApplicantModel> Applicants { get; set; } = new();
        [Reactive] public ApplicantModel SelectedApplicant { get; set; }
        public List<string> Sorts { get; set; } = new() { "По умолчанию", "По возрастанию", "По убыванию" };
        [Reactive] public string SelectedSort { get; set; }
        [Reactive] public string SelectedSalary { get; set; } = "10000";
        [Reactive] public string Search { get; set; }

        [Reactive] public bool IsCompany { get; set; }

        public ReactiveCommand<Unit, Unit> PrintApplicant { get; }
        public ReactiveCommand<Unit, Unit> RespondApplicant { get; }
        public ReactiveCommand<Unit,Task> BrowseRespondsCommand { get; }

        public CompanyControlPanelViewModel(DbTonvoContext dbTonvoContext, ApplicantService applicantService, CompanyService companyService)
        {
            _applicantService = applicantService;
            _companyService = companyService;
            _dbTonvoContext = dbTonvoContext;

            IsCompany = System.Configuration.ConfigurationManager.AppSettings["UserType"] == "1";

            PrintApplicant = ReactiveCommand.Create(() => { CreateDocument.Applicant(SelectedApplicant); });
            RespondApplicant = ReactiveCommand.Create(() => { _companyService.RespondAddAsync(SelectedApplicant.Id); });
            BrowseRespondsCommand = ReactiveCommand.Create(async () =>
            {
                ObservableCollection<int> responders = new(await _dbTonvoContext.Responders
                    .Where(r => r.ApplicantId == int.Parse(System.Configuration.ConfigurationManager.AppSettings["UserId"]) && r.Status == System.Configuration.ConfigurationManager.AppSettings["UserType"])
                    .Select(r => r.VacancyId)
                    .ToListAsync());
                ObservableCollection<CompanyModel> companies = new((await _companyService.GetList()).Where(c => responders.Contains(c.Id)).ToList());
                string text = "";
                foreach (CompanyModel company in companies)
                {
                    text += $"{company.NameCompany} - {company.Email}\n";
                }
                MessageBox.Show(text);
            });

            this.WhenAnyValue(
                x => x.SelectedSort,
                x => x.SelectedSalary,
                x => x.Search)
                .Subscribe(_ => ChangeList());
            _companyService = companyService;
        }

        async void ChangeList()
        {
            var actualApplicants = await _applicantService.GetList();
            actualApplicants = new(actualApplicants.Where(a => a.Status != "Не ищу работу").ToList());

            if (!string.IsNullOrEmpty(Search))
                actualApplicants = new(actualApplicants.Where(a => a.DesiredProfession.ToLower().Contains(Search.ToLower())).ToList());
            if (!string.IsNullOrEmpty(SelectedSalary))
            {
                actualApplicants = new(actualApplicants.Where(a => a.DesiredSalary >= decimal.Parse(SelectedSalary)).ToList());
            }
            if (!string.IsNullOrEmpty(SelectedSort))
            {
                switch (SelectedSort)
                {
                    case "По умолчанию":
                        break;
                    case "По возрастанию":
                        actualApplicants = new(actualApplicants.OrderBy(a => a.DesiredSalary).ToList());
                        break;
                    case "По убыванию":
                        actualApplicants = new(actualApplicants.OrderByDescending(a => a.DesiredSalary).ToList());
                        break;
                }
            }

            Applicants = actualApplicants;
            SelectedApplicant = Applicants.Count != 0 ? Applicants[0] : null;
        }
    }
}
