using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory
{
    // MonsterObj甫 包府且 ObjectPool积己
    private readonly ObjectPool<MonsterView>[] _monsterPools;

    public MonsterFactory()
    {
        int count = System.Enum.GetValues(typeof(MonsterPoolType)).Length;
        _monsterPools = new ObjectPool<MonsterView>[count];

        for (int i = 0; i < count; i++)
        {
            _monsterPools[i] = PoolManager.Instance.GetPool<MonsterView>(i);
        }
    }

    public List<MonsterController> Create(MonsterPoolType poolType, Vector2Int baseGrid)
    {
        List<MonsterController> monsters = new(1);
        
        Vector2Int grid = baseGrid;
        MonsterView view = _monsterPools[(int)poolType].Get();

        MonsterController monCtr = new MonsterController(view, grid, poolType);
        view.Initialize(monCtr);

        monsters.Add(monCtr);

        return monsters;
    }

    public void Realease(List<MonsterController> monsters)
    {
        foreach(var mon in monsters) { mon.Release(); }
    }
}
