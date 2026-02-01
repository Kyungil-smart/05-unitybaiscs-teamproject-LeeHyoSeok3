using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol_Monster : MonoBehaviour
{
    [SerializeField][Range(0,20)] float _moveSpeed;
    
    Rigidbody rb;
    Coroutine _movecoroutine;
    [SerializeField] LayerMask _blockLayer;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    
    IEnumerator WanderRoutine()  // 배회 루틴
    {
        while (true)
        {
            // 정지 및 대기
            rb.velocity = Vector3.zero;
            yield return YieldContainer.WaitForSeconds(1f);
            
            // 목표 방향 
            
            Vector3 targetDir = GetRandomDirection();
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);

            
            // 회전 루프
            while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
                yield return null;
            }
            
            // 일정 시간 혹은 블럭 감지할때까지 이동
            float moveTime = Random.Range(1f, 4f);
            float timer = 0f;
            
            while (timer < moveTime)
            {
                if (IsObstacleInFront()) break;

                rb.velocity = new Vector3(transform.forward.x * _moveSpeed, rb.velocity.y, transform.forward.z * _moveSpeed);

                timer += Time.deltaTime;
                yield return null;
            }
        }
    }

    Vector3 GetRandomDirection()
    {
        int rand = Random.Range(0, 4);
        switch (rand)
        {
            case 0: 
                return transform.forward; 
            case 1: 
                return -transform.forward; 
            case 2: 
                return transform.right; 
            case 3: 
                return -transform.right;
        }
        return transform.forward; 
        
    }

    bool IsObstacleInFront()
    {
        // 레이캐스트로 앞쪽 장애물 감지
        return Physics.Raycast(transform.position + Vector3.up * 1.2f, transform.forward, 0.5f, _blockLayer);
    }

    void OnEnable()
    {
        _movecoroutine = StartCoroutine(WanderRoutine());
    }
    void OnDisable()
    {
        if(_movecoroutine != null) StopCoroutine(_movecoroutine);
        _movecoroutine = null;

    }
    void OnDrawGizmos()
    {
        Vector3 startPos = transform.position + Vector3.up * 1.2f;
        Vector3 rayDirection = transform.forward * 0.5f;
        Gizmos.DrawRay(startPos, rayDirection);
    }
}
