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
        // tag�� �ڽ�������Ʈ ����
        if(collision.gameObject.CompareTag("Block"))
        {
            CollisionWhere(collision);
        }
    }

    private void CollisionWhere(Collision collision)
    {
        
        
        if(collision.transform.position.y > transform.position.y)
        {
            _playerController.SetState(PlayerState.Dead);
            _playerDead = true;
            Debug.Log("Game Over");
        }
    }
}
