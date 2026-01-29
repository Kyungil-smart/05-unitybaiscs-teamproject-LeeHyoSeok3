using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class StageSystem : MonoBehaviour
{
    public static StageSystem Instance { get; private set; }
    public int CurrentStage { get; private set; }
    public int StageTargetScore { get; private set; }
    private int EndStage;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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

    public void StartStage()
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

    public void InitializationStage()
    {
        CurrentStage = 1;
        StageTargetScore = 1000;
    }
}
