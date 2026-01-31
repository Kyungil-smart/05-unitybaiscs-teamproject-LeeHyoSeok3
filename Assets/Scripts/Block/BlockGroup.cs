using System.Collections.Generic;
using UnityEngine;

public class BlockGroup
{
    public BlockType BlockType { get; }
    public BlockPoolType  PoolType { get; }
    
    public Vector2Int PivotGrid { get; private set; }
    public IReadOnlyList<BlockControler> Blocks => _blocks;
    public Transform Root { get; }
    private readonly List<BlockControler> _blocks;
    private Transform _followTarget;
    private Vector3 _holdOffset;
    private bool _isHeld;

    private Transform _ghostRoot;
    private ObjectPool<GhostBlock> _ghostPool;
    private List<GhostBlock> _ghostBlocks;

    private float _dropY;

    public BlockGroup(
        BlockType blockType,
        BlockPoolType poolType,
        Vector2Int pivotGrid,
        List<BlockControler> blocks)
    {
        BlockType = blockType;
        PoolType = poolType;
        PivotGrid = pivotGrid;
        _blocks = blocks;

        Root = new GameObject($"BlockGroupRoot_{blockType}").transform;

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

    public void PickUp(HeldPointDetector heldPoint, float dropY)
    {
        _followTarget = heldPoint.transform;
        _isHeld = true;
        _dropY = dropY;
        
        heldPoint.SetHoldGroup(this);
        CreateGhost(PoolManager.Instance.GetPool<GhostBlock>((int)PoolType));

        foreach (var block in _blocks)
        {
            block.PickUp();
            Vector3 local = block.View.transform.localPosition;
            local.y = 0f;
            block.View.transform.localPosition = local;
        }
        
    }

    public void FollowHeld()
    {
        // if (!_isHeld || _followTarget == null)
        //     return;
        //
        // Vector3 targetPos =
        //     _followTarget.position +
        //     _followTarget.forward * 1f;
        //
        // targetPos.y = 1.5f;
        //
        // BlockControler closest = FindClosestBlock(_followTarget.position);
        // if (closest == null)
        //     return;
        //
        // Vector3 blockPos = closest.View.transform.position;
        //
        // Vector3 delta = targetPos - blockPos;
        // Root.position += delta;
        //
        // UpdateGhostTransform();
        
        if (!_isHeld || _followTarget == null)
            return;
        
        Vector3 targetPos = _followTarget.position;
        targetPos.y = _dropY;

        Root.position = targetPos;

        UpdateGhostTransform();
    }

    public void Drop()
    {
        _isHeld = false;
        _followTarget = null;

        PivotGrid = WorldToGrid(Root.position);

        foreach (var block in _blocks)
        {
            block.Drop();
            block.SetGridPosition(PivotGrid + block.LocalOffset, _dropY);
        }
        
        // SyncRootToGrid();
        DestroyGhost();
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

    public void SnapRootToGrid(Vector2Int targetGrid)
    {
        if (!_isHeld) return;
        
        Vector3 targetWorldPos = new Vector3(
            targetGrid.x,
            Root.position.y,
            targetGrid.y
        );

        Root.position = targetWorldPos;

        UpdateGhostTransform();
    }
    
    // ------------------------
    // Rotate
    // ------------------------

    public void Rotate(bool clockwise)
    {
        foreach (var block in _blocks)
        {
            block.RotateLocalOffset(clockwise);
            
            // 해당 로직 controler로 옮기기.
            Vector3 localWorld =
                new Vector3(
                    block.LocalOffset.x * 1f,
                    0,
                    block.LocalOffset.y * 1f
                );

            block.View.transform.localPosition = localWorld;
        }
    }
    

    // ------------------------
    // Ghost
    // ------------------------

    public void CreateGhost(ObjectPool<GhostBlock> pool)
    {
        if (_ghostRoot != null) return;

        _ghostPool = pool;
        _ghostRoot = new GameObject("GhostRoot").transform;
        _ghostBlocks = new List<GhostBlock>();

        foreach (var block in _blocks)
        {
            var ghost = _ghostPool.Get();
            ghost.transform.SetParent(_ghostRoot, false);
            _ghostBlocks.Add(ghost);
        }

        UpdateGhostTransform();
    }
    
    public void UpdateGhostTransform()
    {
        if(_ghostRoot == null || _ghostBlocks == null) return;
        
        Vector2Int grid = WorldToGrid(Root.position);
        _ghostRoot.position = new Vector3(grid.x, 0f, grid.y);

        for (int i = 0; i < _ghostBlocks.Count; i++)
        {
            if(_ghostBlocks[i] == null) continue;
            
            _ghostBlocks[i].transform.localPosition =
                new Vector3(
                    _blocks[i].LocalOffset.x,
                    0f,
                    _blocks[i].LocalOffset.y
                );
        }
    }

    private void DestroyGhost()
    {
        foreach (var ghost in _ghostBlocks)
            ghost.OnDespawn();
        
        GameObject.Destroy(_ghostRoot.gameObject);

        _ghostRoot = null;
        _ghostBlocks = null;
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

    // 라인 클리어 전 그룹 내 블록 참조를 위한 메서드
    public List<BlockControler> GetBlockList()
    {
        return _blocks;
    }

    // 블럭그룹 내 블럭들이 상호작용 가능한지 여부 반환, 하나라도 Landed상태가 아니면 false 반환
    public bool IsInteract()
    {
        foreach (var block in _blocks)
        {
            if (block.State != BlockState.Landed)
                return false;
        }
        return true;
    }
}