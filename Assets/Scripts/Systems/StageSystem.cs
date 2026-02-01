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

    private float BlockSpawnTime = 6f;
    public int CurrentStage { get; private set; }
    public int StageTargetScore { get; private set; }
    public bool IsPlaying { get; private set; } = false;
    public int EndStage;

    public BlockSpawner blockSpawner;
    public MonsterSpawner MonsterSpawner;
    private CooldownTimer _timer;
    private CooldownTimer _obstacletimer;
    private CooldownTimer _monstertimer;
    private float spawnTime;
    private float _lastSpawnTime;
    private const float MIN_SPAWN_INTERVAL = 2f;
    private int _spawnCount = 0;

    public void Subscribe()
    {
        GameEventBus.Subscribe<StageClearedEvent>(OnStageCleared);
        GameEventBus.Subscribe<LoadSceneRequestedEvent>(DestroyPools);
    }

    public void Unsubscribe()
    {
        GameEventBus.Unsubscribe<StageClearedEvent>(OnStageCleared);
        GameEventBus.Unsubscribe<LoadSceneRequestedEvent>(DestroyPools);
    }

    public void StartStage()
    {
        GameEventBus.Raise(new StageStartedEvent(CurrentStage, StageTargetScore));
        IsPlaying = true;
        spawnTime = BlockSpawnTime - (float)(CurrentStage * 0.2);
        Debug.Log("Spawn Time: " + spawnTime);

        if (_timer == null)
            _timer = new CooldownTimer(Mathf.Max(1, spawnTime));
        else _timer.Resume();


        if (_obstacletimer == null)
            _obstacletimer = new CooldownTimer(Random.Range(spawnTime * 2, spawnTime * 4));
        else _obstacletimer.Resume();

        if (_monstertimer == null)
            _monstertimer = new CooldownTimer(Random.Range(spawnTime * 4, spawnTime * 8));
        else _monstertimer.Resume();
    }

    public void StopStage()
    {
        IsPlaying = false;
        _timer?.Pause();
        _obstacletimer?.Pause();
        _monstertimer?.Pause();
    }

    public void OnStageCleared(StageClearedEvent evt)
    {
        blockSpawner = null;
        MonsterSpawner = null;
        _timer = null;
        _obstacletimer = null;
        _monstertimer = null;
        CurrentStage++;
        _spawnCount = 0;

        if (CurrentStage > EndStage)
        {
            GameEventBus.Raise(new GameClearEvent());
            return;
        }

        StageTargetScore *= 2;

        GameEventBus.Raise(new StageStartedEvent(CurrentStage, StageTargetScore));
    }

    public void DestroyPools(LoadSceneRequestedEvent evt) => PoolManager.Instance.DestroyAllPools();
        

    public void InitializationStage()
    {
        CurrentStage = 1;
        StageTargetScore = 1000;
        EndStage = 10;
        _spawnCount = 0;
    }

    public void BlockSpawn()
    {
        if (!IsPlaying || blockSpawner == null || MonsterSpawner == null)
        {
            blockSpawner = GameObject.FindObjectOfType<BlockSpawner>();
            MonsterSpawner = GameObject.FindObjectOfType<MonsterSpawner>();
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
            _obstacletimer = new CooldownTimer(Random.Range(spawnTime * 3, spawnTime * 6));
            _lastSpawnTime = Time.time;
        }

        if (_monstertimer.IsReady(Time.time) && _spawnCount < CurrentStage)
        {
            MonsterSpawner.SpawnMonster(1);
            _monstertimer = new CooldownTimer(Random.Range(spawnTime * 3, spawnTime * 6));
            _lastSpawnTime = Time.time;
            _spawnCount++;
        }
    }

}
