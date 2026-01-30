using System;
using UnityEngine;

public class PlayerAnimator :  MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField]private Rigidbody _rb;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsMove = Animator.StringToHash("IsMove");

    private void Update()
    {
        // Vector3 horizontalVelocity = _rb.velocity;
        // float speed = horizontalVelocity.magnitude;
        // _animator.SetFloat(Speed, speed);
    }

    public void UpdateMove(bool isMoving)
    {
        _animator.SetBool(IsMove, isMoving);
    }
}