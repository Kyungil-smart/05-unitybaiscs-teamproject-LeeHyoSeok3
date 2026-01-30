using System.Collections.Generic;
using UnityEngine;

public class BlockGroup
{
    public BlockType Type { get; }
    public Vector2Int PivotGrid { get; private set; }
    public IReadOnlyList<BlockControler> Blocks => _blocks;
    public Transform Root { get; }

    private readonly List<BlockControler> _blocks;
    private Transform _followTarget;
    private Vector3 _holdOffset;
    private bool _isHeld;

    public BlockGroup(
        BlockType type,
        Vector2Int pivotGrid,
        List<BlockControler> blocks)
    {
        Type = type;
        PivotGrid = pivotGrid;
        _blocks = blocks;

        Root = new GameObject($"BlockGroupRoot_{type}").transform;

        foreach (var block in _blocks)
        {
            block.SetGroup(this);
            block.View.transform.SetParent(Root, true);
            block.SetState(BlockState.Falling);
        }

        SyncRootToGrid();

        //GameEventBus.Subscribe<BlockCollisionToFloor>(SetBlocksStateToLanded);  // 바닥 충돌 이벤트 구독
    }

    // ------------------------
    // Pick / Follow / Drop
    // ------------------------

    public void PickUp(Transform holdPoint)
    {
        _followTarget = holdPoint;
        _isHeld = true;
        _holdOffset = Root.position - holdPoint.position;
    }

    public void FollowHeld()
    {
        if (!_isHeld || _followTarget == null)
            return;

        Root.position = _followTarget.position + _holdOffset;
    }

    public void Drop(float y)
    {
        _isHeld = false;
        _followTarget = null;

        PivotGrid = WorldToGrid(Root.position);

        foreach (var block in _blocks)
            block.SetGridPosition(PivotGrid + block.LocalOffset, y);

        SyncRootToGrid();
    }

    // ------------------------
    // Utilities
    // ------------------------

    private void SyncRootToGrid()
    {
        Root.position = new Vector3(
            PivotGrid.x,
            0f,
            PivotGrid.y
        );
    }

    private static Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPos.x),
            Mathf.RoundToInt(worldPos.z)
        );
    }

    // ------------------------
    // Visual
    // ------------------------

    public void SetOutline(Color color)
    {
        foreach (var block in _blocks)
            block.View.ShowOutLine(color);
    }

    public void HideOutline()
    {
        foreach (var block in _blocks)
            block.View.HideOutLine();
    }

    // ------------------------
    // Block State
    // ------------------------

    // 블록이 바닥에 닿았을 때 블록의 상태를 Landed로 변경
    /*
    public void SetBlocksStateToLanded(BlockCollisionToFloor _event)
    {

        foreach (var block in _blocks)
        {
            block.SetState(BlockState.Landed);
            Debug.Log($"Block State = {block.State}");
        }

    }
    */
}