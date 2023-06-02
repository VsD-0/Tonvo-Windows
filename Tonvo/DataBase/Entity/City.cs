namespace Tonvo.DataBase.Entity;

public partial class City
{
    public int Id { get; set; }

    public string City1 { get; set; } = null!;

    public virtual ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();
}
