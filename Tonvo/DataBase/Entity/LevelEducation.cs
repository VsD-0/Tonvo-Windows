using System;
using System.Collections.Generic;

namespace Tonvo.DataBase.Entity;
public partial class LevelEducation
{
    public int Id { get; set; }

    public string Education { get; set; } = null!;

    public virtual ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();
}
