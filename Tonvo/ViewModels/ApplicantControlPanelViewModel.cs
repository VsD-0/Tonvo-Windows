using Google.Protobuf;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;

namespace Tonvo.ViewModels
{
    internal class ApplicantControlPanelViewModel : ViewModelBase
    {
        private readonly VacancyService _vacancyService;

        [Reactive] public ObservableCollection<Vacancy> Vacancies { get; set; } = new();
        [Reactive] public Vacancy SelectedVacancy { get; set; }
        public List<string> Sorts { get; set; } = new() { "По умолчанию", "По возрастанию", "По убыванию" };
        [Reactive] public string SelectedSort { get; set; }
        [Reactive] public string SelectedSalary { get; set; } = "10000";
        [Reactive] public string Search { get; set; }
        public ReactiveCommand<Unit, Unit> PrintApplicant { get; }
        public ReactiveCommand<Unit, Unit> RespondApplicant { get; }
        
        public ApplicantControlPanelViewModel(VacancyService vacancyService)
        {
            _vacancyService = vacancyService;

            PrintApplicant = ReactiveCommand.Create(() => { int a = 1; });
            RespondApplicant = ReactiveCommand.Create(() => { int a = 1; });
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
                actualVacancies = new (actualVacancies.Where(v => v.Profession.Name.ToLower().Contains(Search.ToLower())).ToList());
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
