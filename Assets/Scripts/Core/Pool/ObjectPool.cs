using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool<T> where T : Component, IPoolable
{
    private readonly Stack<T> _pool = new Stack<T>();
    private readonly List<T> _allObjects = new List<T>();
    private readonly T _prefab;
    private readonly Transform _parent;

    public ObjectPool(T prefab, int count, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;

        for (int i = 0; i < count; i++)
            Create();
    }

    private void Create()
    {
        var obj = Object.Instantiate(_prefab, _parent);
        obj.gameObject.SetActive(false);
        _pool.Push(obj);
        _allObjects.Add(obj);
    }

    public T Get()
    {
        if(_pool.Count == 0)
            Create();

        var obj = _pool.Pop();
        obj.gameObject.SetActive(true);
        obj.OnSpawn();
        return obj;
    }

    public void Release(T obj)
    {
        obj.OnDespawn();
        obj.gameObject.SetActive(false);
        _pool.Push(obj);
    }

    public void Clear()
    {
        foreach (var obj in _allObjects)
        {
            if(obj != null)
                    Object.Destroy(obj.gameObject);
        }
        _pool.Clear();
        _allObjects.Clear();
    }
}