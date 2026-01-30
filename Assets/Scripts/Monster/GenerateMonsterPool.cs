using System;
using UnityEngine;

public class GenerateMonsterPool : MonoBehaviour
{
    [Serializable]
    public struct MonsterPoolEntry
    {
        public MonsterPoolType MonsterType;
        public MonsterView MonsterPrefab;
        public int MonsterPoolSize;
    }

    [SerializeField] private MonsterPoolEntry[] _monsterPool;

    private void Start()
    {
        foreach (var ety in _monsterPool)
        {
            PoolManager.Instance.CreatePool((int)ety.MonsterType,
                ety.MonsterPrefab, ety.MonsterPoolSize);
        }
    }
}
