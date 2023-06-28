using System.Collections.ObjectModel;
using Tonvo.Services;

namespace Tonvo.ViewModels
{
    internal class CompanyControlPanelViewModel : ViewModelBase
    {
        private readonly ApplicantService _applicantService;

        [Reactive] public ObservableCollection<Applicant> Applicants { get; set; } = new();
        [Reactive] public Applicant SelectedApplicant { get; set; }
        public List<string> Sorts { get; set; } = new() { "По умолчанию", "По возрастанию", "По убыванию" };
        [Reactive] public string SelectedSort { get; set; }
        [Reactive] public string SelectedSalary { get; set; } = "10000";
        [Reactive] public string Search { get; set; }
        public ReactiveCommand<Unit, Unit> PrintApplicant { get; }
        public ReactiveCommand<Unit, Unit> RespondApplicant { get; }

        public CompanyControlPanelViewModel(ApplicantService applicantService)
        {
            _applicantService = applicantService;

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
            var actualApplicants = await _applicantService.GetList();
            actualApplicants = new(actualApplicants.Where(a => a.Status.Id != 3).ToList());

            if (!string.IsNullOrEmpty(Search))
                actualApplicants = new(actualApplicants.Where(a => a.DesiredProfession.Name.ToLower().Contains(Search.ToLower())).ToList());
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
