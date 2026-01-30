using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingState : GameState
{
    public StartingState(GameStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Time.timeScale = 1f;
        Debug.Log("Starting ENTER");
    }

    public override void Exit()
    {
        Debug.Log("Starting EXIT");
    }
}
