using Google.Protobuf;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.Windows.Controls;

namespace Tonvo.ViewModels
{
    internal class ApplicantAccountViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ApplicantService _applicantService;
        private readonly UserService _userService;
        private readonly Frame _mainFrame;
        private readonly DbTonvoContext _context;

        private string _password;
        private ApplicantModel _initialApplicant;
        [Reactive] public ApplicantModel CurrentApplicant { get; set; }
        [Reactive] public string SelectedProfession { get; set; }
        [Reactive] public string SelectedCity { get; set; }
        [Reactive] public string SelectedEducation { get; set; }
        [Reactive] public string SelectedStatus { get; set; }

        [Reactive] public string Name { get; set; } = "";
        [Reactive] public string Surname { get; set; } = "";
        [Reactive] public string Patronymic { get; set; } = "";
        [Reactive] public string BirthDate { get; set; } = "";
        [Reactive] public string DesiredSalary { get; set; } = "";
        [Reactive] public string Experience { get; set; } = "";
        [Reactive] public string Phone { get; set; } = "";
        [Reactive] public string Email { get; set; } = "";
        [Reactive] public string Information { get; set; } = "";

        private bool IsReg { get; set; }

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

        [Reactive] public ObservableCollection<string> Professions { get; set; }
        [Reactive] public ObservableCollection<string> Cities { get; set; }
        [Reactive] public ObservableCollection<string> Educations { get; set; }
        [Reactive] public ObservableCollection<string> Statuses { get; set; }

