using JetBrains.Annotations;
using ModestTree;
using System;
using System.Collections.Generic;

abstract public class Pool<T>
{
    private readonly List<T> _values = new();
    private readonly int _maxObjectsInPool;

    public Pool() : this(Int32.MaxValue)
    {
    }

    public Pool(int maxObjectsInPool)
    {
        _maxObjectsInPool = maxObjectsInPool;
    }

    public abstract T NewObject();

    public T Obtain()
    {
        if (_values.IsEmpty())
        {
            return NewObject();
        }

        T res = _values[^1];
        _values.RemoveAt(_values.Count - 1);
        return res;
    }

    public bool Free(T obj)
    {
        if (_values.Count < _maxObjectsInPool)
        {
            _values.Add(obj);
            Reset(obj);
            return true;
        }
        Reset(obj);
        return false;
    }

    private void Reset(T obj)
    {
        if (obj is IPoolable p)
        {
            p.Reset();
        }
    }

    public void Prepare(int count)
    {
        for (int i = 0, l = _maxObjectsInPool > (count + _values.Count) ? count : _maxObjectsInPool - _values.Count; i < l; i++)
        {
            _values.Add(NewObject());
        }
    }

    public void FreeAll()
    {
        foreach(T obj in _values)
        {
            Reset(obj);
        }
        _values.Clear();
    }

    public int GetCountObjectsInPool()
    {
        return _values.Count;
    }
}
