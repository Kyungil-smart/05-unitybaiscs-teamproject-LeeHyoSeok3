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

    [SerializeField] private float BlockSpawnTime = 5f;
    public int CurrentStage { get; private set; }
    public int StageTargetScore { get; private set; }
    public bool IsPlaying { get; private set; } = false;

    public int EndStage;

    public BlockSpawner blockSpawner;
    private CooldownTimer _timer;

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
        GameEventBus.Raise(new StageStartedEvent(CurrentStage, StageTargetScore));
        IsPlaying = true;

        if (_timer == null)
            _timer = new CooldownTimer(Mathf.Max(1, BlockSpawnTime - (float)(CurrentStage * 0.2)));
        else
            _timer.Resume();
    }

    public void StopStage()
    {
        IsPlaying = false;
        _timer?.Pause();
    }

    public void OnStageCleared(StageClearedEvent evt)
    {
        blockSpawner = null;
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

    public void BlockSpawn()
    {
        if (!IsPlaying || blockSpawner == null)
        {
            blockSpawner = GameObject.FindObjectOfType<BlockSpawner>();
            return;
        }

        if (_timer.IsReady(Time.time))
        {
            blockSpawner.SpawnRandom();
        }
    }

}
