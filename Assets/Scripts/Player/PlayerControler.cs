using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControler : MonoBehaviour
{
    private PlayerInput _input;
    private PlayerMovement _movement;
    private PlayerAnimator _animator;
    private PlayerInteraction _interaction;

    public bool IsStun;
    private Coroutine _stuncoroutine;

    
    public PlayerState State {get; private set;}

    [SerializeField] private TextMeshProUGUI _stunText;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _movement = GetComponent<PlayerMovement>();
        _animator = GetComponent<PlayerAnimator>();
        State = PlayerState.Idle;
        IsStun = false;
        _interaction = GetComponent<PlayerInteraction>();

        if (_stunText != null)
            _stunText.gameObject.SetActive(false);
    }

    private void Update()
    {
        
        HandleAnimation();
        
        if (State == PlayerState.Dead ||
            !(GameManager.Instance.StateMachine.CurrnetState is PlayingState) ||
            IsStun)
        {
            _movement.SetMoveDirection(Vector3.zero);
            return;
        }
        
        HandleState();
        HandleMovement();
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
        _animator.UpdateMove(State == PlayerState.Move);
    }

    private void HandleBlock()
    {
        if (State != PlayerState.Held)
        {
            _interaction.InteractableBlockUpdate();
            // return;
        }
        
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

    public void Stunning(float duration)
    {
        if(_stuncoroutine != null)
        {
            StopCoroutine(_stuncoroutine);
            _stuncoroutine = null;
        }
        _stuncoroutine = StartCoroutine(Stun(duration));
    }
    IEnumerator Stun(float duration)
    {
        IsStun = true;

        if (_stunText != null)
            _stunText.gameObject.SetActive(true);

        float remaining = duration;

        while (remaining > 0f)
        {
            if (_stunText != null)
                _stunText.text = $"STUN : {remaining:0.0}초";

            remaining -= Time.deltaTime;
            yield return null;
        }

        if (_stunText != null)
            _stunText.gameObject.SetActive(false);

        IsStun = false;
    }
}