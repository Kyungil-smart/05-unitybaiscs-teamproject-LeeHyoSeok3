using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class MonsterMovement
{
    private float _moveSpd;
    private float _rotateSpd;

    private Vector3 _movedir;
    public Rigidbody _rb;

    public bool _isArrive;

    // 占쏙옙占쏙옙占쏙옙占신쏙옙占쏙옙占 占쏙옙占쏙옙 占쏙옙占쏙옙트
    private List<GridTile> _nearList;
    // 占쏙옙占쏙옙占쏙옙占 占쏙옙占
    private List<GridTile> _openList;

    // 占쏙옙占쏙옙占쏙옙占 占쏙옙占 : 占쌕시댐옙 占쏙옙 占십울옙 占쏙옙占쏙옙 占쏙옙占
    private List<GridTile> _closedList;

    // 占쏙옙罐占쏙옙占싣
    public Queue<GridTile> _pathList;
    // 경로리스트
    public Stack<GridTile> _pathList;

    // 占쌩듸옙載∽옙占쏙옙占 확占싸울옙 [SerializeField]
    public GridTile[,] GridTiles;
    public GridTile NullTile;
    private GridTile _start;
    private GridTile _next;
    private GridTile _target;

    public MonsterMovement()
    {
        _openList = new List<GridTile>();
        _closedList = new List<GridTile>();
        _nearList = new List<GridTile>();
        _pathList = new Stack<GridTile>();
        _moveSpd = 3f;
        _rotateSpd = 18f;
        _isArrive = false;
    }

    public void ChasePlayer(Vector3 startPos, Vector3 targetPos)
    {
        _start = GetTile(startPos);
        _target = GetTile(targetPos);

        if(_start == _target)
            _isArrive = true;

        // 占쌕쏙옙 호占쏙옙 占쏙옙占쏙옙 占쏙옙 占십깍옙화
        ResetPath();

        // 占쏙옙占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙占 占쌩곤옙
        _openList.Add(_start);

        // 占쏙옙占쏙옙占쏙옙占 占쌩곤옙
        FindNear(_start, _target);

        // 占쏙옙占쏙옙占쏙옙占 占쏙옙占쏙옙
        _openList.Remove(_start);
        // 占쏙옙占쏙옙占쏙옙占 占쌩곤옙
        _closedList.Add(_start);

        // 占쏙옙표占쏙옙占쏙옙 탐占쏙옙
        while (!(_openList.Count == 0 || _openList.Contains(_target)))
        {
            Findpath(_next);
            
        }
        BuildPath(_target);
    }

    private void BuildPath(GridTile tile)
    {
        _pathList.Push(tile);

        if (tile.Parents == null) { return; }
        //BuildPath(tile.Parents);
    }

    private void Findpath(GridTile tile)
    {
        // 占쏙옙占쏙옙 占쌘쏙옙트 占쏙옙占 탐占쏙옙
        int Min = _openList[0]._f;
        tile = _openList[0];

        foreach (GridTile t in _openList)
        {
            if (Min > t._f)
            {
                Min = t._f;
                tile = t;
            }
        }

        _openList.Remove(tile);
        _closedList.Add(tile);

        // 占쏙옙처타占쏙옙 탐占쏙옙
        FindNear(tile, _target);
    }

    private void FindNear(GridTile current, GridTile target)
    {
        // 占쌩곤옙 占쏙옙 占십깍옙화
        _nearList.Clear();
        // 占쏙옙처 타占싹몌옙占쏙옙트 占쌩곤옙

        _nearList.Add(current._upBlock);
        _nearList.Add(current._downBlock);
        _nearList.Add(current._rightBlock);
        _nearList.Add(current._leftBlock);
        _nearList.Add(current._upBlock._rightBlock);
        _nearList.Add(current._upBlock._leftBlock);
        _nearList.Add(current._downBlock._rightBlock);
        _nearList.Add(current._downBlock._leftBlock);

        foreach (GridTile tile in _nearList)
        {
            // 占쏙옙占쏙옙占쏙옙 占쏙옙 占쌍댐옙占쏙옙 占쏙옙占쏙옙占쏙옙
            if (IsReachable(tile, current))
            {
                // 占쏙옙占쏙옙占쏙옙 占쏙옙 占쏙옙占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙占
                RegistTileInfo(current, tile, target);
            }
        }
    }

    // g(n) 占쌘쏙옙트 占쏙옙占싹깍옙
    private int GetGCost(Vector3 vector)
    {
        int dx = (int)Mathf.Abs(vector.x);
        int dz = (int)Mathf.Abs(vector.z);

        int wL = 10;
        int diagonal = 14;

        return (wL* (dx + dz)) + ((diagonal- 2*wL)* Mathf.Min(dx, dz));
    }

    // h(n) 占쌘쏙옙트 占쏙옙占싹깍옙
    private int GetHCost(Vector3 vector)
    {
        int dx = (int)Mathf.Abs(vector.x);
        int dz = (int)Mathf.Abs(vector.z);

        int wL = 10;
        return wL* (dx + dz);
    }

    // 占쏙옙占쏙옙占쏙옙 占쏙옙 占쌍댐옙占쏙옙 占쏙옙占싹깍옙
    private void AddNear(GridTile tile)
    {
        if(tile != null)
            _nearList.Add(tile);
    }

    // 도달할 수 있는지 정하기
    private bool IsReachable(GridTile tile, GridTile currentTile)
    {
        if (tile._blockOn || tile == NullTile || _closedList.Contains(tile) || tile == null)
        {
            return false;
        }

        else if (_openList.Contains(tile))
        {
            Vector3 gVector = currentTile.transform.position - tile.transform.position;

            if (tile._g > GetGCost(gVector)) { return true; }

            return false;
        }

        _openList.Add(tile);
        
        return true;
    }

    // 占쌓몌옙占쏙옙 타占싹울옙 g,h,f 占쌘쏙옙트占쏙옙 占싸몌옙 占쏙옙占쏙옙占싹깍옙
    private void RegistTileInfo(GridTile current, GridTile next, GridTile target)
    {
        Vector3 gVector = current.transform.position - next.transform.position;
        next._g = GetGCost(gVector);

        Vector3 hVector = next.transform.position - target.transform.position;
        next._h = GetHCost(hVector);

        next._f = next._g + next._h;

        next.Parents = current;
    }

    private GridTile GetTile(Vector3 vector)
    {
        return GridTiles[(int)vector.z, (int)vector.x];
    }

    public void Move()
    {
        _rb.MovePosition(GetNextPos());
    }

    private Vector3 GetNextPos()
    {
        Vector3 movement = _movedir * (_moveSpd * Time.fixedDeltaTime);

        return _rb.position + movement;
    }

    public void SetMoveDirection(Vector3 dir)
    {
        _movedir = dir;
    }

    public void Rotate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(-_movedir);
        Quaternion smoothRotation = Quaternion.Slerp(_rb.rotation, targetRotation, _rotateSpd * Time.fixedDeltaTime);

        _rb.MoveRotation(smoothRotation);
    }

    private void ResetPath()
    {
        _openList.Clear();
        _closedList.Clear();
        _pathList.Clear();
    }
}
