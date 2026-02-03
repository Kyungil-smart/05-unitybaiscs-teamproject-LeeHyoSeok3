using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreSystem
{
    public static ScoreSystem Instance { get; private set; }

    public ScoreSystem()
    {
        Instance = this;
    }
    public int Score { get; private set; }

    private int _currentStageTargetScore;
    private int _currentStageIndex;
    private PlayerControler _playerController;

    // 테스트 코드
    public void Test()
    {
        if(Input.GetKeyDown(KeyCode.P))
            GameEventBus.Raise(new LineClearedEvent(1));
    }

    public void Subscribe()
    {
        GameEventBus.Subscribe<LineClearedEvent>(OnLineCleared);
        GameEventBus.Subscribe<StageStartedEvent>(OnStageStarted);
    }

    public void Unsubscribe()
    {
        GameEventBus.Unsubscribe<LineClearedEvent>(OnLineCleared);
        GameEventBus.Unsubscribe<StageStartedEvent>(OnStageStarted);
    }

    public void OnLineCleared(LineClearedEvent evt)
    {

        Score += (evt.Count * evt.Count) * 100;
        GameEventBus.Raise(new ScoreUpdatedEvent(Score));
        if(_playerController == null)
        {
            var playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                _playerController = playerObj.GetComponent<PlayerControler>();
            }
        }

        if (_playerController.State == PlayerState.Dead)
            return;

        if (Score >= _currentStageTargetScore)
        {
            GameEventBus.Raise(new StageClearedEvent());
        }
    }

    public void OnStageStarted(StageStartedEvent evt)
    {
        _currentStageTargetScore = evt.TargetScore;
        _currentStageIndex = evt.Stage;
        Score = 0;
        GameEventBus.Raise(new ScoreUpdatedEvent(Score));
    }
}
