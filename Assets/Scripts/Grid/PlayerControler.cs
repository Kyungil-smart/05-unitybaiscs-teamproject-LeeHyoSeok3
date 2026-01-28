using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    private PlayerInput _input;
    private PlayerMovement _movement;
    private PlayerAnimator _animator;
    
    public PlayerState State {get; private set;}

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _movement = GetComponent<PlayerMovement>();
        _animator = GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        HandleState();
        HandleAnimation();
    }

    private void HandleState()
    {
        State = _input.MoveInput == Vector3.zero ? PlayerState.Idle : PlayerState.Move;
        
        _movement.SetMoveDirection(_input.MoveInput);
    }

    private void HandleAnimation()
    {
        // _animator.UpdateMove(State == PlayerState.Move);
    }
    
    
}