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
                var dbApplicants = await _context.Applicants.ToListAsync();
                return new ObservableCollection<Applicant> (dbApplicants);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
        async public Task<Applicant> GetByIdAsync(int id)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                var applicant = await _context.Applicants.FirstOrDefaultAsync(o => o.Id == id);
                return applicant;
                //var dbApplicants = await _context.Applicants.ToListAsync();
                //ObservableCollection<Applicant> applicants = new();

                //List<Productname> pnames = await _context.Productnames.ToListAsync();
                //List<Productmanufacturer> pmanufactures = await _context.Productmanufacturers.ToListAsync();
                //List<Productunit> productunits = await _context.Productunits.ToListAsync();

                //foreach (var item in _product)
                //{
                //    products.Add(new ProductModel
                //    {
                //        Article = item.ParticleNumber,
                //        Image = item.Pphoto == string.Empty ? "picture.png" : item.Pphoto,
                //        Title = pnames.SingleOrDefault(pn => pn.NameId == item.PnameId).Name,
                //        Description = item.Pdescription,
                //        Manufacturer = pmanufactures.SingleOrDefault(pm => pm.ManufacturerId == item.PmanufacturerId).Manufacturer,
                //        Price = item.Pcost,
                //        Discount = (int)item.PdiscountAmount,
                //        Unit = productunits.SingleOrDefault(pn => pn.UnitId == item.PunitId).Unit,
                //        InStock = item.PquantityInStock,
                //        Status = item.Pstatus,
                //    });
                //}
                //return products;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
        public async Task SaveChangesAsync()
        { 
            await _context.SaveChangesAsync(); 
        }
    }
}
