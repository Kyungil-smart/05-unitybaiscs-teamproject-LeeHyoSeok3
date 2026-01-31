using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField][Range(1,20)] private float _moveSpeed;
    [SerializeField][Range(1,100)] private float _rotateSpeed;
    
    public Vector3 LastMoveDelta {get; private set;}
    
    private Coroutine coroutine;
    private Rigidbody _rb;
    private Vector3 _moveDir;
    private float _initialSpeed;
    
    
    public float MoveAmount { get; private set; }
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _initialSpeed = _moveSpeed;
    }

    public void SetMoveDirection(Vector3 dir)
    {
        _moveDir = dir;
    }

    private void FixedUpdate()
    {
        if (_moveDir == Vector3.zero)
        {
            MoveAmount = 0f;
            LastMoveDelta = Vector3.zero;
            return;
        }
        
        Move();
        Rotate();
    }

    private void Move()
    {
        Vector3 nextPos = GetNextPos();
        LastMoveDelta = nextPos - _rb.position;
        _rb.MovePosition(nextPos);
    }

    private void Rotate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_moveDir);
        Quaternion smoothRotation = Quaternion.Slerp(_rb.rotation, targetRotation, _rotateSpeed * Time.fixedDeltaTime);
        
        _rb.MoveRotation(smoothRotation);
    }

    private Vector3 GetNextPos()
    {
        Vector3 movement = _moveDir * (_moveSpeed * Time.fixedDeltaTime);

        MoveAmount = movement.magnitude / Time.fixedDeltaTime;

        return _rb.position + movement;
    }
    
    public void Slow(float speed, float time, float dece) // 외부에서 접근하는 속도 감소 코루틴
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(SpeedChangeCoroutine(speed, time, dece));
        coroutine = null;
    }
    private IEnumerator SpeedChangeCoroutine(float speed, float time, float dece)
    {
        
        // 일정시간 단계적 감소
        while(_moveSpeed > _initialSpeed - speed)
        {
            _moveSpeed -= dece * Time.deltaTime;
            yield return null;
        } 
        // 지속시간 동안 속도 유지
        yield return YieldContainer.WaitForSeconds(time);

        
        // 단계적 속도 회복
        while(_moveSpeed < _initialSpeed)
        {
            _moveSpeed += dece * Time.deltaTime;
            yield return null;
        }
        _moveSpeed = _initialSpeed;
    }
}
