using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading;

namespace Tonvo.Models
{
    internal class CompanyModel : ModelBase
    {
        public int Id { get; set; }

        public string NameCompany { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Information { get; set; } = null!;

        public ObservableCollection<VacancyModel> Vacancies { get; set; } = new();
        public CompanyModel()
        {
        }
        
    }
}
