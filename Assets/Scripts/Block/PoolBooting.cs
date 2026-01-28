using System;
using UnityEngine;

public class PoolBooting : MonoBehaviour
{
    // [SerializeField] private BlockView _block;
    // [SerializeField] private int poolSize = 200;
    
    [Serializable]
    public struct BlockPoolEntry
    {
        public BlockPoolType poolType;
        public BlockView prefab;
        public int poolsize;
    }
    
    [SerializeField] private BlockPoolEntry[] _pool;
    
    
    private void Start()
    {
        foreach (var ety in _pool) {
            PoolManager.Instance.CreatePool((int)ety.poolType, ety.prefab, ety.poolsize);
        }
    }
}