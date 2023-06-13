using System.Collections.ObjectModel;

namespace Tonvo.Services
{
    internal class CompanyService : IEntityService
    {
        public CompanyService()
        {
        }

        public ObservableCollection<T> GetList<T>()
        {
            throw new NotImplementedException();
        }
    }
}
