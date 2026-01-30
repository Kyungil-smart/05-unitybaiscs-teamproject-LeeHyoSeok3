using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol_Monster : MonoBehaviour
{
    [SerializeField][Range(1,20)] float _moveSpeed;
    GridTile _currentTile;
    GridTile _nextTile;
    Coroutine _movecoroutine;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision) // 떨어지는 블록에 닿으면
    {
        if(collision.gameObject.CompareTag("Block"))
        {
            if(collision.transform.position.y > transform.position.y)
            {
                Explosion();
            }
        }      
    }
    void Explosion() // 블럭 충돌 조건에서 발생할 폭발 메서드
    {
        Debug.Log("펑!");
        // 이펙트 출력
        // 플레이어를 향해서 레이캐스트
        // 플레이어 스테이트 변경?
    }

    void Patrol()
    {
        // 일정 주기로 이동 방향 판단
        //  Move()
    }

    void Move()
    {
        // transform.position? Velocity?
    }
    void Rotate()  // 다음 위치를 바라보는 회전 로직
    {
        Quaternion targetTile = Quaternion.LookRotation(_nextTile.transform.position, transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetTile, 0.2f);
    }

    void OnTriggerEnter(Collider other)
    {
        // 타일 검출 2개에 걸치면 ?
    }
}
