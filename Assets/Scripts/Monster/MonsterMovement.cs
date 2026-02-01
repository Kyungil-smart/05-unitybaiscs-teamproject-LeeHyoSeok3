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

    // 인접노드탐색을 위한 리스트
    private List<GridTile> _nearList;
    // 열린노드 목록
    private List<GridTile> _openList;

    // 닫힌노드 목록 : 다시는 볼 필요 없는 노드
    private List<GridTile> _closedList;

    // 경로리스트
    public Queue<GridTile> _pathList;

    // 잘들어가는지 확인용 [SerializeField]
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

        // 다시 호출 됐을 때 초기화
        ResetPath();

        // 시작점 열린노드 추가
        _openList.Add(_start);

        // 인접노드 추가
        FindNear(_start, _target);

        // 열린노드 제거
        _openList.Remove(_start);
        // 닫힌노드 추가
        _closedList.Add(_start);

        // 목표까지 탐색
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
        // 최저 코스트 노드 탐색
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

        // 근처타일 탐색
        FindNear(tile, _target);
    }

    private void FindNear(GridTile current, GridTile target)
    {
        // 추가 전 초기화
        _nearList.Clear();
        // 근처 타일리스트 추가

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
            // 접근할 수 있는지 없는지
            if (IsReachable(tile, current))
            {
                // 접근할 수 있으면 정보등록
                RegistTileInfo(current, tile, target);
            }
        }
    }

    // g(n) 코스트 구하기
    private int GetGCost(Vector3 vector)
    {
        int dx = (int)Mathf.Abs(vector.x);
        int dz = (int)Mathf.Abs(vector.z);

        int wL = 10;
        int diagonal = 14;

        return (wL* (dx + dz)) + ((diagonal- 2*wL)* Mathf.Min(dx, dz));
    }

    // h(n) 코스트 구하기
    private int GetHCost(Vector3 vector)
    {
        int dx = (int)Mathf.Abs(vector.x);
        int dz = (int)Mathf.Abs(vector.z);

        int wL = 10;
        return wL* (dx + dz);
    }

    private void AddNear(GridTile tile)
    {
        if(tile != null)
            _nearList.Add(tile);
    }

    // 도달할 수 있는지 정하기
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

    // 그리드 타일에 g,h,f 코스트와 부모 지정하기
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
