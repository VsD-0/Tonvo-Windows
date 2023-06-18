using ReactiveUI;
using System.Collections.ObjectModel;
using Tonvo.Core;

namespace Tonvo.ViewModels
{
    public class CompanyControlPanelViewModel : ViewModelBase
    {
        [Reactive] public ObservableCollection<Applicant> Applicants { get; set; } = new();
        [Reactive] public Applicant SelectedApplicant { get; set; }
        public CompanyControlPanelViewModel()
        {
            Applicants.Add(new Applicant
            {
                BirthDate = DateTime.Now,
                City = new City { Id = 0, City1 = "Москва" },
                DesiredSalary = 50000,
                DesiredProfession = new Profession { Id = 0, Profession1 = "Программист" },
                Education = new LevelEducation { Id = 0, Education = "Высшее образование" },
                Email = "mail@gmail.com",
                Id = 0,
                CityId = 0,
                DesiredProfessionId = 0,
                EducationId = 0,
                Information = "Подробная информация",
                Name = "Иван",
                Password = "Password",
                Patronymic = "Иванович",
                PhoneNumber = "+7(927) 358-48-36",
                Status = new StatusApplicant { Id = 0, Status = "Активно ищу работу" },
                StatusId = 0,
                Surname = "Ивановов"
            });
            SelectedApplicant = Applicants[0];
        }
    }
}
