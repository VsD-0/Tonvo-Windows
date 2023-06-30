using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading;
using Tonvo.DataBase.Entity;
using Tonvo.Models;

namespace Tonvo.Services
{
    internal class VacancyService : IEntityService
    {
        private static SemaphoreSlim _semaphoreSlim = new(1, 1);
        private readonly DbTonvoContext _context;

        private List<Vacancy> _dbVacancies;

        public VacancyService(DbTonvoContext context)
        {
            _context = context;
        }
        async public Task<ObservableCollection<VacancyModel>> GetList()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                _dbVacancies = await _context.Vacancies.ToListAsync();
                ObservableCollection<VacancyModel> _vacancies = new();
                List<Profession> professions = await _context.Professions.ToListAsync();
                List<Company> companies = await _context.Companies.ToListAsync();
                List<Responder> responders = await _context.Responders.ToListAsync();

                foreach (var item in _dbVacancies)
                {
                    _vacancies.Add(new VacancyModel
                    {
                        Id = item.Id,
                        Address = item.Address,
                        Company = companies.FirstOrDefault(c => c.Id == item.CompanyId).NameCompany,
                        CompanyId = item.CompanyId,
                        DesiredExperience = item.DesiredExperience,
                        Information = item.Information,
                        PhoneNumber = item.PhoneNumber,
                        Profession = professions.FirstOrDefault(p => p.Id == item.ProfessionId).Name,
                        ProfessionId = item.ProfessionId,
                        Salary = item.Salary,
                        Status = item.Status,
                        СreationDate = item.СreationDate
                    });
                }
                return _vacancies;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
        async public Task AddVacancy(VacancyModel vacancyModel)
        {
            DateTime dt = DateTime.Now;
            Vacancy vacancy = new Vacancy
            {
                Address = vacancyModel.Address,
                CompanyId = vacancyModel.CompanyId,
                DesiredExperience = vacancyModel.DesiredExperience,
                Information = vacancyModel.Information,
                PhoneNumber = vacancyModel.PhoneNumber,
                ProfessionId = vacancyModel.ProfessionId,
                Salary = vacancyModel.Salary,
                Status = 1,
                СreationDate = dt
            };
            await _context.Vacancies.AddAsync(vacancy);
            await _context.SaveChangesAsync();
        }
    }
}
