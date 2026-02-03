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

    // private float BlockSpawnTime = 6f;
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
    private int _monsterSpawnCount = 0;
    private int _obstacleSpawnCount = 0;

    public void Subscribe()
    {
        GameEventBus.Subscribe<StageClearedEvent>(OnStageCleared);
        GameEventBus.Subscribe<LoadSceneRequestedEvent>(DestroyPools);
        GameEventBus.Subscribe<LoadSceneRequestedEvent>(OnStageRestart);
    }

    public void Unsubscribe()
    {
        GameEventBus.Unsubscribe<StageClearedEvent>(OnStageCleared);
        GameEventBus.Unsubscribe<LoadSceneRequestedEvent>(DestroyPools);
        GameEventBus.Unsubscribe<LoadSceneRequestedEvent>(OnStageRestart);
    }

    public void StartStage()
    {
        GameEventBus.Raise(new StageStartedEvent(CurrentStage, StageTargetScore));
        IsPlaying = true;
        // spawnTime = BlockSpawnTime - (float)(CurrentStage * 0.5);

        spawnTime = CurrentStage switch
        {
            1 => 5f,
            2 => 4f,
            3 => 3f,
            4 => 2f,
            _ => 5f
        };
        
        
        if (_timer == null)
            _timer = new CooldownTimer(Mathf.Max(1, spawnTime));

        if (_obstacletimer == null)
            _obstacletimer = new CooldownTimer(20); // 20초마다 장애물 스폰 시도

        if (_monstertimer == null)
            _monstertimer = new CooldownTimer(20); // 20초마다 몬스터 스폰 시도

        _lastSpawnTime = Time.time;
    }

    public void ResumeStage()
    {
        IsPlaying = true;
        _timer?.Resume();
        _obstacletimer?.Resume();
        _monstertimer?.Resume();
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
        _monsterSpawnCount = 0;
        _obstacleSpawnCount = 0;

        if (CurrentStage > EndStage)
        {
            GameEventBus.Raise(new GameClearEvent());
            return;
        }

        StageTargetScore += 1000;

        GameEventBus.Raise(new StageStartedEvent(CurrentStage, StageTargetScore));
    }

    public void OnStageRestart(LoadSceneRequestedEvent evt)
    {
        blockSpawner = null;
        MonsterSpawner = null;
        _timer = null;
        _obstacletimer = null;
        _monstertimer = null;
        _monsterSpawnCount = 0;
        _obstacleSpawnCount = 0;

        GameEventBus.Raise(new StageStartedEvent(CurrentStage, StageTargetScore));
    }

    public void DestroyPools(LoadSceneRequestedEvent evt) => PoolManager.Instance.DestroyAllPools();
        

    public void InitializationStage()
    {
        CurrentStage = 1;
        StageTargetScore = 1000;
        EndStage = 4;
        _monsterSpawnCount = 0;
        _obstacleSpawnCount = 0;
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

        float now = Time.time;

        //  bool tryObstacle = _obstacleSpawnCount < CurrentStage; // 장애물 스폰 제한 걸려면 주석 해제 후 값 교체
        bool tryObstacle = true;
        bool tryMonster = _monsterSpawnCount < CurrentStage * 2;
        bool tryBlock = true;

        // 우선순위: 몬스터 > 장애물 > 블록 (시간이 겹칠 경우)
        if (tryMonster && _monstertimer.IsReady(now)) // 1순위
        {
            MonsterSpawner.SpawnMonster(1);
            _monsterSpawnCount++;
        }
        else if (tryObstacle && _obstacletimer.IsReady(now)) // 2순위
        {
            blockSpawner.SpawnObstacle(CurrentStage);
            _obstacleSpawnCount++;
        }
        else if (tryBlock && _timer.IsReady(now)) // 3순위
        {
            blockSpawner.SpawnRandom();
        }
        else
        {
            return;
        }

        _lastSpawnTime = now;
    }

    public void TestSpawn()
    {
        MonsterSpawner.SpawnMonster(1);
    }

}
