using UnityEngine;

public class HeldPointDetector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _player;

    [Header("Grid Settings")]
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
        
        Vector2Int desiredGrid = _playerGrid + _offsetFromPlayer;

        if (!IsWithinSquareRange(desiredGrid))
        {
            int clampedX = Mathf.Clamp(
                _offsetFromPlayer.x,
                -_maxDistance,
                _maxDistance
            );

            int clampedY = Mathf.Clamp(
                _offsetFromPlayer.y,
                -_maxDistance,
                _maxDistance
            );

            _offsetFromPlayer = new Vector2Int(clampedX, clampedY);
            desiredGrid = _playerGrid + _offsetFromPlayer;
        }
        
        _currentGrid = _playerGrid + _offsetFromPlayer;
        SyncWorldPosition();
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
    
    private bool IsWithinSquareRange(Vector2Int grid)
    {
        int dx = Mathf.Abs(grid.x - _playerGrid.x);
        int dy = Mathf.Abs(grid.y - _playerGrid.y);

        return dx <= _maxDistance && dy <= _maxDistance;
    }

    private void TryMove(Vector2Int dir)
    {
        Vector2Int nextGrid = _currentGrid + dir;
        
        if (nextGrid == _playerGrid)
            return;

        if (!IsWithinSquareRange(nextGrid))
            return;

        _currentGrid = nextGrid;
        _offsetFromPlayer = _currentGrid - _playerGrid; // ⭐ offset 갱신

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

    private void OnTriggerEnter(Collider other)
    {
        if (_holdGroup == null) return;
        if (!other.TryGetComponent<GridTile>(out var tile)) return;

        _holdGroup.SnapRootToGrid(tile.GridPos);
    }
}