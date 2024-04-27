using System.Collections.Generic;
using System.Linq;
using Customs.Data;
using UnityEngine;

namespace SaveSystem.Repositories
{
    public sealed class MethodLabelsRepository : Repository<MethodLabelName>
    {
        public MethodLabelsRepository(DataContext dataContext) : base(dataContext)
        {
            CollectionList = GetCollectionsItems() as List<MethodLabelName>;
        }

        public override MethodLabelName GetItem(string id) => CollectionList
            .FirstOrDefault(element => element.id == id);

        public override void Delete(MethodLabelName item)
        {
            var element = FindElement(item.id);
            if (element == null)
            {
                Debug.LogWarning($"{element} not find in Enumerable");
                return;
            }
            
            CollectionList.Remove(item);
        }
        
        private MethodLabelName FindElement(string id) => CollectionList
            .FirstOrDefault(element => element.id == id);
    }
}