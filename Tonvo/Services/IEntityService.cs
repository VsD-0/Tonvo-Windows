using System.Collections.ObjectModel;

namespace Tonvo.Services
{
    internal interface IEntityService
    {
        public ObservableCollection<T> GetList<T>();
    }
}
