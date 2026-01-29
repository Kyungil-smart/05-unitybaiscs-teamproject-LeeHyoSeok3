using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private MonsterFactory _monFactory;
    private List<MonsterController> _current;

    private void Start()
    {
        _monFactory = new MonsterFactory();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpawnRandom();
        }
    }

    private void SpawnRandom()
    {
       // if(생성가능한 위치인지)
        //Vector2Int baseGrid = new Vector2Int(Random.Range(0, 10), Random.Range(0, 10));
        //MonsterPoolType poolType = (MonsterPoolType)Random.Range(0, 0); 
        _current = _monFactory.Create(MonsterPoolType.Scout, new Vector2Int(0,0));
       foreach(var mon in _current)
        {
            mon.SetState(MonsterState.Chasing);
        }
    }
}
