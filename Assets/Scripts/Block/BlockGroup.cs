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
    }

    // ------------------------
    // Pick / Follow / Drop
    // ------------------------

    public void PickUp(Transform holdPoint)
    {
        _followTarget = holdPoint;
        _isHeld = true;
        
        foreach (var block in  _blocks)
            block.PickUp();
    }

    public void FollowHeld()
    {
        if (!_isHeld || _followTarget == null)
            return;
        
        Vector3 targetPos =
            _followTarget.position +
            _followTarget.forward * 1f;

        targetPos.y = 1.5f;
        
        BlockControler closest = FindClosestBlock(_followTarget.position);
        if (closest == null)
            return;

        Vector3 blockPos = closest.View.transform.position;

        Vector3 delta = targetPos - blockPos;
        Root.position += delta;
    }

    public void Drop(float y)
    {
        _isHeld = false;
        _followTarget = null;

        PivotGrid = WorldToGrid(Root.position);

        foreach (var block in _blocks)
        {
            block.SetGridPosition(PivotGrid + block.LocalOffset, y);
            block.Drop();
        }
        
        SyncRootToGrid();
    }
    
    private BlockControler FindClosestBlock(Vector3 offset)
    {
        BlockControler closest = null;
        float minSqrDist = float.MaxValue;

        foreach (var block in _blocks)
        {
            float sqrDist =
                (block.View.transform.position - offset).sqrMagnitude;

            if (sqrDist < minSqrDist)
            {
                minSqrDist = sqrDist;
                closest = block;
            }
        }

        return closest;
    }
    
    // ------------------------
    // Rotate
    // ------------------------

    public void Rotate(bool clockwise)
    {
        // foreach (var block in _blocks)
        //     block.RotateLocalOffset(clockwise);
        //
        // foreach (var block in _blocks)
        //     block.SetGridPosition(
        //         PivotGrid + block.LocalOffset,
        //         block.View.transform.position.y
        //     );
        //
        // Debug.Log($"Pivot: {PivotGrid}, Root: {Root.position}");
        foreach (var block in _blocks)
        {
            block.RotateLocalOffset(clockwise);
            
            // 해당 로직 controler로 옮기기.
            Vector3 localWorld =
                new Vector3(
                    block.LocalOffset.x * 1f,
                    2f,
                    block.LocalOffset.y * 1f
                );

            block.View.transform.localPosition = localWorld;
        }
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
}