using System.Collections.ObjectModel;

namespace Tonvo.Services
{
    internal class CompanyService : IEntityService
    {
        private static SemaphoreSlim _semaphoreSlim = new(1, 1);
        private readonly DbTonvoContext _context;
        public CompanyService(DbTonvoContext context)
        {
            _context = context;
        }
        async public Task<ObservableCollection<Company>> GetList()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                var dbCompanies = await _context.Companies
                    .Include(o => o.Vacancies)
                    .ToListAsync();
                return new ObservableCollection<Company>(dbCompanies);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
        async public Task<Company> GetByIdAsync(int id)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                var company = await _context.Companies
                    .Include(o => o.Vacancies)
                    .FirstOrDefaultAsync(o => o.Id == id);
                return company;
;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
    }
}
