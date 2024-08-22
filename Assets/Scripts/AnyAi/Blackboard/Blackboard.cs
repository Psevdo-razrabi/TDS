using System.Collections.Generic;
using AnyAi.Blackboard;
using Customs;

public class Blackboard
{
    private readonly Dictionary<string, BlackboardKey> _keyRegistry = new();
    private readonly Dictionary<BlackboardKey, object> _entities = new();

    public void Debug()
    {
        foreach (var entity in _entities)
        {
            var type = entity.Value.GetType();

            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(BlackboardEntry<>)) continue;
            
            var valueProperty = type.GetProperty("Value");
            if(valueProperty == null) continue;
            var value = valueProperty.GetValue(entity.Value);
            UnityEngine.Debug.Log($"Key: {entity.Key}, Value: {value}");
        }
    }

    public bool TryGetValue<T>(BlackboardKey key, out T value)
    {
        if (_entities.TryGetValue(key, out var entity) && entity is BlackboardEntry<T> castedEntry)
        {
            value = castedEntry.Value;
            return true;
        }

        value = default;
        return false;
    }

    public void SetValue<T>(BlackboardKey key, T value)
    {
        _entities[key] = value;
    }

    public BlackboardKey GetOrRegisterKey(string keyName)
    {
        Preconditions.CheckNotNull(keyName);

        if (_keyRegistry.TryGetValue(keyName, out BlackboardKey key) == false)
        {
            key = new BlackboardKey(keyName);
            _keyRegistry[keyName] = key;
        }

        return key;
    }

    public bool ContainsKey(BlackboardKey key) => _entities.ContainsKey(key);
    public bool Remove(BlackboardKey key) => _entities.Remove(key);

}
