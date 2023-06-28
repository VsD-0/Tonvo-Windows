using System;
using System.Collections.Generic;

namespace Tonvo.DataBase.Entity;
public partial class StatusApplicant
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();
}
