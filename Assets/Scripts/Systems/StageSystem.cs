using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class StageSystem : MonoBehaviour
{
    public int CurrentStage { get; private set; }
    public int StageTargetScore { get; private set; }
    public int EndStage { get; private set; }
    private void Awake()
    {
        CurrentStage = 1;
        StageTargetScore = 1000;
        EndStage = 10;
    }

    private void OnEnable()
    {
        GameEventBus.Subscribe<StageClearedEvent>(OnStageCleared);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe<StageClearedEvent>(OnStageCleared);
    }

    public void StartFirstStage()
    {
        GameEventBus.Raise(
            new StageStartedEvent(CurrentStage, StageTargetScore)
        );
    }

    private void OnStageCleared(StageClearedEvent evt)
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
}
