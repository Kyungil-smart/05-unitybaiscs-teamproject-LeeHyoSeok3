using System.Collections.Generic;
using UnityEngine;

public class BlockGroup
{
    private static readonly Vector2Int[] WallKickOffsets =
    {
        Vector2Int.zero,
        Vector2Int.left,
        Vector2Int.right,
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left + Vector2Int.up,
        Vector2Int.right + Vector2Int.up,
    };
    
    private static readonly Vector2Int[] RotateKickOffsets =
    {
        Vector2Int.zero,
        Vector2Int.left,
        Vector2Int.right,
        Vector2Int.up,
        Vector2Int.left + Vector2Int.up,
        Vector2Int.right + Vector2Int.up,
    };
    
    public BlockType BlockType { get; }
    public BlockPoolType  PoolType { get; }
    
    public Vector2Int PivotGrid { get; private set; }
    public IReadOnlyList<BlockControler> Blocks => _blocks;
    public Transform Root { get; }
    private readonly List<BlockControler> _blocks;
    private Transform _followTarget;
    private Vector3 _holdOffset;
    private HeldPointDetector _heldPoint;
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
        _heldPoint = heldPoint;
        _followTarget = _heldPoint.transform;
        _isHeld = true;
        _dropY = dropY;
        
        _heldPoint.SetHoldGroup(this);
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
        
        
        _heldPoint.SetHoldGroup(null);
        _heldPoint = null;
        DestroyGhost();
    }
    
    private void ApplyPivot(Vector2Int pivot)
    {
        PivotGrid = pivot;
        Root.position = GridToWorld(pivot);
        UpdateGhostTransform();
    }
    
    private bool CanPlaceAt(Vector2Int pivot)
    {
        foreach (var block in _blocks)
        {
            Vector2Int grid = pivot + block.LocalOffset;
            if (!HasTile(grid))
                return false;
        }
        return true;
    }
    
    private bool HasTile(Vector2Int grid)
    {
        Vector3 world = new Vector3(grid.x, 0f, grid.y);

        return Physics.CheckBox(
            world,
            Vector3.one * 0.45f,
            Quaternion.identity,
            LayerMask.GetMask("Tile")
        );
    }
    
    public bool TryMovePivot(Vector2Int desiredPivot, out Vector2Int resolvedPivot)
    {
        foreach (var offset in WallKickOffsets)
        {
            Vector2Int testPivot = desiredPivot + offset;

            if (!CanPlaceAt(testPivot))
                continue;

            ApplyPivot(testPivot);
            resolvedPivot = testPivot;
            return true;
        }

        resolvedPivot = PivotGrid;
        return false;
    }
    
    private static Vector3 GridToWorld(Vector2Int grid)
    {
        return new Vector3(grid.x, 0f, grid.y); // gridSize = 1 기준 통일
    }
    
    // ------------------------
    // Rotate
    // ------------------------
    
    public void RotateWithWallKick(bool clockwise)
    {
        List<Vector2Int> backup = new();
        foreach (var block in _blocks)
            backup.Add(block.LocalOffset);
        
        foreach (var block in _blocks)
            block.RotateLocalOffset(clockwise);
        
        foreach (var kick in RotateKickOffsets)
        {
            Vector2Int testPivot = PivotGrid + kick;

            if (!CanPlaceAt(testPivot))
                continue;
            
            PivotGrid = testPivot;
            Root.position = GridToWorld(PivotGrid);
            SyncVisuals();
            UpdateGhostTransform();
            
            _heldPoint?.OnGroupPivotChanged(PivotGrid);
            return;
        }
        
        for (int i = 0; i < _blocks.Count; i++)
            _blocks[i].SetLocalOffset(backup[i]);

        SyncVisuals();
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

    private void SyncVisuals()
    {
        foreach (var block in _blocks)
        {
            block.View.transform.localPosition =
                new Vector3(
                    block.LocalOffset.x,
                    0f,
                    block.LocalOffset.y
                );
        }

        UpdateGhostTransform();
    }
    
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

    // 블럭그룹의 PoolType 반환 메서드
    public BlockPoolType GetPoolType()
    {
        return PoolType;
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