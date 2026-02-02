using System;
using CartoonFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Collider _collider;

    private PlayerControler _playerController;

    [SerializeField] private CFXR_Effect _deadEffectPrefab;

    public bool _playerDead;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _collider = GetComponent<SphereCollider>();
        _playerController = GetComponentInParent<PlayerControler>();
        _playerDead = false;
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (!collision.gameObject.CompareTag("Block"))
    //         return;
    //
    //     var blockView = collision.gameObject.GetComponent<BlockView>();
    //     if (blockView == null)
    //         return;
    //
    //     var blockController = blockView.Controler;
    //     if (!(blockController.State == BlockState.Falling))
    //         return;
    //
    //     CollisionWhere(collision);
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Block"))
            return;

        var blockView = other.GetComponent<BlockView>();
        if (blockView == null)
            return;

        var blockController = blockView.Controler;
        if (blockController.State != BlockState.Falling)
            return;

        Die();
    }

    private void Die()
    {
        Vector3 effectPosition =
            transform.position + Vector3.up * 2f;

        if (_deadEffectPrefab != null)
            Instantiate(_deadEffectPrefab, effectPosition, Quaternion.identity);

        _playerController.SetState(PlayerState.Dead);
        _playerDead = true;
    }
}