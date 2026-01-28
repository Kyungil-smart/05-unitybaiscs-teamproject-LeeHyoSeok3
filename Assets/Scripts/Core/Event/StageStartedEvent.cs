using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStartedEvent : IGameEvent
{
    public int Stage { get; }
    public int TargetScore { get; }
    public StageStartedEvent(int stage, int targetScore) 
    {   
        Stage = stage;
        TargetScore = targetScore;
    }
}
