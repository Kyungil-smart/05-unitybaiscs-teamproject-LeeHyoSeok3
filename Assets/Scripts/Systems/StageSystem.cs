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

    private float BlockSpawnTime = 10f;
    public int CurrentStage { get; private set; }
    public int StageTargetScore { get; private set; }
    public bool IsPlaying { get; private set; } = false;

    public int EndStage;

    public BlockSpawner blockSpawner;
    private CooldownTimer _timer;
    private CooldownTimer _obstacletimer;
    private float spawnTime;
    private float _lastSpawnTime;
    private const float MIN_SPAWN_INTERVAL = 2f;

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
        spawnTime = BlockSpawnTime - (float)(CurrentStage * 0.2);

        if (_timer == null)
            _timer = new CooldownTimer(Mathf.Max(1, spawnTime));
        else _timer.Resume();


        if (_obstacletimer == null)
            _obstacletimer = new CooldownTimer(Random.Range(spawnTime * 2, spawnTime * 4));
        else _obstacletimer.Resume();
    }

    public void StopStage()
    {
        IsPlaying = false;
        _timer?.Pause();
        _obstacletimer?.Pause();
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

        if (Time.time - _lastSpawnTime < MIN_SPAWN_INTERVAL)
            return;

        if (_timer.IsReady(Time.time))
        {
            blockSpawner.SpawnRandom();
            _lastSpawnTime = Time.time;
            return;
        }

        if (_obstacletimer.IsReady(Time.time))
        {
            blockSpawner.SpawnObstacle(CurrentStage);
            _obstacletimer = new CooldownTimer(Random.Range(spawnTime * 2, spawnTime * 4));
            _lastSpawnTime = Time.time;
        }
    }

}
