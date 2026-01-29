using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private Button _retrunTitle;
    [SerializeField] private GameObject _gameExit;
    [Tooltip("Camera")]
    public Camera mainCamera;
    [Tooltip("CameraMoveSpeed")]
    public float moveDuration;
    [Tooltip("FirstCameraMove")]
    public GameObject firstCameraMove;
    [Tooltip("SecondCameraMove")]
    public GameObject secondCameraMove;
    [Tooltip("ThirdCameraMove")]
    public GameObject thirdCameraMove;
    [Tooltip("FourthCameraMove")]
    public GameObject fourthCameraMove;
    [Tooltip("RemoveSPSet")]
    public GameObject removeSPSet;
    private int _currentScore;
    private int _currentStage;
    private int _targetScore;

    void Awake()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }
    void Start()
    {
        _currentStage = StageSystem.Instance.CurrentStage;
        _targetScore = StageSystem.Instance.StageTargetScore;
        StartCoroutine(CameraMoveThenStartGame());
    }

    private void OnEnable()
    {
        GameEventBus.Subscribe<ScoreUpdatedEvent>(OnScoreUpdated);
        GameEventBus.Subscribe<StageStartedEvent>(OnStageStarted);
        GameEventBus.Subscribe<StageClearedEvent>(StageCleared);
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
        GameEventBus.Unsubscribe<StageClearedEvent>(StageCleared);
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
        Debug.Log("ReadyState CALLED");
        _IncreaseScore.gameObject.SetActive(false);
        _totalScore.gameObject.SetActive(false);
        _level.gameObject.SetActive(false);
        _pauseState.gameObject.SetActive(false);
        _pauseKey.gameObject.SetActive(false);
        _retrunTitle.gameObject.SetActive(false);
        _gameExit.gameObject.SetActive(false);
        _readyState.gameObject.SetActive(true);
        _darkOverlay.gameObject.SetActive(true);
        GameManager.Instance.ReadyState();
    }

    void PlayingState()
    {
        Debug.Log("PlayingState CALLED");
        _darkOverlay.gameObject.SetActive(false);
        _readyState.gameObject.SetActive(false);
        _pauseState.gameObject.SetActive(false);
        _pauseKey.gameObject.SetActive(true);
        _totalScore.gameObject.SetActive(true);
        _level.gameObject.SetActive(true);
        GameManager.Instance.StartGame();
        StageSystem.Instance.StartStage();
        
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

    private IEnumerator CameraMoveThenStartGame()
    {
        yield return StartCoroutine(MoveCamera(mainCamera.transform, firstCameraMove.transform.position, firstCameraMove.transform.rotation, moveDuration));
        yield return StartCoroutine(MoveCamera(mainCamera.transform, secondCameraMove.transform.position, secondCameraMove.transform.rotation, moveDuration));
        ReadyState();
    }

    private IEnumerator CameraMoveThenClearGame()
    {
        removeSPSet.SetActive(false);
        yield return StartCoroutine(MoveCamera(mainCamera.transform, thirdCameraMove.transform.position, thirdCameraMove.transform.rotation, moveDuration));
        yield return StartCoroutine(MoveCamera(mainCamera.transform, fourthCameraMove.transform.position, fourthCameraMove.transform.rotation, moveDuration));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator MoveCamera(Transform cam, Vector3 targetPos, Quaternion targetRot, float duration)
    {
        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            t = Mathf.SmoothStep(0f, 1f, t);

            cam.position = Vector3.Lerp(startPos, targetPos, t);
            cam.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        cam.position = targetPos;
        cam.rotation = targetRot;
    }

    private void StageCleared(StageClearedEvent evt)
    {
        StartCoroutine(CameraMoveThenClearGame());
    }
}
