using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tonvo.DataBase.Entity
{
    public partial class Favorite
    {
        public int ApplicantId { get; set; }

        public int VacancyId { get; set; }

        public virtual Applicant Applicant { get; set; } = null!;

        public virtual Vacancy Vacancy { get; set; } = null!;
    }
}
