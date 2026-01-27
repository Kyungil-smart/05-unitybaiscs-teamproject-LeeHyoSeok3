using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBooting : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private PoolManager _poolManager;
    private SceneLoader _sceneLoader;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _sceneLoader = new SceneLoader();
    }

    private void Start()
    {
        _gameManager.InitializeGame();
    }
}
