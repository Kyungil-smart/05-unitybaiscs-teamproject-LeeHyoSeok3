using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField][Range(1,20)] private float _moveSpeed;
    [SerializeField][Range(1,100)] private float _rotateSpeed;
    
    private Rigidbody _rb;
    private Vector3 _moveDir;
    public float MoveAmount { get; private set; }
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
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
            return;
        }
        
        Move();
        Rotate();
    }

    private void Move()
    {
        Vector3 movement = _moveDir * (_moveSpeed * Time.fixedDeltaTime);
        _rb.MovePosition(_rb.position + movement);
        
        MoveAmount = movement.magnitude / Time.fixedDeltaTime;
    }

    private void Rotate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_moveDir);
        Quaternion smoothRotation = Quaternion.Slerp(_rb.rotation, targetRotation, _rotateSpeed * Time.fixedDeltaTime);
        
        _rb.MoveRotation(smoothRotation);
    }
}
