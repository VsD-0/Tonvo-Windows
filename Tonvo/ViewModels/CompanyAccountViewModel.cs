using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tonvo.ViewModels
{
    internal class CompanyAccountViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly CompanyService _companyService;
        private readonly Frame _mainFrame;
        private readonly DbTonvoContext _context;

        private string _password;
        private Company _initialCompany;
        [Reactive] public Company CurrentCompany { get; set; }

        [Reactive] public string NameCompany { get; set; } = "";
        [Reactive] public string Phone { get; set; } = "";
        [Reactive] public string Email { get; set; } = "";
        [Reactive] public string Information { get; set; } = "";
        public string Password
        {
            get => _password;
            set
            {
#if !DEBUG //TODO: Убрать "!" при демонстрации проекта в режиме DEBUG
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Пароль не может быть пустым");
                if (!value.Any(char.IsPunctuation)) throw new ArgumentException("Пароль должен содержать спецсимволы");
                if (!value.Any(char.IsDigit)) throw new ArgumentException("Пароль должен содержать цифры");
                if (!value.Any(char.IsLetter)) throw new ArgumentException("Пароль должен содержать буквы");
                if (!value.Any(char.IsUpper)) throw new ArgumentException("Пароль должен содержать прописные буквы");
                if (!value.Any(char.IsLower)) throw new ArgumentException("Пароль должен содержать строчные буквы");
                if (string.IsNullOrEmpty(value) || value.Length <= 7) throw new ArgumentException("Пароль должен быть больше 8 символов");
#endif
                this.RaiseAndSetIfChanged(ref _password, value);
            }
        }

        [Reactive] public ObservableCollection<Vacancy> Vacancies { get; set; }
        [Reactive] public Vacancy SelectedVacancy { get; set; }


        public ReactiveCommand<Unit, Unit> ExitAccount { get; }
        public ReactiveCommand<Unit, Unit> CanselEditCommand { get; }
        public ReactiveCommand<Unit, Task> SaveEditCommand { get; }

        public CompanyAccountViewModel(INavigationService navigationService, CompanyService companyService, Frame mainFrame, DbTonvoContext context)
        {
            _navigationService = navigationService;
            _companyService = companyService;
            _mainFrame = mainFrame;
            _context = context;

            //Vacancies = new(Task.Run(async () => await _context.Vacancies.Where(v => v.CompanyId == CurrentCompany.Id).ToListAsync()).Result);

            string userID = System.Configuration.ConfigurationManager.AppSettings["UserID"];

            Task.Run(async () => {
                CurrentCompany = await _companyService.GetByIdAsync(int.Parse(userID));
                _initialCompany = new Company
                {
                    NameCompany = CurrentCompany.NameCompany,
                    Email = CurrentCompany.Email,
                    PhoneNumber = CurrentCompany.PhoneNumber,
                    Information = CurrentCompany.Information,
                    Password = CurrentCompany.Password
                };
                NameCompany = CurrentCompany.NameCompany;
                Email = CurrentCompany.Email;
                Phone = CurrentCompany.PhoneNumber;
                Information = CurrentCompany.Information;
                Password = CurrentCompany.Password;
            });

            ExitAccount = ReactiveCommand.Create(() =>
            {
                Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["UserID"].Value = "";
                config.AppSettings.Settings["UserType"].Value = "";
                config.Save(ConfigurationSaveMode.Modified);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                _navigationService.NavigateToPage(_mainFrame, "ApplicantControlPanelView");
            });

            CanselEditCommand = ReactiveCommand.Create(() =>
            {
                NameCompany = _initialCompany.NameCompany;
                Email = _initialCompany.Email;
                Phone = _initialCompany.PhoneNumber;
                Information = _initialCompany.Information;
                Password = _initialCompany.Password;
            });

            SaveEditCommand = ReactiveCommand.Create(async () =>
            {
                CurrentCompany.NameCompany = NameCompany;
                CurrentCompany.Email = Email;
                CurrentCompany.PhoneNumber = Phone;
                CurrentCompany.Information = Information;
                CurrentCompany.Password = Password;

                var companies = await _companyService.GetList();
                var item = companies.First(i => i.Id == CurrentCompany.Id);
                var index = companies.IndexOf(item);

                companies.RemoveAt(index);
                companies.Insert(index, item);

                // Сохранение изменений в базе данных
                await _context.SaveChangesAsync();
            });
        }
    }
}
