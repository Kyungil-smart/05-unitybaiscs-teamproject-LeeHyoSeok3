using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol_Monster : MonoBehaviour
{
    [SerializeField][Range(1,20)] float _moveSpeed;
    GridTile _currentTile;
    GridTile _nextTile;
    Coroutine _movecoroutine;
    LayerMask _gridLayer;

    void Start()
    {
        _gridLayer = LayerMask.NameToLayer("Grid");
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
        // 플레이어를 향해서 레이캐스트
        // 플레이어 스테이트 변경?
        Debug.Log("펑!");
        // 이펙트 출력
        
    }

    

    IEnumerator Patrol()
    {
        if(_currentTile._upBlock && _currentTile._rightBlock && _currentTile._downBlock && _currentTile._leftBlock)
        yield break; // 모든 방향이 막혀있으면 정지
        if(_nextTile._blockOn)
        PathFind(); // 가려던 방향이 막혀있으면 다음 타일 탐색

        while(Vector3.Distance(transform.position, _nextTile.transform.position) <= 0.05f)
        {
            Rotate();
            Move();
            yield return null;
        }
        PathFind();
        _movecoroutine = StartCoroutine(Patrol());
        
    }

    void PathFind()
    {
        int[] dir = new int[]{0,1,2,3};
        Shuffle.Array(dir);
        for(int i = 0; i < 4; i++)
        {
            switch(dir[i])
            {
                case 0 :
                    if(!_currentTile._upBlock._blockOn)
                    _nextTile = _currentTile._upBlock;
                    break;
                case 1:
                    if(!_currentTile._rightBlock._blockOn)
                    _nextTile = _currentTile._upBlock;
                    break;
                case 2:
                    if(!_currentTile._downBlock._blockOn)
                    _nextTile = _currentTile._upBlock;
                    break;
                case 3:
                    if(!_currentTile._leftBlock._blockOn)
                    _nextTile = _currentTile._upBlock;
                    break;
            }
        }
    }

    void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * _moveSpeed);
    }
    void Rotate()  // 다음 위치를 바라보는 회전 로직
    {
        Quaternion targetTile = Quaternion.LookRotation(_nextTile.transform.position, transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetTile, 0.2f);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == _gridLayer)
        {
            _currentTile = other.GetComponent<GridTile>();
        }
    }
    void OnEnable()
    {
        PathFind();
        _movecoroutine = StartCoroutine(Patrol());
        GameEventBus.Subscribe<GridUpdateEvent>(NullAct);

    }
    void OnDisable()
    {
        _currentTile = null;
        _nextTile = null;
        _movecoroutine = null;
        GameEventBus.Unsubscribe<GridUpdateEvent>(NullAct);

    }
    void NullAct(GridUpdateEvent evt)
    {
        _movecoroutine = StartCoroutine(Patrol());
    }

    
}
