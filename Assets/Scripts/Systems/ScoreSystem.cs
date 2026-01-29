using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public int Score { get; private set; }

    private int _currentStageTargetScore;

    GameScene _gameScene;

    void Start()
    {
        _gameScene = FindAnyObjectByType<GameScene>();
    }

    // 테스트 코드
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            GameEventBus.Raise(new LineClearedEvent(1));
    }

    private void OnEnable()
    {
        GameEventBus.Subscribe<LineClearedEvent>(OnLineCleared);
        GameEventBus.Subscribe<StageStartedEvent>(OnStageStarted);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe<LineClearedEvent>(OnLineCleared);
        GameEventBus.Unsubscribe<StageStartedEvent>(OnStageStarted);
    }

    private void OnLineCleared(LineClearedEvent evt)
    {
        Score += (evt.Count * evt.Count) * 100;
        Debug.Log($"Score : {Score}");
        GameEventBus.Raise(new ScoreUpdatedEvent(Score));

        if (Score >= _currentStageTargetScore)
        {
            GameEventBus.Raise(new StageClearedEvent());
            GameManager.Instance.StateMachine.ChangeState(new ReadyState(GameManager.Instance.StateMachine));
            if (_gameScene != null)
                _gameScene.ReadyState();
        }
    }

    private void OnStageStarted(StageStartedEvent evt)
    {
        _currentStageTargetScore = evt.TargetScore;
        Score = 0;
        GameEventBus.Raise(new ScoreUpdatedEvent(Score));
    }
}
