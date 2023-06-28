using System.Collections.ObjectModel;
using Tonvo.Models;

namespace Tonvo.Services
{
    internal class CompanyService : IEntityService
    {
        private static SemaphoreSlim _semaphoreSlim = new(1, 1);
        private readonly DbTonvoContext _context;
        private readonly VacancyService _vacancyService;

        private List<Company> _dbCompanies;
        public CompanyService(DbTonvoContext context, VacancyService vacancyService)
        {
            _context = context;
            _vacancyService = vacancyService;
        }
        async public Task<ObservableCollection<CompanyModel>> GetList()
        {
            _dbCompanies = await _context.Companies.ToListAsync();
            ObservableCollection<CompanyModel> _companies = new();

            List<Vacancy> vacancies = await _context.Vacancies.ToListAsync();

            foreach (var item in _dbCompanies)
            {
                _companies.Add(new CompanyModel
                {
                    Id = item.Id,
                    NameCompany = item.NameCompany,
                    PhoneNumber = item.PhoneNumber,
                    Email = item.Email,
                    Password = item.Password,
                    Information = item.Information,
                    Vacancies = new((await _vacancyService.GetList())
                                                        .Where(v => v.CompanyId == item.Id)
                                                        .ToList())
                });
            }
            return _companies;
        }
        async public Task<CompanyModel> GetByIdAsync(int id)
        {
            _dbCompanies = await _context.Companies.ToListAsync();
            var vacancies = await _vacancyService.GetList();
            var item = _dbCompanies.SingleOrDefault(c => c.Id == id);
            var model = new CompanyModel
            {
                Id = id,
                NameCompany = item.NameCompany,
                PhoneNumber = item.PhoneNumber,
                Email = item.Email,
                Password = item.Password,
                Information = item.Information,
                Vacancies = new(vacancies.Where(v => v.CompanyId == item.Id)
                                         .ToList())
            };
            return model;
        }
    }
}
