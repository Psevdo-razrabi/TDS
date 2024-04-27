using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;

namespace SaveSystem
{
    [Serializable]
    public abstract class DataContext 
    {
        protected GameData Data = new();
        public abstract UniTask Load();
        public abstract UniTask Save();

        public List<T> GetDataToList<T>()
        {
            var field = GetData<T>();
            return field.GetValue(Data) as List<T>;
        }

        public void SetDataToList<T>(List<T> dataList)
        {
            var field = GetData<T>();
            field.SetValue(Data, dataList);
        }

        private FieldInfo GetData<T>()
        {
            var field = Data
                .GetType()
                .GetField(typeof(T).Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            
            if(field == null) throw new TypeAccessException($"{typeof(T)} not found in GameData");

            return field;
        }
    }
}