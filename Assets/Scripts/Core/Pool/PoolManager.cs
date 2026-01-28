using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    private Dictionary<(System.Type, int), object> _pools = new();

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

    public ObjectPool<T> CreatePool<T>(int poolId, T prefab, int count) where T : Component, IPoolable
    {
        var key = (typeof(T), poolId);
        var pool = new ObjectPool<T>(prefab, count, transform);
        _pools[key] = pool;
        return pool;
    }

    public ObjectPool<T> GetPool<T>(int poolId) where T : Component, IPoolable
    {
        var key = (typeof(T), poolId);
        return _pools[key] as ObjectPool<T>;
    }
}