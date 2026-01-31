using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Collider _collider;

    private PlayerControler _playerController;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _collider = GetComponent<CapsuleCollider>();
        _playerController = GetComponent<PlayerControler>();
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
        
        
        if(collision.transform.position.y > transform.position.y + 1)
        {
            _playerController.SetState(PlayerState.Dead);
            Debug.Log("Game Over");
        }
    }
}
