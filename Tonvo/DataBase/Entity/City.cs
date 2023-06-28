using System;
using System.Collections.Generic;

namespace Tonvo.DataBase.Entity;
public partial class City
{
    public int Id { get; set; }

    public string? City1 { get; set; }

    public virtual ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();
}
