using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    private bool _isLoading;

    public SceneLoader()
    {
        GameEventBus.Subscribe<LoadSceneRequestedEvent>(OnLoadSceneRequested);
    }

    private void OnLoadSceneRequested(LoadSceneRequestedEvent e)
    {
        if (_isLoading)
            return;

        _isLoading = true;
        
        GameEventBus.Raise(new SceneUnloadEvent());

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(e.SceneType.ToString());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        _isLoading = false;

        GameEventBus.Raise(new SceneLoadedEvent());
    }

    public void Dispose()
    {
        GameEventBus.Unsubscribe<LoadSceneRequestedEvent>(OnLoadSceneRequested);
    }
}
