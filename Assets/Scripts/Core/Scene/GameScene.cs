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
    [SerializeField] private StageSystem _stageSystem;
    [SerializeField] private Button _retrunTitle;
    [SerializeField] private GameObject _gameExit;

    private int _currentScore;
    private int _currentStage;
    private int _targetScore;

    void Awake()
    { 
        _currentStage = 1;
        _targetScore = 1000;
    }
    void Start()
    {
        ReadyState();
    }

    private void OnEnable()
    {
        GameEventBus.Subscribe<ScoreUpdatedEvent>(OnScoreUpdated);
        GameEventBus.Subscribe<StageStartedEvent>(OnStageStarted);
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
                ResumeGame();
            }
        }
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe<ScoreUpdatedEvent>(OnScoreUpdated);
        GameEventBus.Unsubscribe<StageStartedEvent>(OnStageStarted);
    }

    private void OnScoreUpdated(ScoreUpdatedEvent evt)
    {
        _currentScore = evt.Score;
    }

    private void OnStageStarted(StageStartedEvent evt)
    {
        _currentStage = evt.Stage;
        _targetScore = evt.TargetScore;
    }

    public void ReadyState()
    {
        _IncreaseScore.gameObject.SetActive(false);
        _totalScore.gameObject.SetActive(false);
        _level.gameObject.SetActive(false);
        _pauseState.gameObject.SetActive(false);
        _pauseKey.gameObject.SetActive(false);
        _retrunTitle.gameObject.SetActive(false);
        _gameExit.gameObject.SetActive(false);
        _readyState.gameObject.SetActive(true);
        _darkOverlay.gameObject.SetActive(true);
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
        _stageSystem.StartFirstStage();
        
    }
    void ResumeGame()
    {
        _darkOverlay.gameObject.SetActive(false);
        _pauseState.gameObject.SetActive(false);
        _retrunTitle.gameObject.SetActive(false);
        GameManager.Instance.StartGame();
    }

    public void PauseState()
    {
        _gameExit.gameObject.SetActive(false);
        _darkOverlay.gameObject.SetActive(true);
        _pauseState.gameObject.SetActive(true);
        _retrunTitle.gameObject.SetActive(true);
        GameManager.Instance.PauseGame();
    }

    void TextUpdate()
    {
        _totalScore.text = $"목표 점수 : {_targetScore} 점\n" +
            $"현재 점수 : {_currentScore} 점";
        _readyState.text = $"게임을 시작하시려면 [SpaceBar] 키를 눌러주세요!\n" +
            $"현재 스테이지 : {_currentStage} Stage\n" +
            $"목표 점수 : {_targetScore} 점";
        _level.text = $"{_currentStage} Stage";
        _pauseState.text = $"게임이 일시정지 되었습니다.\n" +
            $"계속하시려면 [Esc] 키를 눌러주세요!";
        _IncreaseScore.text = "+ 100";
    }

    public void ReturnTitle()
    {
        GameManager.Instance.InitializeGame();
    }

    public void AreYouSure()
    {
        _gameExit.gameObject.SetActive(true);
    }
}
