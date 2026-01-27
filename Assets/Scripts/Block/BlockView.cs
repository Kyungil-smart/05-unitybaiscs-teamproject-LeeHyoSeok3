using System;
using UnityEngine;

public class BlockView : MonoBehaviour, IPoolable
{
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
}