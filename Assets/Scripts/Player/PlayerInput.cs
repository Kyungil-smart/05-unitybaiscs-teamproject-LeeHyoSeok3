using System;
using UnityEngine;
using UnityEngine.UIElements;


public class PlayerInput : MonoBehaviour
{
    public Vector3 MoveInput { get; private set; }
    public float? RotateInput { get; private set; }

    private void Update()
    {
        Move();
        Interact();
        Rotate();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        MoveInput = new Vector3(h, 0f, v).normalized;
    }

    public bool Interact()
    {
        return Input.GetKeyDown(KeyCode.F);
    }
    
    public void Rotate()
    {
        RotateInput = null;
        
        if (Input.GetKeyDown(KeyCode.E))
            RotateInput = 1f;
        else if (Input.GetKeyDown(KeyCode.Q))
            RotateInput = -1f;
    }
}