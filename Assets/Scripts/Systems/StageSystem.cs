using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class StageSystem
{
    public static StageSystem Instance { get; private set; }

    public StageSystem()
    {
        Instance = this;
    }
    public int CurrentStage { get; private set; }
    public int StageTargetScore { get; private set; }
    public int EndStage;

    public void Subscribe()
    {
        GameEventBus.Subscribe<StageClearedEvent>(OnStageCleared);
    }

    public void Unsubscribe()
    {
        GameEventBus.Unsubscribe<StageClearedEvent>(OnStageCleared);
    }

    public void StartStage()
    {
        GameEventBus.Raise(
            new StageStartedEvent(CurrentStage, StageTargetScore)
        );
    }

    public void OnStageCleared(StageClearedEvent evt)
    {
        CurrentStage++;

        if (CurrentStage > EndStage)
        {
            GameEventBus.Raise(new GameClearEvent());
            return;
        }

        StageTargetScore *= 2;

        GameEventBus.Raise(new StageStartedEvent(CurrentStage, StageTargetScore));
    }

    public void InitializationStage()
    {
        CurrentStage = 1;
        StageTargetScore = 1000;
        EndStage = 10;
    }
}
