using Google.Protobuf;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using Tonvo.DataBase.Entity;

namespace Tonvo.ViewModels
{
    public class BrowseListViewModel : ReactiveObject
    {
        private readonly IMessageBus _messageBus;

        [Reactive] public ObservableCollection<Applicant>  Applicants { get; set; }
        [Reactive] public ObservableCollection<Vacancy> Vacancies { get; set; } = new();
        [Reactive] public UserControl ControlPanelView { get; set; }
        [Reactive] public int SelectedList { get; set; }
        [Reactive] public Applicant SelectedApplicant { get; set; }
        [Reactive] public Vacancy SelectedVacancy { get; set; }
        public BrowseListViewModel(IMessageBus messageBus)
        {
            _messageBus = messageBus;


            this.WhenAnyValue(x => x.SelectedApplicant)
                .Subscribe(selectedApplicant =>
                {
                    var message = new Messages { SelectedApplicant = selectedApplicant };
                    _messageBus.SendMessage(message);
                });
            this.WhenAnyValue(x => x.SelectedVacancy)
                .Subscribe(selectedVacancy =>
                {
                    var message = new Messages { SelectedVacancy = selectedVacancy };
                    _messageBus.SendMessage(message);
                });
            Applicants = new ObservableCollection<Applicant>();

            Applicants.Add(new Applicant
            {
                BirthDate = DateTime.Now,
                City = new City { Id = 0, City1 = "Москва"},
                DesiredSalary = 50000,
                DesiredProfession = new Profession { Id = 0, Profession1 = "Программист"},
                Education = new LevelEducation { Id = 0, Education = "Высшее образование" },
                Email = "mail@gmail.com",
                Id=0,
                CityId=0,
                DesiredProfessionId=0,
                EducationId=0,
                Information = "Подробная информация",
                Name = "Иван",
                Password = "Password",
                Patronymic = "Иванович",
                PhoneNumber = "+7(927) 358-48-36",
                Status = new StatusApplicant { Id = 0, Status="Активно ищу работу"},
                StatusId=0,
                Surname="Ивановов"
            });
            Vacancies.Add(new Vacancy
            {
                Id = 0,
                Address = "Россия, г. Саратов, Речной пер., д. 10 кв.88",
                CompanyId = 0,
                Company = new Company
                {
                    Id = 0,
                    Email = "mailcompany@gmail.com",
                    Information = "Information about company..............",
                    InitialsOfDirector = "В.Ф.Иванов",
                    NameCompany = "NameCompany",
                    Password = "Password",
                    PhoneNumber = "+7(927) 328-98-56",
                },
                Information = "Information about vacancies..............",
                PhoneNumber = "+7(937) 368-98-76",
                Profession = new Profession { Id = 0, Profession1 = "Программист" },
                ProfessionId = 0,
                Salary = 60000,
                СreationDate = DateTime.Now
            });
        }
    }
}
