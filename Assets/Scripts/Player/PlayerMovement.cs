using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement
{
    private int _moveSpeed;
    private int _rotateSpeed;
    
    // private Vector3 dir;
    // private float _rotateValue;
    // private Animator _animator;
    // private Rigidbody _rb;

    public PlayerMovement(int  moveSpeed, int rotateSpeed)
    {
        _moveSpeed = moveSpeed;
        _rotateSpeed = rotateSpeed;
    }
    
    
    
    // private void Awake()
    // {
    //     Init();
    //     _rb = GetComponent<Rigidbody>();
    // }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(h, 0f, v).normalized;
        Vector3 targetPos = _rb.position + moveDir * (_moveSpeed * Time.fixedDeltaTime);

        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRot, _rotateSpeed * Time.fixedDeltaTime));
        }
        
        _rb.MovePosition(targetPos);
    }

    private void Update()
    {
        // Rotate();
        // Move();
    }

    private void Init()
    {
        _rotateValue = 0;
    }

    private void Move()
    {
        dir.z = Input.GetAxisRaw("Vertical");
        dir.x = Input.GetAxisRaw("Horizontal");

        //transform.Translate(Vector3.forward * _moveSpeed * dir.z * Time.deltaTime);
        if (Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);
        }
    }

    private void Rotate()
    {
        _rotateValue += dir.x;
        //transform.rotation = Quaternion.LookRotation(dir);

        //transform.rotation = Quaternion.Slerp(transform.rotation,
        //    Quaternion.Euler(0, _rotateValue, 0), 0.5f);
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(dir), 0.01f);


        //transform.Rotate(Vector3.up, 100f*dir.x * Time.deltaTime);
    }
}