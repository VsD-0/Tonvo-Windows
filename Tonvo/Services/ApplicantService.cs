using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading;
using Tonvo.DataBase.Entity;
using Tonvo.Models;

namespace Tonvo.Services
{
    internal class ApplicantService : IEntityService
    {
        private static SemaphoreSlim _semaphoreSlim = new(1, 1);
        private readonly DbTonvoContext _context;
        private readonly VacancyService _vacancyService;

        private List<Applicant> _dbApplicants;

        public ApplicantService(DbTonvoContext context, VacancyService vacancyService)
        {
            _context = context;
            _vacancyService = vacancyService;
        }

        async public Task<ObservableCollection<ApplicantModel>> GetList()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                _dbApplicants = await _context.Applicants.ToListAsync();
                ObservableCollection<ApplicantModel> _applicants = new();

                List<LevelEducation> educations = await _context.LevelEducations.ToListAsync();
                List<StatusApplicant> statuses = await _context.StatusApplicants.ToListAsync();
                List<Profession> professions = await _context.Professions.ToListAsync();
                List<City> cities = await _context.Cities.ToListAsync();
                List<Responder> responders = await _context.Responders.ToListAsync();
                List<Favorite> favorites = await _context.Favorites.ToListAsync();

                var vacancies = await _vacancyService.GetList();


                foreach (var item in _dbApplicants)
                {
                    _applicants.Add(new ApplicantModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Surname = item.Surname,
                        Patronymic = item.Patronymic,
                        Email = item.Email,
                        Password = item.Password,
                        CityId = item.CityId,
                        DesiredProfessionId = item.DesiredProfessionId,
                        EducationId = item.EducationId,
                        StatusId = item.StatusId,
                        Education = educations.FirstOrDefault(e => e.Id == item.EducationId).Education,
                        BirthDate = item.BirthDate,
                        City = cities.FirstOrDefault(c => c.Id == item.CityId).City1,
                        DesiredProfession = professions.FirstOrDefault(p => p.Id == item.DesiredProfessionId).Name,
                        DesiredSalary = item.DesiredSalary,
                        Experience = item.Experience,
                        Favorites = new(favorites.Where(r => r.VacancyId == item.Id)
                                                 .Select(r => vacancies.FirstOrDefault(v => v.Id == r.VacancyId))
                                                 .ToList()),
                        Information = item.Information,
                        PhoneNumber = item.PhoneNumber,
                        Status = statuses.FirstOrDefault(s => s.Id == item.StatusId).Name
                    });
                }
                return _applicants;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
        async public Task<ApplicantModel> GetByIdAsync(int id)
        {
            List<LevelEducation> educations = await _context.LevelEducations.ToListAsync();
            List<StatusApplicant> statuses = await _context.StatusApplicants.ToListAsync();
            List<Profession> professions = await _context.Professions.ToListAsync();
            List<City> cities = await _context.Cities.ToListAsync();
            List<Responder> responders = await _context.Responders.ToListAsync();
            List<Favorite> favorites = await _context.Favorites.ToListAsync();
            _dbApplicants = await _context.Applicants.ToListAsync();
            var vacancies = await _vacancyService.GetList();
            var item = _dbApplicants.SingleOrDefault(c => c.Id == id);
            var model = new ApplicantModel
            {
                Id = item.Id,
                Name = item.Name,
                Surname = item.Surname,
                Patronymic = item.Patronymic,
                Email = item.Email,
                Password = item.Password,
                CityId = item.CityId,
                DesiredProfessionId = item.DesiredProfessionId,
                EducationId = item.EducationId,
                StatusId = item.StatusId,
                Education = educations.FirstOrDefault(e => e.Id == item.EducationId).Education,
                BirthDate = item.BirthDate,
                City = cities.FirstOrDefault(c => c.Id == item.CityId).City1,
                DesiredProfession = professions.FirstOrDefault(p => p.Id == item.DesiredProfessionId).Name,
                DesiredSalary = item.DesiredSalary,
                Experience = item.Experience,
                Favorites = new(favorites.Where(r => r.VacancyId == item.Id)
                                                 .Select(r => vacancies.FirstOrDefault(v => v.Id == r.VacancyId))
                                                 .ToList()),
                Information = item.Information,
                PhoneNumber = item.PhoneNumber,
                Status = statuses.FirstOrDefault(s => s.Id == item.StatusId).Name
            };
            return model;
        }
        public async Task SaveChangesAsync()
        { 
            await _context.SaveChangesAsync(); 
        }
    }
}
