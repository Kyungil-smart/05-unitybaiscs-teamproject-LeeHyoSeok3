using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUpdatedEvent : IGameEvent
{
    public int Score { get; }
    public ScoreUpdatedEvent(int score) => Score = score;
}
