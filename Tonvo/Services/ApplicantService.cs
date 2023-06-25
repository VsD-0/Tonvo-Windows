using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading;
using Tonvo.Models;

namespace Tonvo.Services
{
    internal class ApplicantService : IEntityService
    {
        private static SemaphoreSlim _semaphoreSlim = new(1, 1);
        private readonly DbTonvoContext _context;

        public ApplicantService(DbTonvoContext context)
        {
            _context = context;
        }

        async public Task<ObservableCollection<Applicant>> GetList()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                var dbApplicants = await _context.Applicants
                    .Include(o => o.Education)
                    .Include(o => o.Status)
                    .Include(o => o.DesiredProfession)
                    .Include(o => o.City)
                    .Include(o => o.Responders)
                    .Include(o => o.Favorites)
                    .Include(o => o.Vacancies)
                    .ToListAsync();
                return new ObservableCollection<Applicant> (dbApplicants);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
    }
}
