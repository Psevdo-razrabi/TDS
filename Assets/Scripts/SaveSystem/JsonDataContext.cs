using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace SaveSystem
{
    public class JsonDataContext : DataContext
    {
        private const string Path = "GameData";
        
        public override async UniTask Load()
        {
            if (!File.Exists(FilePath)) return;
            using var reader = new StreamReader(FilePath);
            var json = await reader.ReadToEndAsync();
            Data = JsonConvert.DeserializeObject<GameData>(json);
        }

        public override async UniTask Save()
        {
            var json = JsonConvert.SerializeObject(Data);
            await using var writer = new StreamWriter(FilePath);
            await writer.WriteAsync(json);
        }
        
       private string FilePath => $"{Application.persistentDataPath}/{Path}.json";
    }
} 