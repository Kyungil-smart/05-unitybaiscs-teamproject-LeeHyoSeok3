using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterMovement
{
    private float _moveSpd;
    private float _rotateSpd;

    private Vector3 _movedir;

    // 인접노드탐색을 위한 리스트
    private List<Vector3> _findNear;
    private List<Vector3> _nearList;
    // 열린노드 목록
    //private List<GridTile> _openList;
    private List<Vector3> _openList;

    // 닫힌노드 목록 : 다시는 볼 필요 없는 노드
    //private List<GridTile> _closedList;
    private List<Vector3> _closedList;

    // 경로리스트
    //private List<GridTile> _pathList;
    private List<Vector3> _pathList;

    public MonsterMovement()
    {
        //_openList = new List<GridTile>();
        //_closedList = new List<GridTile>();
        //_pathList = new List<GridTile>();
        _openList =   new List<Vector3>();
        _closedList = new List<Vector3>();
        _pathList =   new List<Vector3>();
        _findNear = new List<Vector3>();
        SetNeardir();
    }

    public void ChasePlayer(Vector3 start, Vector3 target)
    {
        _openList.Add(start);
        FindNear(start);

        Findpath(target);

    }

    private void Findpath(Vector3 target)
    {
       
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
    }

    private void SetNeardir()
    {
        _findNear.Add(Vector3.forward);
        _findNear.Add(Vector3.back);
        _findNear.Add(Vector3.left);
        _findNear.Add(Vector3.right);
        _findNear.Add(Vector3.forward + Vector3.left);
        _findNear.Add(Vector3.forward + Vector3.right);
        _findNear.Add(Vector3.back + Vector3.left);
        _findNear.Add(Vector3.back + Vector3.right);
    }

    private void FindNear(Vector3 current)
    {
        _nearList.Clear();

        foreach (Vector3 dir in _findNear)
        {
            _nearList.Add(current + dir);
        }
    }
}
