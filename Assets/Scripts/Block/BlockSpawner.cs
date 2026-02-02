using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private float blockSize = 1f;
    [SerializeField] private float dropY;
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
    
    [Serializable]
    public struct GhostPoolEntry
    {
        public BlockPoolType poolType;
        public GhostBlock prefab;
        public int poolsize;
    }

    [SerializeField] private BlockPoolEntry[] _blockPools;
    [SerializeField]private GhostPoolEntry[] _ghostPools;
    
    private void Start()
    {
        foreach (var e in _blockPools)
            PoolManager.Instance.CreatePool(
                (int)e.poolType,
                e.prefab,
                e.poolsize
            );

        foreach (var e in _ghostPools)
            PoolManager.Instance.CreatePool(
                (int)e.poolType,
                e.prefab,
                e.poolsize
            );

        _factory = new BlockFactory(blockSize);
    }

    // 테스트 코드
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SpawnRandom();
        }
    }

    public void SpawnRandom()
    {
        if (!CanAny()) { return; } // 생성 가능 블럭 없으면 중단
        
        BlockType type = (BlockType)Random.Range(0, 7);
        BlockPoolType poolType = (BlockPoolType)Random.Range(0, 7);
        int rotation = Random.Range(0, 4);

        while (TargetList(type, rotation).Count == 0)
        {
            type = (BlockType)Random.Range(0, 7);
            rotation = Random.Range(0, 4);
        }  // = Worst Code Ever

        //Grid에서 생성가능한 좌표 가져오기
        GridTile pivotgrid = TargetList(type, rotation)[Random.Range(0, TargetList(type, rotation).Count)];
        Vector2Int baseGrid = GetVecToGrid(pivotgrid);
        _current = _factory.Create(type, poolType, baseGrid, dropY, rotation);
        
        // 떨어질 자리 표시
        pivotgrid.Predict(type, rotation);
        pivotgrid.GetComponent<CanJBlock>().CheckNear();
        pivotgrid.GetComponent<CanOBlock>().CheckNear();
        pivotgrid.GetComponent<CanSBlock>().CheckNear();
        pivotgrid.GetComponent<CanZBlock>().CheckNear();
        pivotgrid.GetComponent<CanLBlock>().CheckNear();
        pivotgrid.GetComponent<CanIBlock>().CheckNear();
        pivotgrid.GetComponent<CanTBlock>().CheckNear();
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
            _factory.Create(
                type,
                poolType,
                new Vector2Int((int)_cangeneratelist.ObList[index].transform.position.x,
                    (int)_cangeneratelist.ObList[index].transform.position.z),
                dropY,
                0);
            _cangeneratelist.ObList[index]._predict = true;
            _cangeneratelist.ObList[index].GetComponent<CanLBlock>().CheckNear();
            _cangeneratelist.ObList[index].GetComponent<CanJBlock>().CheckNear();
            _cangeneratelist.ObList[index].GetComponent<CanOBlock>().CheckNear();
            _cangeneratelist.ObList[index].GetComponent<CanTBlock>().CheckNear();
            _cangeneratelist.ObList[index].GetComponent<CanIBlock>().CheckNear();
            _cangeneratelist.ObList[index].GetComponent<CanSBlock>().CheckNear();
            _cangeneratelist.ObList[index].GetComponent<CanZBlock>().CheckNear();
            _cangeneratelist.ObList.RemoveAt(index);
        }
    }
    

    private List<GridTile> TargetList(BlockType shape, int rotation)
    {
        switch (shape)
        {
            case BlockType.I:
                switch (rotation)
                {
                    case 0: return _cangeneratelist.IUpList;
                    case 1: return _cangeneratelist.ILeftList;
                    case 2: return _cangeneratelist.IDownList;
                    case 3: return _cangeneratelist.IRightList;
                }
                break;
            
            case BlockType.T:
                switch (rotation)
                {
                    case 0: return _cangeneratelist.TUpList;
                    case 1: return _cangeneratelist.TLeftList;
                    case 2: return _cangeneratelist.TDownList;
                    case 3: return _cangeneratelist.TRightList;
                }
                break;
            case BlockType.O:
                switch (rotation)
                {
                    case 0: return _cangeneratelist.OUpList;
                    case 1: return _cangeneratelist.OLeftList;
                    case 2: return _cangeneratelist.ODownList;
                    case 3: return _cangeneratelist.ORightList;
                }
                break;
            case BlockType.S:
                switch (rotation)
                {
                    case 0: return _cangeneratelist.SUpList;
                    case 1: return _cangeneratelist.SLeftList;
                    case 2: return _cangeneratelist.SDownList;
                    case 3: return _cangeneratelist.SRightList;
                }
                break;
            case BlockType.Z:
                switch (rotation)
                {
                    case 0: return _cangeneratelist.ZUpList;
                    case 1: return _cangeneratelist.ZLeftList;
                    case 2: return _cangeneratelist.ZDownList;
                    case 3: return _cangeneratelist.ZRightList;
                }
                break;
            case BlockType.J:
                switch (rotation)
                {
                    case 0: return _cangeneratelist.JUpList;
                    case 1: return _cangeneratelist.JLeftList;
                    case 2: return _cangeneratelist.JDownList;
                    case 3: return _cangeneratelist.JRightList;
                }
                break;
            case BlockType.L:
                switch (rotation)
                {
                    case 0: return _cangeneratelist.LUpList;
                    case 1: return _cangeneratelist.LLeftList;
                    case 2: return _cangeneratelist.LDownList;
                    case 3: return _cangeneratelist.LRightList;
                }
                break;   
        }
        return null;
    }

    private Vector2Int GetVecToGrid(GridTile grid)
    {
        return new Vector2Int(
            (int)grid.transform.position.x,
            (int)grid.transform.position.z);
    }

    private bool CanAny()
    {
        if(_cangeneratelist.IUpList.Count == 0 &&
           _cangeneratelist.IDownList.Count == 0 &&
           _cangeneratelist.ILeftList.Count == 0 &&
           _cangeneratelist.IRightList.Count == 0 &&
           _cangeneratelist.OUpList.Count == 0 &&
           _cangeneratelist.ODownList.Count == 0 &&
           _cangeneratelist.OLeftList.Count == 0 &&
           _cangeneratelist.ORightList.Count == 0 &&
           _cangeneratelist.TUpList.Count == 0 &&
           _cangeneratelist.TDownList.Count == 0 &&
           _cangeneratelist.TLeftList.Count == 0 &&
           _cangeneratelist.TRightList.Count == 0 &&
           _cangeneratelist.SUpList.Count == 0 &&
           _cangeneratelist.SDownList.Count == 0 &&
           _cangeneratelist.SLeftList.Count == 0 &&
           _cangeneratelist.SRightList.Count == 0 &&
           _cangeneratelist.ZUpList.Count == 0 &&
           _cangeneratelist.ZDownList.Count == 0 &&
           _cangeneratelist.ZLeftList.Count == 0 &&
           _cangeneratelist.ZRightList.Count == 0 &&
           _cangeneratelist.LUpList.Count == 0 &&
           _cangeneratelist.LDownList.Count == 0 &&
           _cangeneratelist.LLeftList.Count == 0 &&
           _cangeneratelist.LRightList.Count == 0 &&
           _cangeneratelist.JUpList.Count == 0 &&
           _cangeneratelist.JDownList.Count == 0 &&
           _cangeneratelist.JLeftList.Count == 0 &&
           _cangeneratelist.JRightList.Count == 0)
            return false;
        return true;
    }

    

    private GridTile GetVectortoList(BlockType shape, int rotation)
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
        return null;
    }
}