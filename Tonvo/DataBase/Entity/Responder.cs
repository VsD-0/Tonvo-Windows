using System;
using System.Collections.Generic;

namespace Tonvo.DataBase.Entity;
public partial class Responder
{
    public int ApplicantId { get; set; }

    public int VacancyId { get; set; }

    public DateTime RespondDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Applicant Applicant { get; set; } = null!;

    public virtual Vacancy Vacancy { get; set; } = null!;
}
