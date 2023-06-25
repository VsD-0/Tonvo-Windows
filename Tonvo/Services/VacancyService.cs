using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading;

namespace Tonvo.Services
{
    internal class VacancyService : IEntityService
    {
        private static SemaphoreSlim _semaphoreSlim = new(1, 1);
        private readonly DbTonvoContext _context;

        public VacancyService(DbTonvoContext context)
        {
            _context = context;
        }
        async public Task<ObservableCollection<Vacancy>> GetList()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                var dbVacancies = await _context.Vacancies
                    .Include(o => o.Company)
                    .Include(o => o.Profession)
                    .Include(o => o.Responders)
                    .Include(o => o.Applicants)
                    .ToListAsync();
                return new ObservableCollection<Vacancy>(dbVacancies);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
    }
}
