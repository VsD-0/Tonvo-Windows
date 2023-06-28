using System.Collections.ObjectModel;

namespace Tonvo.Models
{
    internal class VacancyModel : ModelBase
    {
        public int Id { get; set; }

        public int ProfessionId { get; set; }

        public int CompanyId { get; set; }

        public string Salary { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Information { get; set; } = null!;

        public DateTime СreationDate { get; set; }

        public int? DesiredExperience { get; set; }

        public int Status { get; set; }

        public string Company { get; set; } = null!;

        public string Profession { get; set; } = null!;
        public VacancyModel()
        {

        }
    }
}
