using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using Random = UnityEngine.Random;

public class test : MonoBehaviour
{
    [SerializeField] private float blockSize = 1f;
    [SerializeField] private float fallSpeed = 2f;
    [SerializeField] private CangenerateBolockList _cangeneratelist;

    private BlockFactory _factory;
    private BlockGroup _current;

    // private List<List<BlockControler>> _fallingBlockps = new(); // 떨어지는 블록들을 관리하기 위한 리스트

    private void Start()
    {
        _factory = new BlockFactory(blockSize);
    }

    // 테스트 코드
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnRandom();
        }

        // FallingBlocks();
        if (Input.GetKeyDown(KeyCode.Q)) // 테스트 코드
        {
            GameEventBus.Raise(new GridUpdateEvent());
            Debug.Log("업데이트 이벤트");
        }

        if (Input.GetKeyDown(KeyCode.E)) // 테스트 코드
        {
            SpawnObstacle(Random.Range(1,5));
            // Debug.Log("업데이트 이벤트");
        }
        //
        // if (_current != null)
        // {
        //     foreach (var block in _current)
        //     {
        //         block.DownGridPosition(block.GridPosition, fallSpeed);
        //     }
        // }
    }

    void FixedUpdate()
    {
        // GameEventBus.Raise(new GridUpdateEvent());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_current == null) return;

        Debug.Log("OnTriggerEnter");
        
    }

    // 생성된 블록들이 떨어지는 처리
    private void FallingBlocks()
    {
        // if (_fallingBlockps != null)
        // {
        //     foreach (var blocks in _fallingBlockps)
        //     {
        //         foreach (var block in blocks)
        //         {
        //             if (block.State == BlockState.Falling)   // 떨어지는 상태이면 계속 떨어짐
        //                 block.DownGridPosition(block.GridPosition, fallSpeed);
        //
        //             if (block.YPosition <= 0.1f && block.State == BlockState.Falling) // 떨어지는 중에 땅에 닿기 직전
        //             {
        //                 block.SetState(BlockState.Locked);
        //                 block.GroundGridPosition(block.GridPosition);
        //             }
        //         }
        //     }
        // }
    }

    public void SpawnRandom()
    {
        BlockType type = (BlockType)Random.Range(0, 7);
        BlockPoolType poolType = (BlockPoolType)Random.Range(0, 7);

        // 테스트용 랜덤 위치 생성
        //Vector2Int TestPosition = new Vector2Int(Random.Range(-5,5), Random.Range(-5,5));
        // Vector2Int TestPosition = new Vector2Int(0, -4);
        // 테스트용 블록 생성 코드
        // _current = _factory.Create(type, poolType, TestPosition);

       // foreach (var block in _current)
            //block.SetState(BlockState.Falling);

        //_fallingBlockps.Add(_current); // 생성된 블록은 생성되자마자 _fallingBlockps 리스트에 추가하여 떨어지는 상태 일괄 관리할 예정
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

            
            
            //_fallingBlockps.Add(_current); // 생성된 블록은 생성되자마자 _fallingBlockps 리스트에 추가하여 떨어지는 상태 일괄 관리할 예정
            // type 추출 코드 테스트
        }
    }

    public void SpawnObstacle(int howmany)
    {
        BlockPoolType obstacle = BlockPoolType.Rock1; // 방해물 pool 타입 
        BlockType type = BlockType.Ob; // 방해물 타입 [0,0]
        List<GridTile> Grid = new List<GridTile>(howmany);

        if(_cangeneratelist.ObList.Count < howmany) // 빈 자리보다 생성 블럭이 많을 때 처리
        {
            howmany = _cangeneratelist.ObList.Count;
        }

        for(int i = 0; i < howmany; i++) // 요청한 수 만큼 반복
        {
            // 좌표 탐색
            
            // _cangeneratelist.ObList 에서 좌표 받아오기
            int index = Random.Range(0,_cangeneratelist.ObList.Count);
            while(!Grid.Contains(_cangeneratelist.ObList[index]))
            {
                index = Random.Range(0,_cangeneratelist.ObList.Count);
            }
            Grid.Add(_cangeneratelist.ObList[index]);


            // 생성
            _factory.Create(type, obstacle, new Vector2Int((int)Grid[i].transform.position.x, (int)Grid[i].transform.position.z));
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