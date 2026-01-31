using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }
   public GameStateMachine StateMachine { get; private set; }

   public ScoreSystem ScoreSystem { get; private set; }
   public StageSystem StageSystem { get; private set; }

    private void Awake()
    {
      if (Instance != null && Instance != this)
      {
         Destroy(gameObject);
         return;
      }

      Instance = this;
      DontDestroyOnLoad(gameObject);
      
      StateMachine = new GameStateMachine();
      ScoreSystem = new ScoreSystem();
      StageSystem = new StageSystem();
    }

    private void OnEnable()
    {
        ScoreSystem.Subscribe();
        StageSystem.Subscribe();
    }

    private void Update()
    {
        ScoreSystem.Test();
    }

    private void OnDisable()
    {
        ScoreSystem.Unsubscribe();
        StageSystem.Unsubscribe();
    }

    public void InitializeGame()
    {
       StateMachine.ChangeState(new ReadyState(StateMachine));
       GameEventBus.Raise(new LoadSceneRequestedEvent(SceneType.Menu));
    }

    public void ReadyState()
    {
       StateMachine.ChangeState(new ReadyState(StateMachine));
    }
   
    public void StartGame()
    {
       StateMachine.ChangeState(new PlayingState(StateMachine));
    }

    public void PauseGame()
    {
       StateMachine.ChangeState(new PausedState(StateMachine));
    }

    public void GameOver()
    {
       StateMachine.ChangeState(new GameOverState(StateMachine));
    }
}
