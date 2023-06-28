using System;
using System.Collections.Generic;

namespace Tonvo.DataBase.Entity;
public partial class Favorite
{
    public int ApplicantId { get; set; }

    public int VacancyId { get; set; }

    public DateTime? Date { get; set; }

    public virtual Applicant Applicant { get; set; } = null!;

    public virtual Vacancy Vacancy { get; set; } = null!;
}
