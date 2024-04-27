using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace SaveSystem
{
    public interface IRepository<T> where T : class 
    {
        DataContext DataContext { get; }
        IEnumerable<T> GetCollectionsItems();
        void SetToCollectionItems(IEnumerable<T> collection);
        T GetItem(string id);
        void Create(T item);
        void Delete(T item);
        void Update(T item);
        UniTask Save();
    }
}