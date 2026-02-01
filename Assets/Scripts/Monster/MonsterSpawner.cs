using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private CangenerateBolockList _cangeneratelist;
    private MonsterFactory _monFactory;
    private List<MonsterController> _current;

    [SerializeField] private List<Vector2Int> _monVecList;

    [Serializable]
    public struct MonsterPoolEntry
    {
        public MonsterPoolType poolType;
        public MonsterView prefab;
        public int poolsize;
    }

    [SerializeField] private MonsterPoolEntry[] _pools;

    private void Start()
    {
        foreach(var ety in _pools)
        {
            PoolManager.Instance.CreatePool((int)ety.poolType, ety.prefab, ety.poolsize);
        }

        _monFactory = new MonsterFactory();
    }

    private void SpawnRandom()
    {
        //MonsterPoolType poolType = (MonsterPoolType)Random.Range(0, 0); 

            int index = Random.Range(0, _cangeneratelist.ObList.Count);
            _monFactory.Create(MonsterPoolType.Scout, new Vector2Int((int)_cangeneratelist.ObList[index].transform.position.x, (int)_cangeneratelist.ObList[index].transform.position.z));
            _cangeneratelist.ObList.RemoveAt(index);
    }

    public void SpawnMonster(int howmany)
    {
        if (_cangeneratelist.ObList.Count < howmany) // �� �ڸ����� ���� ������ ���� �� ó��
        {
            howmany = _cangeneratelist.ObList.Count;
        }

        for (int i = 0; i < howmany; i++) // ��û�� �� ��ŭ �ݺ�
        {
            // ��ǥ Ž��

            // _cangeneratelist.ObList ���� ��ǥ �޾ƿ���
            int index = Random.Range(0, _cangeneratelist.ObList.Count);
            // ����
            _monFactory.Create((MonsterPoolType)Random.Range(0, 2), new Vector2Int((int)_cangeneratelist.ObList[index].transform.position.x, (int)_cangeneratelist.ObList[index].transform.position.z));
            _cangeneratelist.ObList.RemoveAt(index);
        }
    }

    // ���� Ÿ���� ��� �ӽ÷� ���� 0���
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

}
