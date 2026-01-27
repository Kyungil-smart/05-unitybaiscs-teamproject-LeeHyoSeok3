using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingState : GameState
{
    public PlayingState(GameStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Time.timeScale = 1f;
        Debug.Log("PLAYING ENTER");
    }

    public override void Exit()
    {
        Debug.Log("PLAYING EXIT");
    }
}
