using System;
using UnityEngine;
using UnityEngine.UIElements;


public class PlayerInput : MonoBehaviour
{
    public Vector3 MoveInput { get; private set; }

    private void Update()
    {
        Move();
        Interact();
        RotateBlock();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        MoveInput = new Vector3(h, 0f, v).normalized;
    }

    private void Interact()
    {
        
    }
    
    private void RotateBlock()
    {
        
    }
}