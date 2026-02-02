using System.Collections.Generic;
using UnityEngine;


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

    private Vector3 _fixPos;

    public MonsterMovement()
    {
        _openList = new List<GridTile>();
        _closedList = new List<GridTile>();
        _nearList = new List<GridTile>();
        _pathList = new Stack<GridTile>();
        _fixPos = new Vector3();

        _moveSpd = 1f;
        _rotateSpd = 18f;
    }

    public void ChasePlayer(Vector3 startPos, Vector3 targetPos)
    {
        _start = GetTile(startPos);
        _start._g = 0;
        _start._h = 0;
        _start._f = 0;

        _target = GetTile(targetPos);
        if(_target._blockOn)
        {
            FixTarget(targetPos);
        }

        if(_start == _target) { return; }

        // 占쌕쏙옙 호占쏙옙 占쏙옙占쏙옙 占쏙옙 占십깍옙화
        ResetPath();

        FindNear(_start, _target);

        _closedList.Add(_start);

        if(_openList.Count == 0) { return; }

        int Min = _openList[0]._f;
        _next = _openList[0];

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

        Findpath(_next);

        _openList.Remove(_next);
        _closedList.Add(_next);

        if ( !(_openList.Contains(_target)) || !(_closedList.Contains(_target)))
        {
            // ��ǥ���� Ž��
            while (!(_openList.Count == 0) && !(_openList.Contains(_target)) && !(_closedList.Contains(_target)))
            {
                if (Findpath(_next))
                {
                    _openList.Remove(_next);
                    _closedList.Add(_next);
                }

                else { break; }
            }
        }

        if (_openList.Count == 0) { BuildPath(_target); }

        else { BuildPath(_next); }
    }

    private void BuildPath(GridTile tile)
    {
        if (tile.Parents == null || tile == tile.Parents.Parents) { return; }
        _pathList.Push(tile);

        BuildPath(tile.Parents);
    }

    private bool Findpath(GridTile prevtile)
    {
        FindNear(_next, _target);

            // ���� �ڽ�Ʈ ��� Ž��
            int Min = _openList[0]._f;

            foreach (GridTile t in _openList)
            {
                if (!(_closedList.Contains(t)))
                {
                    if (Min >= t._f)
                    {
                        Min = t._f;
                        _next = t;
                    }
                }
            }

            if(prevtile == _next) { return false; }
            return true;
    }

    private void FindNear(GridTile current, GridTile target)
    {
        _nearList.Clear();
        _canMove = false;

        if (current._upBlock != null && !(current._upBlock._blockOn))
            _nearList.Add(current._upBlock);
        if (current._downBlock != null && !(current._downBlock._blockOn))
            _nearList.Add(current._downBlock);

        if(current._rightBlock != null && !(current._rightBlock._blockOn))
            _nearList.Add(current._rightBlock);
        if(current._leftBlock != null && !(current._leftBlock._blockOn))
            _nearList.Add(current._leftBlock);

        if (current._upBlock._rightBlock != null && 
            !(current._upBlock._blockOn) &&
            !(current._rightBlock._blockOn)
            )
            _nearList.Add(current._upBlock._rightBlock);

        if (current._upBlock._leftBlock != null && 
            !(current._upBlock._blockOn) &&
            !(current._leftBlock._blockOn))
            _nearList.Add(current._upBlock._leftBlock);

        if (current._downBlock._rightBlock != null &&
            !(current._downBlock._blockOn) &&
            !(current._rightBlock._blockOn))
            _nearList.Add(current._downBlock._rightBlock);

        if (current._downBlock._leftBlock != null &&
            !(current._downBlock._blockOn) &&
            !(current._leftBlock._blockOn))
            _nearList.Add(current._downBlock._leftBlock);


        foreach (GridTile nexttile in _nearList)
        {
            if (IsReachable(nexttile, current))
            {
                RegistTileInfo(current, nexttile, target);
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
    private bool IsReachable(GridTile nexttile, GridTile currentTile)
    {
        if (nexttile._blockOn || nexttile == NullTile || _closedList.Contains(nexttile) ||
            nexttile == null)
        {
            return false;
        }

        else if (_openList.Contains(nexttile))
        {
            Vector3 gVector = currentTile.transform.position - nexttile.transform.position;

            if (nexttile._g > currentTile._g + GetGCost(gVector)) 
            {
                nexttile.Parents = currentTile;
                return true; 
            }
            return false;
        }

        else
        {
            _openList.Add(nexttile);
            nexttile._g = 0;
            nexttile._h = 0;
            nexttile._f = 0;

            _canMove = true;
            nexttile.Parents = currentTile;
            return true;
        }
    }

    private void RegistTileInfo(GridTile current, GridTile next, GridTile target)
    {
        Vector3 gVector = current.transform.position - next.transform.position;
        next._g = current._g + GetGCost(gVector);

        Vector3 hVector = next.transform.position - target.transform.position;
        next._h = GetHCost(hVector);

        next._f = next._g + next._h;
    }

    private GridTile GetTile(Vector3 vector)
    {
        return GridTiles[ (int)Mathf.Round(vector.z), (int) Mathf.Round(vector.x)];
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

    private void FixTarget(Vector3 targetPos)
    {
        if (_target.transform.position.x > targetPos.x)
        { _fixPos.x = _target.transform.position.x - 1; }

        else if (_target.transform.position.x < targetPos.x)
        { _fixPos.x = _target.transform.position.x + 1; }

        if (_target.transform.position.y > targetPos.y)
        { _fixPos.y = _target.transform.position.y - 1; }

        else if (_target.transform.position.x < targetPos.x)
        { _fixPos.y = _target.transform.position.y + 1; }

        _target = GetTile(_fixPos);
    }

    private void ResetPath()
    {
        _openList.Clear();
        _closedList.Clear();
        _pathList.Clear();
        _nearList.Clear();
    }
}
