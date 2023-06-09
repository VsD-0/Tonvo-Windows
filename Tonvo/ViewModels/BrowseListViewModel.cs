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

        [Reactive] public int SelectedList { get; set; }

        public BrowseListViewModel(IMessageBus messageBus)
        {
            _messageBus = messageBus;

            this.WhenAnyValue(x => x.SelectedList)
                .Subscribe(selectedList =>
                {
                    var message = new Messages { SelectedList = selectedList };
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
        }
    }
}
