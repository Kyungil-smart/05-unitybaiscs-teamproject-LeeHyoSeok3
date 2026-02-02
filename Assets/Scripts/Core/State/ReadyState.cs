using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyState : GameState
{
    public ReadyState(GameStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Time.timeScale = 0f;
    }

    public override void Exit()
    {
        Time.timeScale = 1f;
    }
}
