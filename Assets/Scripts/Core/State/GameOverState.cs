using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : GameState
{
    public GameOverState(GameStateMachine stateMachine) : base(stateMachine)
    {
    }


    public override void Enter()
    {
        Time.timeScale = 0f;
        Debug.Log("GAMEOVER ENTER");
    }

    public override void Exit()
    {
    }
}
