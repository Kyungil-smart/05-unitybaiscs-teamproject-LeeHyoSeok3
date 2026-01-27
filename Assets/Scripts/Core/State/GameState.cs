using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState
{
    protected GameStateMachine _stateMachine;

    protected GameState(GameStateMachine stateMachine)
    {
        this._stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }
}
