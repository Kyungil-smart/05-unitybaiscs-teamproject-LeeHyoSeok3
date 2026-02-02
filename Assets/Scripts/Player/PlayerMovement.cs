using CartoonFX;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField][Range(1, 20)] private float _moveSpeed;
    [SerializeField][Range(1, 100)] private float _rotateSpeed;
    [SerializeField] private CFXR_Effect _slowEffectPrefab;
    [SerializeField] private TextMeshProUGUI _slowText;


    private Coroutine coroutine;
    private Rigidbody _rb;
    private Vector3 _moveDir;
    private float _initialSpeed;
    private bool _isSlowed = false;



    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _initialSpeed = _moveSpeed;

        if (_slowText != null)
            _slowText.gameObject.SetActive(false);
    }

    public void SetMoveDirection(Vector3 dir)
    {
        _moveDir = dir;
    }

    private void FixedUpdate()
    {
        if (_moveDir == Vector3.zero)
        {
            _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f);
            return;
        }

        Move();
        Rotate();
    }

    private void Move()
    {
        Vector3 velocity = _moveDir * _moveSpeed;
        velocity.y = _rb.velocity.y;

        _rb.velocity = velocity;
    }

    private void Rotate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_moveDir);
        Quaternion smoothRotation = Quaternion.Slerp(_rb.rotation, targetRotation, _rotateSpeed * Time.fixedDeltaTime);

        _rb.MoveRotation(smoothRotation);
    }


    public void Slow(float speed, float time, float dece) // 외부에서 접근하는 속도 감소 코루틴
    {
        if (_isSlowed) return;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(SpeedChangeCoroutine(speed, time, dece));
        coroutine = null;
    }
    private IEnumerator SpeedChangeCoroutine(float speed, float duration, float decPerSecond)
    {
        if (_slowText != null)
            _slowText.gameObject.SetActive(true);

        _isSlowed = true;

        // 공격당한 이펙트 재생
        if (_slowEffectPrefab != null)
        {
            Instantiate(
             _slowEffectPrefab,
             transform.position,
             Quaternion.Euler(-90f, 0f, 0f)
             );
        }

        float elapsed = 0f;
        float decreaseTime = speed / decPerSecond;  // 속도 감소 시간
        float increaseTime = decreaseTime;          // 속도 회복 시간
        float totalTime = decreaseTime + duration + increaseTime;

        // 1️. 속도 감소
        while (_moveSpeed > _initialSpeed - speed)
        {
            _moveSpeed = Mathf.MoveTowards(_moveSpeed, _initialSpeed - speed, decPerSecond * Time.deltaTime);
            elapsed += Time.deltaTime;

            if (_slowText != null)
                _slowText.text = $"SLOW : {Mathf.Clamp(totalTime - elapsed, 0f, totalTime):0.0}초";

            yield return null;
        }

        // 2️. 지속 시간 유지
        float sustainElapsed = 0f;
        while (sustainElapsed < duration)
        {
            float dt = Time.deltaTime;
            sustainElapsed += dt;
            elapsed += dt;

            if (_slowText != null)
                _slowText.text = $"SLOW : {Mathf.Clamp(totalTime - elapsed, 0f, totalTime):0.0}초";

            yield return null;
        }

        // 3️. 속도 회복
        while (_moveSpeed < _initialSpeed)
        {
            _moveSpeed = Mathf.MoveTowards(_moveSpeed, _initialSpeed, decPerSecond * Time.deltaTime);
            elapsed += Time.deltaTime;

            if (_slowText != null)
                _slowText.text = $"SLOW : {Mathf.Clamp(totalTime - elapsed, 0f, totalTime):0.0}초";

            yield return null;
        }

        _moveSpeed = _initialSpeed;

        if (_slowText != null)
            _slowText.gameObject.SetActive(false);

        _isSlowed = false;
    }

}
