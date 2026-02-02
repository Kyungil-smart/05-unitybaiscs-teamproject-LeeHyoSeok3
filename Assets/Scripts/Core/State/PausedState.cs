using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedState : GameState
{
    // Start is called before the first frame update
    public PausedState(GameStateMachine stateMachine) : base(stateMachine)
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
