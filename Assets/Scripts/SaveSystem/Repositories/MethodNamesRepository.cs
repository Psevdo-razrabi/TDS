using System.Collections.Generic;
using System.Linq;
using Customs.Data;
using UnityEngine;

namespace SaveSystem.Repositories
{
    public sealed class MethodNamesRepository : Repository<MethodName>
    {
        public MethodNamesRepository(DataContext dataContext) : base(dataContext)
        {
            CollectionList = GetCollectionsItems() as List<MethodName>;
        }

        public override MethodName GetItem(string id) => CollectionList
            .FirstOrDefault(element => element.id == id);

        public override void Delete(MethodName item)
        {
            var element = FindElement(item.id);
            if (element == null)
            {
                Debug.LogWarning($"{element} not find in Enumerable");
                return;
            }
            
            CollectionList.Remove(item);
        }
        
        private MethodName FindElement(string id) => CollectionList
            .FirstOrDefault(element => element.id == id);
    }
}