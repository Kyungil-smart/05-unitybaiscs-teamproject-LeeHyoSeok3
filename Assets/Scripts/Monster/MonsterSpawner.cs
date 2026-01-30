using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private MonsterFactory _monFactory;
    private List<MonsterController> _current;

    [SerializeField] private List<Vector2Int> _monVecList;

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
        //MonsterPoolType poolType = (MonsterPoolType)Random.Range(0, 0); 

        while(!IsCanGenerate(0))
        {
            if(!IsCanGenerate(0)) { break; }
        }


        if (IsCanGenerate(0))
        {
            Vector2Int baseGrid = GetVectortoList(0);
            _current = _monFactory.Create(MonsterPoolType.Scout, baseGrid);
            foreach (var mon in _current)
            {
                mon.SetState(MonsterState.Chasing);
            }
        }
    }

    // 몬스터 타입이 없어서 임시로 정수 0사용
    private bool IsCanGenerate(int index)
    {
        switch(index)
        {
            case 0:
                if(_monVecList.Count == 0) { return false; }
                return true;
        }

        return false;
    }

    private Vector2Int GetVectortoList(int index)
    {
        switch (index)
        {
            case 0:
                return _monVecList[index];
        }

        return new Vector2Int(0, 0);
    }
}
