using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BlockControler
{
    public Vector2Int GridPosition { get; private set; }
    public BlockState State { get; private set; }

    public float YPosition; // 위에서 떨어지는 효과를 위한 Y 위치 값

    private readonly BlockView _view;
    private readonly BlockPoolType _poolType;
    private readonly float _blockSize;

    public BlockControler(BlockView view, Vector2Int gridPosition, float blockSize, BlockPoolType poolType)
    {
        _view = view;
        _blockSize = blockSize;
        
        State = BlockState.Spawn;
        _poolType = poolType;

        YPosition = 10f;    // y=10 에서 시작

        SetGridPosition(gridPosition);
    }
    
    public void SetGridPosition(Vector2Int gridPosition)
    {
        GridPosition = gridPosition;
        Vector3 worldPos = new Vector3(gridPosition.x * _blockSize, YPosition, gridPosition.y * _blockSize);
        
        _view.SetWorldPostion(worldPos);
    }

    public void SetState(BlockState state) => State = state;
    public bool IsMoveable() {
        return  State == BlockState.Spawn || State == BlockState.Falling || State == BlockState.Locked;
    }
    
    public void Release() {
        SetState(BlockState.Release);
        PoolManager.Instance.GetPool<BlockView>((int)_poolType).Release(_view);
    }
    
    // 아래로 이동하는 로직
    public void DownGridPosition(Vector2Int BlockMovePosition, float FallSpeed) {
        GridPosition = BlockMovePosition;

        float NextY = YPosition - FallSpeed * Time.deltaTime;

        Vector3 MovePos = new Vector3(BlockMovePosition.x * _blockSize, NextY, BlockMovePosition.y * _blockSize);
        _view.SetWorldPostion(MovePos);
        YPosition = NextY;
    }

    // 땅에 착지할 때 지면에 위치 고정
    public void GroundGridPosition(Vector2Int BlockMovePosition)
    {
        GridPosition = BlockMovePosition;

        float NextY = 0f;

        Vector3 MovePos = new Vector3(BlockMovePosition.x * _blockSize, NextY, BlockMovePosition.y * _blockSize);
        _view.SetWorldPostion(MovePos);
        YPosition = NextY;
    }
}