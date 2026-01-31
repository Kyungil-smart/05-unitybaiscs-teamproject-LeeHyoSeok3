using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Collider _collider;

    private PlayerControler _playerController;

    public bool _playerDead;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _collider = GetComponent<CapsuleCollider>();
        _playerController = GetComponent<PlayerControler>();
        _playerDead = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Block"))
            return;

        var blockView = collision.gameObject.GetComponent<BlockView>();
        if (blockView == null)
            return;

        var blockController = blockView.Controler;
        if (!(blockController.State == BlockState.Falling))
            return;

        CollisionWhere(collision);
    }

    private void CollisionWhere(Collision collision)
    {             
        if (collision.transform.position.y > transform.position.y + 1)
        {
            _playerController.SetState(PlayerState.Dead);
            _playerDead = true;
            Debug.Log("Game Over");
        }
    }
}
