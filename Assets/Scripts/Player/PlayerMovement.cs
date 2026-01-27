using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int _moveSpeed;
    private Vector3 dir;

    private Animator _animator;

    private void Update()
    {
        Rotate();
        Move();
    }

    private void Move()
    {
        dir.z = Input.GetAxisRaw("Vertical");
        dir.x = Input.GetAxisRaw("Horizontal");
        
        transform.Translate(transform.forward * _moveSpeed * dir.z * Time.deltaTime);
        transform.Translate(transform.right * _moveSpeed * -(dir.x) * Time.deltaTime);
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.LookRotation(dir);
        
        //transform.Rotate(Vector3.up, 100f*dir.x * Time.deltaTime);
    }

}
