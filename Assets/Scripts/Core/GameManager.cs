using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameStateMachine StateMachine { get; private set; }

    public ScoreSystem ScoreSystem { get; private set; }
    public StageSystem StageSystem { get; private set; }

    [SerializeField] private float BlockSpawnTime = 5;

    private BlockSpawner blockSpawner;
    private float blockTimer = 0f;
    private Coroutine blockRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        StateMachine = new GameStateMachine();
        ScoreSystem = new ScoreSystem();
        StageSystem = new StageSystem();
    }

    private void OnEnable()
    {
        ScoreSystem.Subscribe();
        StageSystem.Subscribe();
    }


    private void OnDisable()
    {
        ScoreSystem.Unsubscribe();
        StageSystem.Unsubscribe();
    }

    public void InitializeGame()
    {
        StateMachine.ChangeState(new ReadyState(StateMachine));
        GameEventBus.Raise(new LoadSceneRequestedEvent(SceneType.Menu));
    }

    public void ReadyState()
    {
        StateMachine.ChangeState(new ReadyState(StateMachine));
    }

    public void StartGame()
    {
        StateMachine.ChangeState(new StartingState(StateMachine));
        StopCoroutine(BlockRoutine());
    }

    public void PlayGame()
    {
        StateMachine.ChangeState(new PlayingState(StateMachine));
        if (blockRoutine == null)
        {
            blockSpawner = FindObjectOfType<BlockSpawner>();
            blockRoutine = StartCoroutine(BlockRoutine());
        }
    }

    public void PauseGame()
    {
        StateMachine.ChangeState(new PausedState(StateMachine));

        if (blockRoutine != null)
        {
            StopCoroutine(blockRoutine);
            blockRoutine = null;
        }
    }

    public void GameOver()
    {
        StateMachine.ChangeState(new GameOverState(StateMachine));
        StopCoroutine(BlockRoutine());
    }

    IEnumerator BlockRoutine()
    {
        while (true)
        {
            blockTimer += Time.deltaTime;
            float blockSpawnInterval = Mathf.Max(1, BlockSpawnTime - (float)(StageSystem.CurrentStage * 0.5));

            if(blockTimer >= blockSpawnInterval)
            {
                blockSpawner.SpawnRandom();
                blockTimer = 0f;
            }
            Debug.Log($"Block Spawn Interval: {blockSpawnInterval}, Timer: {blockTimer}");
            yield return null;
        }
    }
}
