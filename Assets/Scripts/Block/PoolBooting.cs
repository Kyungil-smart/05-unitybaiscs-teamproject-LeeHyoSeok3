using System;
using UnityEngine;

public class PoolBooting : MonoBehaviour
{
    [SerializeField] private BlockView _block;
    [SerializeField] private int poolSize = 200;

    private void Awake()
    {
        PoolManager.Instance.CreatePool(_block,  poolSize);
    }
}