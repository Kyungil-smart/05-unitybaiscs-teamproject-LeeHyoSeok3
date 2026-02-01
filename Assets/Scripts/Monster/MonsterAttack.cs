using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    GameObject player;

    [SerializeField] float _decelerationValue; // 속도 감소치
    [SerializeField] float _duration; // 감속 지속시간
    [SerializeField] private float _decelerationPerTime; // 감속 보간치
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
}
