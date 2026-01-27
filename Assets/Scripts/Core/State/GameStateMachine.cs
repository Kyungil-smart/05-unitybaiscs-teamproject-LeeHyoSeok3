using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine
{
    public GameState CurrnetState { get; private set; }

    public void ChangeState(GameState newState)
    {
        CurrnetState?.Exit();
        CurrnetState = newState;
        CurrnetState?.Enter();
    }
}
