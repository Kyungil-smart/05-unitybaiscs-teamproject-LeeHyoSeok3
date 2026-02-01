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

    // �������Ž���� ���� ����Ʈ
    private List<GridTile> _nearList;
    // ������� ���
    private List<GridTile> _openList;

    // ������� ��� : �ٽô� �� �ʿ� ���� ���
    private List<GridTile> _closedList;

    // ��θ���Ʈ
    public Queue<GridTile> _pathList;

    // �ߵ����� Ȯ�ο� [SerializeField]
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
        _pathList = new Queue<GridTile>();
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

        // �ٽ� ȣ�� ���� �� �ʱ�ȭ
        ResetPath();

        // ������ ������� �߰�
        _openList.Add(_start);

        // ������� �߰�
        FindNear(_start, _target);

        // ������� ����
        _openList.Remove(_start);
        // ������� �߰�
        _closedList.Add(_start);

        // ��ǥ���� Ž��
        while (!(_openList.Count == 0 || _openList.Contains(_target)))
        {
            Findpath(_next);
            
        }
        BuildPath(_target);
    }

    private void BuildPath(GridTile tile)
    {
        _pathList.Enqueue(tile);

        if (tile.Parents == null) { return; }
        //BuildPath(tile.Parents);
    }

    private void Findpath(GridTile tile)
    {
        // ���� �ڽ�Ʈ ��� Ž��
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

        // ��óŸ�� Ž��
        FindNear(tile, _target);
    }

    private void FindNear(GridTile current, GridTile target)
    {
        // �߰� �� �ʱ�ȭ
        _nearList.Clear();
        // ��ó Ÿ�ϸ���Ʈ �߰�

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
            // ������ �� �ִ��� ������
            if (IsReachable(tile, current))
            {
                // ������ �� ������ �������
                RegistTileInfo(current, tile, target);
            }
        }
    }

    // g(n) �ڽ�Ʈ ���ϱ�
    private int GetGCost(Vector3 vector)
    {
        int dx = (int)Mathf.Abs(vector.x);
        int dz = (int)Mathf.Abs(vector.z);

        int wL = 10;
        int diagonal = 14;

        return (wL* (dx + dz)) + ((diagonal- 2*wL)* Mathf.Min(dx, dz));
    }

    // h(n) �ڽ�Ʈ ���ϱ�
    private int GetHCost(Vector3 vector)
    {
        int dx = (int)Mathf.Abs(vector.x);
        int dz = (int)Mathf.Abs(vector.z);

        int wL = 10;
        return wL* (dx + dz);
    }

    // ������ �� �ִ��� ���ϱ�
    private bool IsReachable(GridTile tile, GridTile currentTile)
    { 
        if (tile._blockOn || tile == NullTile || _closedList.Contains(tile))
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

    // �׸��� Ÿ�Ͽ� g,h,f �ڽ�Ʈ�� �θ� �����ϱ�
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
