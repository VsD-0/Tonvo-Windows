namespace Tonvo.DataBase.Entity;

public partial class Profession
{
    public int Id { get; set; }

    public string Profession1 { get; set; } = null!;

    public virtual ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();

    public virtual ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}
