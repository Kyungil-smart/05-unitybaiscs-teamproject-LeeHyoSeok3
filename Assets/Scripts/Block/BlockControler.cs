using UnityEngine;

public class BlockControler
{
    public Vector2Int GridPosition { get; private set; }
    public Vector2Int LocalOffset { get; private set; }

    public BlockState State { get; private set; }
    public BlockGroup Group { get; private set; }
    public BlockView View { get; }

    private readonly float _blockSize;
    private readonly BlockPoolType _poolType;

    // public float YPosition { get; private set; }

    public BlockControler(
        BlockView view,
        Vector2Int gridPosition,
        float blockSize,
        BlockPoolType poolType)
    {
        View = view;
        _blockSize = blockSize;
        _poolType = poolType;

        State = BlockState.Spawn;

        SetGridPosition(gridPosition, 3f);
        LocalOffset = gridPosition;
    }

    // ------------------------
    // Group
    // ------------------------

    public void SetGroup(BlockGroup group)
    {
        Group = group;
        // LocalOffset = GridPosition - group.PivotGrid;
    }

    // ------------------------
    // Grid / World
    // ------------------------

    public void SetGridPosition(Vector2Int grid, float dropY)
    {
        GridPosition = grid;
        SyncWorldPosition(dropY);
    }

    private void SyncWorldPosition(float dropY)
    {
        Vector3 worldPos = new Vector3(
            GridPosition.x * _blockSize,
            dropY,
            GridPosition.y * _blockSize
        );

        View.SetWorldPosition(worldPos);
    }

    // ------------------------
    // State
    // ------------------------

    public void SetState(BlockState state)
    {
        State = state;
    }

    public bool IsMoveable()
    {
        return State is BlockState.Spawn or BlockState.Falling or BlockState.Locked;
    }

    // ------------------------
    // Pickup Drop
    // ------------------------

    public void PickUp()
    {
        View.DisableCollision();
        SetState(BlockState.Held);
    }

    public void Drop()
    {
        View.EnableCollision();
        SetState(BlockState.Landed);
    }
    
    
    // ------------------------
    // Pickup Drop
    // ------------------------

    public void RotateLocalOffset(bool clockwise)
    {
        LocalOffset = clockwise ?
            new Vector2Int(LocalOffset.y, -LocalOffset.x) :
            new Vector2Int(-LocalOffset.y, LocalOffset.x);
    }
    
    // ------------------------
    // Pool
    // ------------------------

    public void Release()
    {
        SetState(BlockState.Release);
        PoolManager.Instance
            .GetPool<BlockView>((int)_poolType)
            .Release(View);
    }
}