using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    private Dictionary<System.Type, object> _pools = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public ObjectPool<T> CreatePool<T>(T prefab, int count) where T : Component, IPoolable
    {
        var pool = new ObjectPool<T>(prefab, count, transform);
        _pools[typeof(T)] = pool;
        return pool;
    }

    public ObjectPool<T> GetPool<T>() where T : Component, IPoolable
    {
        return _pools[typeof(T)] as ObjectPool<T>;
    }
}