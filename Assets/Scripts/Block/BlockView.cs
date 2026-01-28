using System;
using UnityEngine;

public class BlockView : MonoBehaviour, IPoolable
{
    private Transform _follwTarget;
    private bool _isFollowing;
    
    public void OnSpawn()
    {
        gameObject.SetActive(true);   
    }

    public void OnDespawn()
    {
        gameObject.SetActive(false);
    }
    
    public void SetWorldPostion(Vector3 pos)
    {
        transform.position = pos;
    }

    public void AttachTo(Transform target)
    {
        _follwTarget = target;
        _isFollowing = true;
    }

    public void Detach()
    {
        _isFollowing = false;
        _follwTarget = null;
    }

    private void LateUpdate()
    {
        if (_isFollowing && _follwTarget != null)
        {
            transform.position = _follwTarget.position;
        }
    }
}