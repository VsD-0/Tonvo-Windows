using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Tonvo.DataBase.Entity;
using Tonvo.Services;


namespace Tonvo.ViewModels
{
    internal class CompanyAccountViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly CompanyService _companyService;
        private readonly VacancyService _vacancyService;
        private readonly UserService _userService;
        private readonly Frame _mainFrame;
        private readonly DbTonvoContext _context;

        private string _password;
        private Company _initialCompany;
        [Reactive] public CompanyModel CurrentCompany { get; set; }

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

        [Reactive] public string Salary { get; set; } = "";
        [Reactive] public string PhoneNumber { get; set; } = "";
        [Reactive] public string Address { get; set; } = "";
        [Reactive] public string DesiredExperience { get; set; } = "";
        [Reactive] public string InformationVacancy { get; set; } = "";


        private bool IsReg { get; set; }

        [Reactive] public ObservableCollection<VacancyModel> Vacancies { get; set; }
        [Reactive] public VacancyModel SelectedVacancy { get; set; }
        [Reactive] public VacancyModel NewVacancy { get; set; }

        [Reactive] public ObservableCollection<string> Professions { get; set; }
        [Reactive] public string SelectedProfession { get; set; }

        public ReactiveCommand<Unit, Unit> ExitAccount { get; }
        public ReactiveCommand<Unit, Unit> CanselEditCommand { get; }
        public ReactiveCommand<Unit, Task> SaveEditCommand { get; }
        public ReactiveCommand<Unit, Task> CreateVacancyCommand { get; }
        public ReactiveCommand<Unit, Task> ChangeStatus { get; }

        public CompanyAccountViewModel(INavigationService navigationService, CompanyService companyService, Frame mainFrame, DbTonvoContext context, UserService userService, VacancyService vacancyService)
        {
            _navigationService = navigationService;
            _companyService = companyService;
            _vacancyService = vacancyService;
            _mainFrame = mainFrame;
            _context = context;
            _userService = userService;

            Professions = new(Task.Run(async () => await _context.Professions.Select(p => p.Name).ToListAsync()).Result);

            string userID = System.Configuration.ConfigurationManager.AppSettings["UserID"];
            string userType = System.Configuration.ConfigurationManager.AppSettings["UserType"];

            if (userID != "" && userType == "1")
            {
                Task.Run(async () =>
                {
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
                    Vacancies = CurrentCompany.Vacancies;
                });
            }
            else IsReg = true;

            CreateVacancyCommand = ReactiveCommand.Create(async () =>
            {
                NewVacancy = new VacancyModel
                {
                    СreationDate = DateTime.Now,
                    Status = 1,
                    Salary = Salary,
                    ProfessionId = _context.Professions.SingleOrDefault(p => p.Name == SelectedProfession).Id,
                    DesiredExperience = int.Parse(DesiredExperience),
                    Information = Information,
                    Address = Address,
                    CompanyId = CurrentCompany.Id,
                    PhoneNumber = Phone,
                };
                await _vacancyService.AddVacancy(NewVacancy);
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
                if (!IsReg)
                {
                    
                    NameCompany = _initialCompany.NameCompany;
                    Email = _initialCompany.Email;
                    Phone = _initialCompany.PhoneNumber;
                    Information = _initialCompany.Information;
                    Password = _initialCompany.Password;
                }
                else _navigationService.NavigateToPage(_mainFrame, "ApplicantControlPanelView");
            });

            SaveEditCommand = ReactiveCommand.Create(async () =>
            {
                if (!IsReg)
                {
                    CurrentCompany = await _companyService.GetByIdAsync(int.Parse(System.Configuration.ConfigurationManager.AppSettings["UserID"]));
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
                }
                else
                {
                    await _userService.AddNewCompany(
                        NameCompany,
                        Phone,
                        Email,
                        Password,
                        Information);
                    IsReg = false;
                }
            });
        }
    }
}
