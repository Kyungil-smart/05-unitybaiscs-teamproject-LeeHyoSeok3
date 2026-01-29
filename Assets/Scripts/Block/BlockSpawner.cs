using System;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private float blockSize = 1f;
    [SerializeField] private float fallSpeed = 2f;

    private BlockFactory _factory;
    private List<BlockControler> _current;

    private List<List<BlockControler>> _fallingBlockps = new(); // 떨어지는 블록들을 관리하기 위한 리스트

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

        FallingBlocks();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (_current == null) return;

        Debug.Log("OnTriggerEnter");
    }

    // 생성된 블록들이 떨어지는 처리
    private void FallingBlocks()
    {
        if (_fallingBlockps != null)
        {
            foreach (var blocks in _fallingBlockps)
            {
                foreach (var block in blocks)
                {
                    if (block.State == BlockState.Falling)   // 떨어지는 상태이면 계속 떨어짐
                        block.DownGridPosition(block.GridPosition, fallSpeed);

                    if (block.YPosition <= 0.1f && block.State == BlockState.Falling) // 떨어지는 중에 땅에 닿기 직전
                    {
                        block.SetState(BlockState.Locked);
                        block.GroundGridPosition(block.GridPosition);
                    }
                }
            }
        }
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

        _fallingBlockps.Add(_current); // 생성된 블록은 생성되자마자 _fallingBlockps 리스트에 추가하여 떨어지는 상태 일괄 관리할 예정
    }
    
}