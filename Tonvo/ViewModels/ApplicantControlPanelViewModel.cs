using Google.Protobuf;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;
using Tonvo.DataBase.Entity;

namespace Tonvo.ViewModels
{
    internal class ApplicantControlPanelViewModel : ViewModelBase
    {
        private readonly VacancyService _vacancyService;
        private readonly ApplicantService _applicantService;
        private readonly DbTonvoContext _dbTonvoContext;

        [Reactive] public ObservableCollection<VacancyModel> Vacancies { get; set; } = new();
        [Reactive] public VacancyModel SelectedVacancy { get; set; }
        public List<string> Sorts { get; set; } = new() { "По умолчанию", "По возрастанию", "По убыванию" };
        [Reactive] public string SelectedSort { get; set; }
        [Reactive] public string SelectedSalary { get; set; } = "10000";
        [Reactive] public string Search { get; set; }

        [Reactive] public bool IsApplicant { get; set; }

        public ReactiveCommand<Unit, Unit> PrintApplicant { get; }
        public ReactiveCommand<Unit, Unit> RespondApplicant { get; }
        public ReactiveCommand<Unit, Task> BrowseRespondsCommand { get; }


        public ApplicantControlPanelViewModel(DbTonvoContext dbTonvoContext, VacancyService vacancyService, ApplicantService applicantService)
        {
            _vacancyService = vacancyService;
            _applicantService = applicantService;
            _dbTonvoContext = dbTonvoContext;

            IsApplicant = System.Configuration.ConfigurationManager.AppSettings["UserType"] == "0";

            PrintApplicant = ReactiveCommand.Create(() => { CreateDocument.Vacancy(SelectedVacancy); });
            RespondApplicant = ReactiveCommand.Create(() => { _applicantService.RespondAddAsync(SelectedVacancy.Id); });
            BrowseRespondsCommand = ReactiveCommand.Create(async () =>
            {
                ObservableCollection<int> responders = new(await _dbTonvoContext.Responders
                    .Where(r => r.VacancyId == int.Parse(System.Configuration.ConfigurationManager.AppSettings["UserId"]) && r.Status == System.Configuration.ConfigurationManager.AppSettings["UserType"])
                    .Select(r => r.ApplicantId)
                    .ToListAsync());
                ObservableCollection<ApplicantModel> applicants = new((await _applicantService.GetList()).Where(a => responders.Contains(a.Id)).ToList());
                string text = "";
                foreach (ApplicantModel applicant in applicants)
                {
                    text += $"{applicant.DesiredProfession} - {applicant.PhoneNumber}\n";
                }
                MessageBox.Show(text);
            });
            this.WhenAnyValue(
                x => x.SelectedSort,
                x => x.SelectedSalary,
                x => x.Search)
                .Subscribe(_ => ChangeList());
        }
        async void ChangeList()
        {
            var actualVacancies = await _vacancyService.GetList();
            actualVacancies = new (actualVacancies.Where(v => v.Status == 1).ToList());

            if (!string.IsNullOrEmpty(Search))
                actualVacancies = new (actualVacancies.Where(v => v.Profession.ToLower().Contains(Search.ToLower())).ToList());
            if (!string.IsNullOrEmpty(SelectedSalary))
            {
                actualVacancies = new (actualVacancies.Where(v => int.Parse(v.Salary) >= int.Parse(SelectedSalary)).ToList());
            }
            if (!string.IsNullOrEmpty(SelectedSort))
            {
                switch (SelectedSort)
                {
                    case "По умолчанию":
                        break;
                    case "По возрастанию":
                        actualVacancies = new (actualVacancies.OrderBy(v => int.Parse(v.Salary)).ToList());
                        break;
                    case "По убыванию":
                        actualVacancies = new (actualVacancies.OrderByDescending(v => int.Parse(v.Salary)).ToList());
                        break;
                }
            }

            Vacancies = actualVacancies;
            SelectedVacancy = Vacancies.Count != 0 ? Vacancies[0] : null;
        }
    }

}
