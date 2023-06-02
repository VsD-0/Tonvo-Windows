namespace Tonvo.DataBase.Entity;

public partial class Vacancy
{
    public int Id { get; set; }

    public int ProfessionId { get; set; }

    public int CompanyId { get; set; }

    public string Salary { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Information { get; set; } = null!;

    public DateTime СreationDate { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Profession Profession { get; set; } = null!;

    public virtual ICollection<Responder> Responders { get; set; } = new List<Responder>();

    public virtual ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();
}
