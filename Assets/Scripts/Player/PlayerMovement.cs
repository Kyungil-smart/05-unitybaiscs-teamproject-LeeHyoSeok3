using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int _moveSpeed;
    private Vector3 dir;
    private Vector3 _movePos;
    private Quaternion _rotation;

    private Animator _animator;

    [SerializeField] private Transform _playerBody;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        Init();   
    }

    private void Update()
    {
        dir.z = Input.GetAxisRaw("Vertical");
        dir.x = Input.GetAxisRaw("Horizontal");

        Rotate();
        Move();
    }

    private void Init()
    {
        dir = new Vector3();
        _movePos = new Vector3();
        _rotation = new Quaternion();
        _rigidbody = GetComponent<Rigidbody>();
        _moveSpeed = 24;
    }

    private void Move()
    {
        _movePos = transform.position + dir * (_moveSpeed * Time.deltaTime);

        _rigidbody.MovePosition(Vector3.Lerp(transform.position,
            _movePos, 0.875f));

        //transform.position = Vector3.Lerp(transform.position,
        //    _movePos, 0.875f);
    }

    private void Rotate()
    {
        if(Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.D) ||
           Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.S))
        {
            _rotation = Quaternion.LookRotation(dir);
        }
        _rigidbody.MoveRotation(Quaternion.Slerp(_playerBody.transform.rotation,
            _rotation, 0.125f));

        //_playerBody.transform.rotation = Quaternion.Slerp(_playerBody.transform.rotation,
        //    _rotation, 0.0125f);
    }

}
