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
    }

    public override void Exit()
    {
    }
}
