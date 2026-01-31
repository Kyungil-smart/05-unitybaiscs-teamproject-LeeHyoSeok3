using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControler : MonoBehaviour
{
    private PlayerInput _input;
    private PlayerMovement _movement;
    private PlayerAnimator _animator;
    private PlayerInteraction _interaction;
    
    public PlayerState State {get; private set;}

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _movement = GetComponent<PlayerMovement>();
        _animator = GetComponent<PlayerAnimator>();
        State = PlayerState.Idle;
        _interaction = GetComponent<PlayerInteraction>();
    }

    private void Update()
    {
        if (State == PlayerState.Dead || !(GameManager.Instance.StateMachine.CurrnetState is PlayingState))
        {
            _movement.SetMoveDirection(Vector3.zero);
            return;
        }
        HandleState();
        HandleMovement();
        HandleAnimation();
        HandleBlock();
    }

    private void HandleState()
    {
        State = ResolveState();
    }

    private PlayerState ResolveState()
    {
        // todo : 플레이어 dead 처리
        // if(isDead)
        //     return PlayerState.Dead;
        if (_interaction.HoldingGroup != null)
            return PlayerState.Held;
        if(_input.MoveInput != Vector3.zero)
            return PlayerState.Move;

        return PlayerState.Idle;
    }

    private void HandleMovement()
    {
        // if(State == PlayerState.Dead) 
        // {
        //     _movement.SetMoveDirection(Vector3.zero);
        //     return; 
        // }

        State = _input.MoveInput == Vector3.zero ? PlayerState.Idle : PlayerState.Move;
        _movement.SetMoveDirection(_input.MoveInput);
    }

    private void HandleAnimation()
    {
        _animator.UpdateMove(_input.MoveInput != Vector3.zero);
    }

    private void HandleBlock()
    {
        if (State != PlayerState.Held)
            _interaction.InteractableBlockUpdate();    
        
        if (_input.Interact())
        {
            if(_interaction.HoldingGroup == null)
                _interaction.PickUpBlock();
            else
            {
                _interaction.DropBlock();
            }
        }
        
        if(_input.RotateInput != null && _interaction.HoldingGroup != null)
            _interaction.RotateBlock(_input.RotateInput);
    }

    public void SetState(PlayerState state)
    {
        State = state;
    }
}