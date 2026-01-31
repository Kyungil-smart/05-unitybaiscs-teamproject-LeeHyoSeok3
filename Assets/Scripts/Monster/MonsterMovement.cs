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

    // 인접노드탐색을 위한 리스트
    private List<GridTile> _nearList;
    // 열린노드 목록
    private List<GridTile> _openList;
    //private List<Vector3> _openList;

    // 닫힌노드 목록 : 다시는 볼 필요 없는 노드
    private List<GridTile> _closedList;
    //private List<Vector3> _closedList;

    // 경로리스트
    public Queue<GridTile> _pathList;
    //private List<Vector3> _pathList;

    // 잘들어가는지 확인용 [SerializeField]
    [SerializeField]public GridTile[,] GridTiles;
    [SerializeField]public GridTile NullTile;
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
    }

    public void ChasePlayer(Vector3 startPos, Vector3 targetPos)
    {
        _start = GetTile(startPos);
        _target = GetTile(targetPos);

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
        #region A* 이론
        // f(n) = g(n) + h (n)
        // 비용계산
        // g(n) : 출발노드에서 현재노드 최단비용
        // g(n)은 유클리디안 거리로 나타냄 가로세로를 10 대각선을 14로

        // h(n) : 현재노드에서 목표노드(휴리스틱거리측정) 예상 비용
        // h(n)은 맨하탄 거리로 나타냄 대각선없이 가로세로만 1칸당 10으로

        // f(n) : 출발노드에서 현재노드 최단비용에 목표노드까지의 예상 비용을 더한것
        // 휴리스틱 코스트 펑션이라고도 한다.

        //    1. 출발지점을 열린 노드에 넣는다.
        //    2. 주변 8방향 노드를 탐색해 코스트를 입력한다.
        //        - 장애물이 있거나 갈수 없는 곳은 제외
        //    3. 출발지점을 부모노드로 지정한다.
        //    4. 출발지점을 닫힌노드에 넣는다.
        //    5. 열린목록중 f(n)코스트가 제일 적은 노드를 닫힌목록에 넣는다.
        //    6. 선택한 노드의 인접노드를 탐색한다.
        //        - 장애물, 닫힌목록인 노드를 제외한 노드 열린목록에 추가
        //        - 추가한 노드의 부모노드 설정, 코스트 입력
        //    7. 인접한 노드 중 열린목록에 포함된 노드는 g(n)코스트 계산 후,
        //       g(n)코스트가 낮아졌다면 부모노드 변경, 코스트 재계산
        //    8. 목표노드가 열린목록에 들어오거나 열린목록에 아무것도 없을 때까지 5~7반복
        //    9. 목표노드가 열린목록에 들어왔다면 부모노드를 따라가 경로를 생성한다.
        #endregion
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
