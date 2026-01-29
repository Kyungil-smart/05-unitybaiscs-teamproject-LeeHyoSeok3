using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BlockControler
{
    public Vector2Int GridPosition { get; private set; }
    public BlockState State { get; private set; }

    public float YPosition; // ������ �������� ȿ���� ���� Y ��ġ ��

    private readonly BlockView _view;
    private readonly BlockPoolType _poolType;
    private readonly float _blockSize;

    public BlockControler(BlockView view, Vector2Int gridPosition, float blockSize, BlockPoolType poolType)
    {
        _view = view;
        _blockSize = blockSize;

        State = BlockState.Spawn;
        _poolType = poolType;

        YPosition = 10f; // y=10 ���� ����

        SetGridPosition(gridPosition);
    }

    public void SetGridPosition(Vector2Int gridPosition)
    {
        GridPosition = gridPosition;
        Vector3 worldPos = new Vector3(gridPosition.x * _blockSize, YPosition, gridPosition.y * _blockSize);

        _view.SetWorldPosition(worldPos);
    }

    public void SetState(BlockState state) => State = state;

    public bool IsMoveable()
    {
        return State == BlockState.Spawn || State == BlockState.Falling || State == BlockState.Locked;
    }

    public void Release()
    {
        SetState(BlockState.Release);
        PoolManager.Instance.GetPool<BlockView>((int)_poolType).Release(_view);
    }

    public void DownGridPosition(Vector2Int BlockMovePosition, float FallSpeed)
    {
        GridPosition = BlockMovePosition;

        float NextY = YPosition - FallSpeed * Time.deltaTime;

        Vector3 MovePos = new Vector3(BlockMovePosition.x * _blockSize, NextY, BlockMovePosition.y * _blockSize);
        _view.SetWorldPosition(MovePos);
        YPosition = NextY;
    }

    public void PickUp(Transform followTarget)
    {
        SetState(BlockState.Held);
        _view.AttachTo(followTarget);
    }

    public void Drop(Vector2Int dropGridPos, float dropStartY)
    {
        SetState(BlockState.Falling);
        YPosition = dropStartY;
        SetGridPosition(dropGridPos);
        
        _view.Detach();
    }
}
    
    