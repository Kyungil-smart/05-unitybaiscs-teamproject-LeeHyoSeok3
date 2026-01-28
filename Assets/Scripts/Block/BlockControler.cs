using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockControler
{
    public Vector2Int GridPosition { get; private set; }
    public BlockState State { get; private set; }
    
    private readonly BlockView _view;
    private readonly BlockPoolType _poolType;
    private readonly float _blockSize;

    public BlockControler(BlockView view, Vector2Int gridPosition, float blockSize, BlockPoolType poolType)
    {
        _view = view;
        _blockSize = blockSize;
        
        State = BlockState.Spawn;
        _poolType = poolType;
        SetGridPosition(gridPosition);
    }
    
    public void SetGridPosition(Vector2Int gridPosition)
    {
        GridPosition = gridPosition;
        Vector3 worldPos = new Vector3(gridPosition.x * _blockSize, 0, gridPosition.y * _blockSize);
        
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
    
}