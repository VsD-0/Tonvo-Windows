using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tonvo.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Tonvo.Services
{
    internal class UserService
    {
        private static SemaphoreSlim _semaphoreSlim = new(1, 1);
        private readonly DbTonvoContext _context;
        private readonly ApplicantService _applicantService;
        private readonly CompanyService _companyService;
        public UserService(DbTonvoContext context, ApplicantService applicantService, CompanyService companyService)
        {
            _context = context;
            _applicantService = applicantService;
            _companyService = companyService;
        }
        async private Task<ObservableCollection<User>> GetList()
        {
            ObservableCollection<User> users = new();
            var applicants = await _applicantService.GetList();
            var companies = await _companyService.GetList();

            // Добавляем объекты из списка applicants в users
            foreach (var applicant in applicants)
            {
                var user = new User
                {
                    Id = applicant.Id,
                    Email = applicant.Email,
                    Password = applicant.Password,
                    Role = 0
                };
                users.Add(user);
            }

            // Добавляем объекты из списка companies в users
            foreach (var company in companies)
            {
                var user = new User
                {
                    Id = company.Id,
                    Email = company.Email,
                    Password = company.Password,
                    Role = 1
                };
                users.Add(user);
            }

            return users;
        }
        public async Task<bool> AuthorizationAsync(string email, string password)
        {
            var users = await  GetList();
            User user = users.SingleOrDefault(u => u.Email == email);
            if (user == null)
                return false;
            if (user.Password.Equals(password))
            {
                Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["UserID"].Value = user.Id.ToString();
                config.AppSettings.Settings["UserType"].Value = user.Role.ToString();
                config.Save(ConfigurationSaveMode.Modified);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                return true;
            }
            return false;
        }

        public async Task<bool> IsEmailExists(string email)
        {
            var users = await GetList();
            return users.Any(u => u.Email == email);
        }

        public async Task AddNewApplicant(string Surname, string Name, string Patronymic, int CityId, DateTime BirthDate, int DesiredProfessionId, int EducationId, decimal DesiredSalary, string PhoneNumber, string Email, string Password, string Information, int StatusId, int Experience)
        {
            try
            {
                var applicant = new Applicant
                {
                    Surname = Surname,
                    Name = Name,
                    Patronymic = Patronymic,
                    CityId = CityId,
                    BirthDate = BirthDate,
                    DesiredProfessionId = DesiredProfessionId,
                    EducationId = EducationId,
                    DesiredSalary = DesiredSalary,
                    Email = Email,
                    Experience = Experience,
                    Information = Information,
                    Password = Password,
                    PhoneNumber = PhoneNumber,
                    StatusId = StatusId
                };
                await _context.Applicants.AddAsync(applicant);
                await _context.SaveChangesAsync();
                AuthorizationAsync(Email, Password);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}
