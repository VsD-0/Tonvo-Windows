namespace Tonvo.DataBase.Entity;

public partial class Company
{
    public int Id { get; set; }

    public string NameCompany { get; set; } = null!;

    public string InitialsOfDirector { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Information { get; set; } = null!;

    public virtual ICollection<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
}
