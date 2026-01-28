using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalScore;
    [SerializeField] private TextMeshProUGUI _IncreaseScore;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _readyState;
    [SerializeField] private TextMeshProUGUI _pauseState;
    [SerializeField] private TextMeshProUGUI _pauseKey;
    [SerializeField] private Image _darkOverlay;

    private ScoreUpdatedEvent _scoreUpdatedEvent;
    void Start()
    {
        ReadyState();
    }

    void Update()
    {
        TextUpdate();

        if (GameManager.Instance.StateMachine.CurrnetState is ReadyState
            && Input.GetKeyDown(KeyCode.Space))
        {
            PlayingState();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.StateMachine.CurrnetState is PlayingState)
            {
                PauseState();
            }
            else if (GameManager.Instance.StateMachine.CurrnetState is PausedState)
            {
                PlayingState();
            }
        }
    }

    void ReadyState()
    {
        _IncreaseScore.gameObject.SetActive(false);
        _totalScore.gameObject.SetActive(false);
        _level.gameObject.SetActive(false);
        _pauseState.gameObject.SetActive(false);
        _pauseKey.gameObject.SetActive(false);
        _readyState.gameObject.SetActive(true);
        _darkOverlay.gameObject.SetActive(true);

        _scoreUpdatedEvent = new ScoreUpdatedEvent(0);
    }

    void PlayingState()
    {
        _darkOverlay.gameObject.SetActive(false);
        _readyState.gameObject.SetActive(false);
        _pauseState.gameObject.SetActive(false);
        _pauseKey.gameObject.SetActive(true);
        _totalScore.gameObject.SetActive(true);
        _level.gameObject.SetActive(true);
        GameManager.Instance.StartGame();
    }

    void PauseState()
    {
        _darkOverlay.gameObject.SetActive(true);
        _pauseState.gameObject.SetActive(true);
        GameManager.Instance.PauseGame();
    }

    void TextUpdate()
    {
        _totalScore.text = $"목표 점수 : {_scoreUpdatedEvent.Score} 점\n" +
            $"현재 점수 : {_scoreUpdatedEvent.Score} 점";
        _readyState.text = $"게임을 시작하시려면 [SpaceBar] 키를 눌러주세요!\n" +
            $"현재 스테이지 : {_scoreUpdatedEvent.Score} Stage\n" +
            $"목표 점수 : {_scoreUpdatedEvent.Score} 점";
        _level.text = $"{_scoreUpdatedEvent.Score} Stage";
        _pauseState.text = $"게임이 일시정지 되었습니다.\n" +
            $"계속하시려면 [Esc] 키를 눌러주세요!";
        _IncreaseScore.text = $"+ {_scoreUpdatedEvent.Score}";
    }
}
