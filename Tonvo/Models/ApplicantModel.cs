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
        public int CityId { get; set; }

        public string City { get; set; } = null!;
        public int DesiredProfessionId { get; set; }

        public string DesiredProfession { get; set; } = null!;
        public int EducationId { get; set; }

        public string Education { get; set; } = null!;
        public ObservableCollection<VacancyModel> Favorites { get; set; } = new();

        public int StatusId { get; set; }
        public string Status { get; set; } = null!;
        public ApplicantModel()
        {

        }
    }
}
