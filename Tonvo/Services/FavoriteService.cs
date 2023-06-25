using System.Collections.ObjectModel;

namespace Tonvo.Services
{
    internal class FavoriteService : IEntityService
    {
        private static SemaphoreSlim _semaphoreSlim = new(1, 1);
        private readonly DbTonvoContext _context;

        public FavoriteService(DbTonvoContext context)
        {
            _context = context;
        }
        async public Task<ObservableCollection<Favorite>> GetList()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                var dbFavorites = await _context.Favorites.ToListAsync();
                return new ObservableCollection<Favorite>(dbFavorites);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
    }
}