        public ReactiveCommand<Unit, Unit> ExitAccount { get; }
        public ReactiveCommand<Unit, Unit> CanselEditCommand { get; }
        public ReactiveCommand<Unit, Task> SaveEditCommand { get; }
        public ApplicantAccountViewModel(INavigationService navigationService, ApplicantService applicantService, Frame mainFrame, DbTonvoContext context, UserService userService)
        {
            _navigationService = navigationService;
            _applicantService = applicantService;
            _userService = userService;
            _mainFrame = mainFrame;
            _context = context;

            Professions = new(Task.Run(async () => await _context.Professions.Select(p => p.Name).ToListAsync()).Result);
            Cities = new(Task.Run(async () => await _context.Cities.Select(p => p.City1).ToListAsync()).Result);
            Educations = new(Task.Run(async () => await _context.LevelEducations.Select(p => p.Education).ToListAsync()).Result);
            Statuses = new(Task.Run(async () => await _context.StatusApplicants.Select(p => p.Name).ToListAsync()).Result);

            string userID = System.Configuration.ConfigurationManager.AppSettings["UserID"];
            if (userID != "")
            {
                Task.Run(async () =>
                {
                    CurrentApplicant = await _applicantService.GetByIdAsync(int.Parse(userID));
                    _initialApplicant = new ApplicantModel
                    {
                        Name = CurrentApplicant.Name,
                        Surname = CurrentApplicant.Surname,
                        Patronymic = CurrentApplicant.Patronymic,
                        Email = CurrentApplicant.Email,
                        BirthDate = CurrentApplicant.BirthDate,
                        DesiredProfession = CurrentApplicant.DesiredProfession,
                        DesiredSalary = CurrentApplicant.DesiredSalary,
                        Experience = CurrentApplicant.Experience,
                        PhoneNumber = CurrentApplicant.PhoneNumber,
                        Information = CurrentApplicant.Information,
                        Password = CurrentApplicant.Password,
                        Status = CurrentApplicant.Status,
                        Education = CurrentApplicant.Education,
                        City = CurrentApplicant.City
                    };
                    Name = CurrentApplicant.Name;
                    Surname = CurrentApplicant.Surname;
                    Patronymic = CurrentApplicant.Patronymic;
                    Email = CurrentApplicant.Email;
                    BirthDate = CurrentApplicant.BirthDate.ToString();
                    DesiredSalary = ((int)CurrentApplicant.DesiredSalary).ToString();
                    Experience = CurrentApplicant.Experience.ToString();
                    Phone = CurrentApplicant.PhoneNumber;
                    Information = CurrentApplicant.Information;
                    Password = CurrentApplicant.Password;
                    SelectedStatus = CurrentApplicant.Status;
                    SelectedEducation = CurrentApplicant.Education;
                    SelectedCity = CurrentApplicant.City;
                    SelectedProfession = CurrentApplicant.DesiredProfession;
                });
            }
            else IsReg = true;

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
                    Name = _initialApplicant.Name;
                    Surname = _initialApplicant.Surname;
                    Patronymic = _initialApplicant.Patronymic;
                    Email = _initialApplicant.Email;
                    BirthDate = _initialApplicant.BirthDate.ToString();
                    DesiredSalary = ((int)_initialApplicant.DesiredSalary).ToString();
                    Experience = _initialApplicant.Experience.ToString();
                    Phone = _initialApplicant.PhoneNumber;
                    Information = _initialApplicant.Information;
                    Password = _initialApplicant.Password;
                    SelectedStatus = _initialApplicant.Status;
                    SelectedEducation = _initialApplicant.Education;
                    SelectedCity = _initialApplicant.City;
                    SelectedProfession = _initialApplicant.DesiredProfession;
                } 
                else _navigationService.NavigateToPage(_mainFrame, "ApplicantControlPanelView");
            });

            SaveEditCommand = ReactiveCommand.Create(async () =>
            {
                if (!IsReg)
                {
                    CurrentApplicant.Name = Name;
                    CurrentApplicant.Surname = Surname;
                    CurrentApplicant.Patronymic = Patronymic;
                    CurrentApplicant.Email = Email;
                    CurrentApplicant.BirthDate = DateTime.Parse(BirthDate);
                    CurrentApplicant.DesiredSalary = decimal.Parse(DesiredSalary);
                    CurrentApplicant.Experience = int.Parse(Experience);
                    CurrentApplicant.PhoneNumber = Phone;
                    CurrentApplicant.Information = Information;
                    CurrentApplicant.Password = Password;
                    CurrentApplicant.StatusId = (await _context.StatusApplicants.FirstOrDefaultAsync(s => s.Name == SelectedStatus)).Id;
                    CurrentApplicant.EducationId = (await _context.LevelEducations.FirstOrDefaultAsync(e => e.Education == SelectedEducation)).Id;
                    CurrentApplicant.CityId = (await _context.Cities.FirstOrDefaultAsync(c => c.City1 == SelectedCity)).Id;
                    CurrentApplicant.DesiredProfessionId = (await _context.Professions.FirstOrDefaultAsync(p => p.Name == SelectedProfession)).Id;

                    var applicants = await _applicantService.GetList();
                    var item = applicants.First(i => i.Id == CurrentApplicant.Id);
                    var index = applicants.IndexOf(item);

                    applicants.RemoveAt(index);
                    applicants.Insert(index, item);

                    // Сохранение изменений в базе данных
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var city = _context.Cities.SingleOrDefault(c => c.City1 == SelectedCity).Id;
                    var birthdate = DateTime.ParseExact(BirthDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    var profession = _context.Professions.SingleOrDefault(p => p.Name == SelectedProfession).Id;
                    var education = _context.LevelEducations.SingleOrDefault(e => e.Education == SelectedEducation).Id;
                    var status = _context.StatusApplicants.SingleOrDefault(s => s.Name == SelectedStatus).Id;
                    await _userService.AddNewApplicant(
                        Surname,
                        Name,
                        Patronymic,
                        city,
                        birthdate,
                        profession,
                        education,
                        decimal.Parse(DesiredSalary),
                        Phone,
                        Email,
                        Password,
                        Information,
                        status,
                        int.Parse(Experience)
                        );

                }
            });
        }
    }
}
