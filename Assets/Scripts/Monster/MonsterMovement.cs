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

    private bool _canMove;

    private List<GridTile> _nearList;
    private List<GridTile> _openList;

    private List<GridTile> _closedList;

     public Stack<GridTile> _pathList;
    // 경로리스트

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
        _moveSpd = 1f;
        _rotateSpd = 18f;
        _canMove = false;
    }

    public void ChasePlayer(Vector3 startPos, Vector3 targetPos)
    {
         _start = GetTile(startPos);
        _target = GetTile(targetPos);

        if(_start == _target) { return; }

        // 占쌕쏙옙 호占쏙옙 占쏙옙占쏙옙 占쏙옙 占십깍옙화
        ResetPath();

        FindNear(_start, _target);

        _closedList.Add(_start);

        int Min = _openList[0]._f;

        foreach (GridTile t in _openList)
        {
            if (Min > t._f)
            {
                Min = t._f;
                _next = t;
            }
        }
        _openList.Remove(_next);
        _closedList.Add(_next);

        if (!(_openList.Count == 0) || !(_openList.Contains(_target)) || !(_closedList.Contains(_target)))
        {
            // ��ǥ���� Ž��
            while (!(_openList.Count == 0) || !(_openList.Contains(_target)) || !(_closedList.Contains(_target)))
            {
                if (!Findpath(_next))
                    break;
            }
        }

        if (_canMove) { BuildPath(_target); }

        else { BuildPath(_next); }
    }

    private void BuildPath(GridTile tile)
    {
        if (tile.Parents == null) { return; }
        _pathList.Push(tile);

        BuildPath(tile.Parents);
    }

    private bool Findpath(GridTile tile)
    {
        FindNear(tile, _target);

        if (_canMove)
        {
            // ���� �ڽ�Ʈ ��� Ž��
            int Min = _nearList[0]._f;

            foreach (GridTile t in _nearList)
            {
                if (_openList.Contains(t) && !(_closedList.Contains(t)))
                {
                    if (Min > t._f)
                    {
                        Min = t._f;
                        tile = t;
                    }
                }
            }
            _openList.Remove(tile);
            _closedList.Add(tile);

            return true;
        }

        else { return false; }
    }

    private void FindNear(GridTile current, GridTile target)
    {
        _nearList.Clear();
        _canMove = false;

        if (current._upBlock != null)
        {
            _nearList.Add(current._upBlock);

            if (current._upBlock._rightBlock != null)
                _nearList.Add(current._upBlock._rightBlock);

            if (current._upBlock._leftBlock != null)
                _nearList.Add(current._upBlock._leftBlock);
        }
        if (current._downBlock != null)
        {
            _nearList.Add(current._downBlock);

            if (current._downBlock._rightBlock != null)
                _nearList.Add(current._downBlock._rightBlock);

            if (current._downBlock._leftBlock != null)
                _nearList.Add(current._downBlock._leftBlock);
        }

        if(current._rightBlock != null)
            _nearList.Add(current._rightBlock);
        if(current._leftBlock != null)
            _nearList.Add(current._leftBlock);
        
       

        foreach (GridTile tile in _nearList)
        {
            if (IsReachable(tile, current))
            {
                RegistTileInfo(current, tile, target);
            }
        }
    }

    private int GetGCost(Vector3 vector)
    {
        int dx = (int)Mathf.Abs(vector.x);
        int dz = (int)Mathf.Abs(vector.z);

        int wL = 10;
        int diagonal = 14;

        return (wL* (dx + dz)) + ((diagonal- 2*wL)* Mathf.Min(dx, dz));
    }

    private int GetHCost(Vector3 vector)
    {
        int dx = (int)Mathf.Abs(vector.x);
        int dz = (int)Mathf.Abs(vector.z);

        int wL = 10;
        return wL* (dx + dz);
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

        else
        {
            _openList.Add(tile);
            _canMove = true;
            return true;
        }
    }

    private void RegistTileInfo(GridTile current, GridTile next, GridTile target)
    {
        Vector3 gVector = _start.transform.position - next.transform.position;
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
        Vector3 movement = _movedir * (_moveSpd * Time.deltaTime);

        return _rb.position + movement;
    }

    public void SetMoveDirection(Vector3 dir)
    {
        _movedir = dir;
    }

    public void Rotate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(-_movedir);
        Quaternion smoothRotation = Quaternion.Slerp(_rb.rotation, targetRotation, _rotateSpd * Time.deltaTime);

        _rb.MoveRotation(smoothRotation);
    }

    private void ResetPath()
    {
        _openList.Clear();
        _closedList.Clear();
        _pathList.Clear();
        _nearList.Clear();
    }
}
