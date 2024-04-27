using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaveSystem.Repositories
{
    public abstract class Repository<T> where T : class
    {
        public DataContext DataContext { get; protected set; }
        protected List<T> CollectionList;

        protected Repository(DataContext dataContext)
        {
            DataContext = dataContext;
        }
        
        public virtual IReadOnlyCollection<T> GetCollectionsItems() => DataContext.GetDataToList<T>();

        public virtual void SetToCollectionItems(IEnumerable<T> collection) => DataContext.SetDataToList(collection as List<T>);

        public virtual void Create(T item) => CollectionList.Add(item);

        public virtual void Update(T item)
        {
            var index = CollectionList.IndexOf(item);

            if (index == -1)
            {
                Debug.LogWarning($"{index} is not found in {nameof(CollectionList)}");
                return;
            }

            CollectionList[index] = item;
        }
        
        public abstract T GetItem(string id);
        public abstract void Delete(T item);
    }
}