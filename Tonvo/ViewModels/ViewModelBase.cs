using System.Collections.ObjectModel;

namespace Tonvo.ViewModels
{
    public class ViewModelBase : ReactiveObject 
    {
        public static ViewModelBase Instance { get; } = new ViewModelBase();

        public ObservableCollection<Vacancy> Vacancies { get; set; }
        public ObservableCollection<Applicant> Applicants { get; set; }

        // 0 = Applicant, 1 = Company, 2 = Vacancy
        public static int modeAccount;

        static Applicant _selectedApplicant = new();
        public static Applicant SelectedApplicant
        {
            get => _selectedApplicant; set
            {
                _selectedApplicant = value;
                onSelectedApplicantUpdate.ForEach((item) => item.DynamicInvoke());
            }
        }

        public static List<Delegate> onSelectedApplicantUpdate = new();

        private static Vacancy _selectedVacancy = new();
        public static Vacancy SelectedVacancy
        {
            get => _selectedVacancy; set
            {
                _selectedVacancy = value;
                onSelectedVacancyUpdate.ForEach((item) => item.DynamicInvoke());
            }
        }
        public static List<Delegate> onSelectedVacancyUpdate = new();

        public Applicant ApplicantNewAccount { get; set; }
        public Vacancy VacancyNewAccount { get; set; }

        static object _currentView = new();
        public static object CurrentView
        {
            get => _currentView; set
            {
                _currentView = value;
                onViewUpdate.ForEach((item) => item.DynamicInvoke());
            }
        }

        public static List<Delegate> onViewUpdate = new();

        public static Applicant UserApplicant { get; set; }
        public static Vacancy UserVacancy { get; set; }


        static object _currentControlView = new();
        public static object CurrentControlView
        {
            get => _currentControlView; set
            {
                _currentControlView = value;
                onControlViewUpdate.ForEach((item) => item.DynamicInvoke());
            }
        }

        public static List<Delegate> onControlViewUpdate = new();
    }
}
