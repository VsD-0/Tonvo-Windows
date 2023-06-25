using System.Collections.ObjectModel;

namespace Tonvo.Models
{
    internal class ApplicantModel : ModelBase
    {
        public int Id { get; set; }

        public string Surname { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Patronymic { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public decimal DesiredSalary { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Information { get; set; } = null!;

        public int? Experience { get; set; }

        public City City { get; set; } = null!;

        public Profession DesiredProfession { get; set; } = null!;

        public LevelEducation Education { get; set; } = null!;

        public ObservableCollection<Responder> Responders { get; set; } = new ObservableCollection<Responder>();

        public StatusApplicant Status { get; set; } = null!;

        public ObservableCollection<Vacancy> Vacancies { get; set; } = new ObservableCollection<Vacancy>();
        public ApplicantModel()
        {

        }
    }
}
