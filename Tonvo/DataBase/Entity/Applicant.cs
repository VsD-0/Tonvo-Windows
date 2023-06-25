using System;
using System.Collections.Generic;

namespace Tonvo.DataBase.Entity;

public partial class Applicant
{
    public int Id { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public int CityId { get; set; }

    public DateTime BirthDate { get; set; }

    public int DesiredProfessionId { get; set; }

    public int EducationId { get; set; }

    public decimal DesiredSalary { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Information { get; set; } = null!;

    public int StatusId { get; set; }

    public int? Experience { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual Profession DesiredProfession { get; set; } = null!;

    public virtual LevelEducation Education { get; set; } = null!;

    public virtual ICollection<Responder> Responders { get; set; } = new List<Responder>();
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual StatusApplicant Status { get; set; } = null!;

    public virtual ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}
