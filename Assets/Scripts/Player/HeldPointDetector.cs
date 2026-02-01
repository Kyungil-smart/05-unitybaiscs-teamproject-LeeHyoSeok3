using UnityEngine;

public class HeldPointDetector : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _gridSize = 1f;
    [SerializeField] private int _defaultForwardOffset = 2;
    [SerializeField] private int _maxDistance = 2;

    private Vector2Int _playerGrid;
    private Vector2Int _currentGrid;
    private Vector2Int _offsetFromPlayer;

    private BlockGroup _holdGroup;

    private void Start()
    {
        _playerGrid = WorldToGrid(_player.position);
        _currentGrid = _playerGrid + Vector2Int.up * _defaultForwardOffset;
        _offsetFromPlayer = _currentGrid - _playerGrid;

        SyncWorldPosition();
    }

    private void Update()
    {
        FollowPlayerGrid();
        HandleInput();
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

    private void FollowPlayerGrid()
    {
        Vector2Int newPlayerGrid = WorldToGrid(_player.position);
        if (newPlayerGrid == _playerGrid)
            return;

        _playerGrid = newPlayerGrid;
        if (_holdGroup == null)
            return;

        Vector2Int desiredPivot = _playerGrid + _offsetFromPlayer;

        if (_holdGroup.TryMovePivot(desiredPivot, out var resolvedPivot))
        {
            _currentGrid = resolvedPivot;
            _offsetFromPlayer = _currentGrid - _playerGrid;
            SyncWorldPosition();
        }
    }

    private void HandleInput()
    {
        if (_holdGroup == null)
            return;

        Vector2Int dir = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.LeftArrow))  dir = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.RightArrow)) dir = Vector2Int.right;
        if (Input.GetKeyDown(KeyCode.UpArrow))    dir = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.DownArrow))  dir = Vector2Int.down;

        if (dir == Vector2Int.zero)
            return;

        TryMove(dir);
    }

    private void TryMove(Vector2Int dir)
    {
        if (_holdGroup == null)
            return;

        Vector2Int desiredPivot = _holdGroup.PivotGrid + dir;

        if (!_holdGroup.TryMovePivot(desiredPivot, out var resolvedPivot))
            return;

        _currentGrid = resolvedPivot;
        _offsetFromPlayer = _currentGrid - _playerGrid;

        SyncWorldPosition();
    }

    private void SyncWorldPosition()
    {
        transform.position = new Vector3(
            _currentGrid.x * _gridSize,
            transform.position.y,
            _currentGrid.y * _gridSize
        );
    }

    private static Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPos.x),
            Mathf.RoundToInt(worldPos.z)
        );
    }

    public void SetHoldGroup(BlockGroup holdGroup)
    {
        _holdGroup = holdGroup;

        if (_holdGroup == null)
            return;
        
        _playerGrid = WorldToGrid(_player.position);
        _currentGrid = _playerGrid + Vector2Int.up * _defaultForwardOffset;
        _offsetFromPlayer = _currentGrid - _playerGrid;
        
        SyncWorldPosition();
    }
    
    public void OnGroupPivotChanged(Vector2Int newPivot)
    {
        _currentGrid = newPivot;
        _offsetFromPlayer = _currentGrid - _playerGrid;
        SyncWorldPosition();
    }
}