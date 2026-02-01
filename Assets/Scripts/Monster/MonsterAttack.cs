using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    GameObject player;

    [Header("감속")]
    [SerializeField] float _decelerationValue; // 속도 감소치
    [SerializeField] float _duration; // 감속 지속시간
    [SerializeField] private float _decelerationPerTime; // 감속 보간치
    
    [Header("기절")]
    [SerializeField][Range(1,20)] float _explosionRange; // 폭발 사거리
    [SerializeField][Range(0,10)] float _stunDuration;  // 기절 지속시간

    private void Awake()
    {
        _decelerationValue = 2f;
        _duration = 2f;
        _decelerationPerTime = 0.2f;
    }
    void OnEnable()
    {
        player = GameObject.Find("Player");
    }
    
    public void SlowPlayer()
    {
        // 플래이어 상태를 변화시키는 매서드
        player.GetComponent<PlayerMovement>().Slow(_decelerationValue , _duration, _decelerationPerTime);
        // 퇴장
        gameObject.GetComponent<MonsterView>().Controller.Release();
    }

    public void Explosion() // 블럭 충돌 조건에서 발생할 폭발 메서드
    {        
        Debug.Log("펑!");
        // 이펙트 출력


        // 플레이어 거리 계산
        if(Vector3.Distance(transform.position, player.transform.position) <= _explosionRange)
        {
            player.GetComponent<PlayerControler>().SetState(PlayerState.Idle);
            player.GetComponent<PlayerControler>().Stunning(_stunDuration);
        }        
    }
}
