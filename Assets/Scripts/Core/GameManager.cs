using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }
   public GameStateMachine StateMachine { get; private set; }

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
   }

   public void InitializeGame()
   {
      StateMachine.ChangeState(new ReadyState(StateMachine));
      GameEventBus.Raise(new LoadSceneRequestedEvent(SceneType.Menu));
   }
   
   public void StartGame()
   {
      StateMachine.ChangeState(new PlayingState(StateMachine));
      GameEventBus.Raise(new LoadSceneRequestedEvent(SceneType.Game));
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
