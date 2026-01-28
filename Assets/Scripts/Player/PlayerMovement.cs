using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int _moveSpeed;
    private Vector3 dir;
    private float _rotateValue;
    private Animator _animator;

    private void Awake()
    {
     Init();   
    }

    private void Update()
    {
        Rotate();
        Move();
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
