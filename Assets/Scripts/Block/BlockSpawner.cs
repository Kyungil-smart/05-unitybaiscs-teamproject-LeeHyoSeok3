using System;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private float blockSize = 1f;
    
    private BlockFactory _factory;
    private List<BlockControler> _current;

    private void Start()
    {
        _factory = new BlockFactory(blockSize);
    }

    // 테스트 코드
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            SpawnRandom();
    }

    public void SpawnRandom()
    {
        BlockType type = (BlockType)Random.Range(0, 7);
        BlockPoolType poolType = (BlockPoolType)Random.Range(0, 7);

        // todo : Grid에서 생성가능한 좌표 가져오기
        Vector2Int baseGrid = new( Random.Range(-5, 5),  Random.Range(-5, 5));
        _current = _factory.Create(type, poolType, baseGrid);

        foreach (var block in _current)
            block.SetState(BlockState.Falling);
    }
    
}