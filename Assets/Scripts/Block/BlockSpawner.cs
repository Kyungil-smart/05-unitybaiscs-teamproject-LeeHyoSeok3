using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private float blockSize = 1f;
    [SerializeField] private float fallSpeed = 2f;
    [SerializeField] private CangenerateBolockList _cangeneratelist;

    private BlockFactory _factory;
    private BlockGroup _current;

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
        
        _factory = new BlockFactory(blockSize);
    }

    // 테스트 코드
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnRandom();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_current == null) return;

        Debug.Log("OnTriggerEnter");
        
    }

    public void SpawnRandom()
    {
        BlockType type = (BlockType)Random.Range(0, 7);
        BlockPoolType poolType = (BlockPoolType)Random.Range(0, 7);

        // type 추출 코드 테스트
        while (!IsCanGenerate(type))
        {
            type = (BlockType)Random.Range(0, 7);
            if (
                !IsCanGenerate(BlockType.I) &&
                !IsCanGenerate(BlockType.O) &&
                !IsCanGenerate(BlockType.T) &&
                !IsCanGenerate(BlockType.S) &&
                !IsCanGenerate(BlockType.Z) &&
                !IsCanGenerate(BlockType.J) &&
                !IsCanGenerate(BlockType.L))
            {
                Debug.Log("생성 불가");
                break;
            }
        }

        //Grid에서 생성가능한 좌표 가져오기
        if (IsCanGenerate(type)) // 해당 타입이 생성 불가능하면 동작 안함
        {
            Vector2Int baseGrid = GetVectortoList(type);
            Debug.Log($"x: {baseGrid.x}, y: {baseGrid.y}");
            _current = _factory.Create(type, poolType, baseGrid);
        }
    }

    public void SpawnObstacle(int howmany)
    {
        BlockType type = BlockType.B;
        BlockPoolType poolType = BlockPoolType.Rock;


        if(_cangeneratelist.ObList.Count < howmany) // 빈 자리보다 생성 블럭이 많을 때 처리
        {
            howmany = _cangeneratelist.ObList.Count;
        }

        for(int i = 0; i < howmany; i++) // 요청한 수 만큼 반복
        {
            // 좌표 탐색
            
            // _cangeneratelist.ObList 에서 좌표 받아오기
            int index = Random.Range(0,_cangeneratelist.ObList.Count);
            // 생성
            _factory.Create(type, poolType, new Vector2Int((int)_cangeneratelist.ObList[index].transform.position.x, (int)_cangeneratelist.ObList[index].transform.position.z));
            _cangeneratelist.ObList.RemoveAt(index);
        }
    }
    

    private bool IsCanGenerate(BlockType shape)
    {
        switch (shape)
        {
            case BlockType.I: // I
                if (_cangeneratelist.IUpList.Count == 0) return false;
                return true;
            case BlockType.O: // O
                if (_cangeneratelist.OUpList.Count == 0) return false;
                return true;

            case BlockType.T: // T
                if (_cangeneratelist.TUpList.Count == 0) return false;
                return true;

            case BlockType.S: // S
                if (_cangeneratelist.SUpList.Count == 0) return false;
                return true;

            case BlockType.Z: // Z
                if (_cangeneratelist.ZUpList.Count == 0) return false;
                return true;

            case BlockType.J: // J
                if (_cangeneratelist.JUpList.Count == 0) return false;
                return true;

            case BlockType.L: //L
                if (_cangeneratelist.LUpList.Count == 0) return false;
                return true;
        }

        return false;
    }

    private Vector2Int GetVectortoList(BlockType shape)
    {
        switch (shape)
        {
            case BlockType.I: // I
                return _cangeneratelist.IUpList[Random.Range(0, _cangeneratelist.IUpList.Count)];

            case BlockType.O: // O
                return _cangeneratelist.OUpList[Random.Range(0, _cangeneratelist.OUpList.Count)];

            case BlockType.T: // T
                return _cangeneratelist.TUpList[Random.Range(0, _cangeneratelist.TUpList.Count)];

            case BlockType.S: // S
                return _cangeneratelist.SUpList[Random.Range(0, _cangeneratelist.SUpList.Count)];

            case BlockType.Z: // Z
                return _cangeneratelist.ZUpList[Random.Range(0, _cangeneratelist.ZUpList.Count)];

            case BlockType.J: // J
                return _cangeneratelist.JUpList[Random.Range(0, _cangeneratelist.JUpList.Count)];

            case BlockType.L: // L
                return _cangeneratelist.LUpList[Random.Range(0, _cangeneratelist.LUpList.Count)];
        }

        return new Vector2Int(0, 0);
    }
}