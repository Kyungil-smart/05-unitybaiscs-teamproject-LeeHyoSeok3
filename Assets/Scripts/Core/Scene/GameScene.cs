using CartoonFX;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _totalScore;
    [SerializeField] private TextMeshProUGUI _IncreaseScore;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _readyState;
    [SerializeField] private TextMeshProUGUI _pauseState;
    [SerializeField] private TextMeshProUGUI _pauseKey;
    [SerializeField] private TextMeshProUGUI _gameOverState;
    [SerializeField] private Image _darkOverlay;
    [SerializeField] private Button _retrunTitle;
    [SerializeField] private Button _retrunTitle2;
    [SerializeField] private GameObject _gameExit;
    [SerializeField] private GameObject _gameExit2;
    [SerializeField] private GameObject _returnGame;
    [SerializeField] private GameObject _returnGame2;
    [SerializeField] private CFXR_Effect _destroyEffectPrefab;
    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float moveDuration;
    [SerializeField] private GameObject firstCameraMove;
    [SerializeField] private GameObject secondCameraMove;
    [SerializeField] private GameObject thirdCameraMove;
    [SerializeField] private GameObject fourthCameraMove;
    [SerializeField] private GameObject topViewCamera;
    [Header("Remove")]
    [SerializeField] private GameObject removeSPSet;

    private PlayerCollision PlayerCollision;
    private int _currentScore;
    private int _currentStage;
    private int _targetScore;
    private bool _cameraViewed = false;
    private float _viewTransTime = 1.0f;
    private bool _cameraMoving = false;

    void Awake()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }
    void Start()
    {
        _currentStage = StageSystem.Instance.CurrentStage;
        _targetScore = StageSystem.Instance.StageTargetScore;
        PlayerCollision = FindObjectOfType<PlayerCollision>();
        StartCoroutine(CameraMoveThenStartGame());
    }

    private void OnEnable()
    {
        GameEventBus.Subscribe<ScoreUpdatedEvent>(OnScoreUpdated);
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

        if (Input.GetKeyDown(KeyCode.Escape))
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

        if (PlayerCollision._playerDead &&
            !(GameManager.Instance.StateMachine.CurrnetState is GameOverState))
        {
            GameOver();
        }

        if (GameManager.Instance.StateMachine.CurrnetState is PlayingState && Input.GetKeyDown(KeyCode.Alpha0))
        {
            StageSystem.Instance.TestSpawn();
        }

        if (GameManager.Instance.StateMachine.CurrnetState is PlayingState && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && !_cameraMoving)
        {
            if (_cameraViewed == false)
            {
                StartCoroutine(ViewTrans(mainCamera.transform, topViewCamera.transform.position, topViewCamera.transform.rotation));
                _cameraViewed = true;
            }
            else
            {
                StartCoroutine(ViewTrans(mainCamera.transform, secondCameraMove.transform.position, secondCameraMove.transform.rotation));
                _cameraViewed = false;
            }
        }

    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe<ScoreUpdatedEvent>(OnScoreUpdated);
        GameEventBus.Unsubscribe<StageClearedEvent>(StageCleared);
    }

    private void OnScoreUpdated(ScoreUpdatedEvent evt)
    {
        _currentScore = evt.Score;
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
        _gameOverState.gameObject.SetActive(false);
        _returnGame.gameObject.SetActive(false);
        _returnGame2.gameObject.SetActive(false);
        _readyState.gameObject.SetActive(true);
        _darkOverlay.gameObject.SetActive(true);
        GameManager.Instance.ReadyState();
    }

    void PlayingState()
    {
        _darkOverlay.gameObject.SetActive(false);
        _readyState.gameObject.SetActive(false);
        _pauseState.gameObject.SetActive(false);
        _returnGame.gameObject.SetActive(false);
        _returnGame2.gameObject.SetActive(false);
        _pauseKey.gameObject.SetActive(true);
        _totalScore.gameObject.SetActive(true);
        _level.gameObject.SetActive(true);
        GameManager.Instance.PlayGame();
    }
    void ResumeGame()
    {
        _darkOverlay.gameObject.SetActive(false);
        _pauseState.gameObject.SetActive(false);
        _retrunTitle.gameObject.SetActive(false);
        _gameExit.gameObject.SetActive(false);
        _returnGame.gameObject.SetActive(false);
        _returnGame2.gameObject.SetActive(false);
        GameManager.Instance.ResumeGame();
    }

    public void PauseState()
    {
        _gameExit.gameObject.SetActive(false);
        _darkOverlay.gameObject.SetActive(true);
        _pauseState.gameObject.SetActive(true);
        _retrunTitle.gameObject.SetActive(true);
        _returnGame.gameObject.SetActive(true);
        GameManager.Instance.PauseGame();
    }

    private void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        PlayerCollision.GetComponentInParent<PlayerAnimator>().PlayerDeath();

        yield return new WaitForSeconds(4f);

        _darkOverlay.gameObject.SetActive(true);
        _retrunTitle2.gameObject.SetActive(true);
        _gameOverState.gameObject.SetActive(true);
        _returnGame2.gameObject.SetActive(true);
        GameManager.Instance.GameOver();
    }

    public void GameOverScnen()
    {
        _gameExit2.gameObject.SetActive(false);
        _darkOverlay.gameObject.SetActive(true);
        _retrunTitle2.gameObject.SetActive(true);
        _gameOverState.gameObject.SetActive(true);
        _returnGame2.gameObject.SetActive(true);
        GameManager.Instance.GameOver();
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
        _gameOverState.text = $"플레이어가 죽었습니다. 게임 오버!";
    }

    public void ReturnTitle()
    {
        GameManager.Instance.InitializeGame();
    }

    public void RetrunGame()
    {
        Time.timeScale = 1f;

        GameEventBus.Raise(
            new LoadSceneRequestedEvent(
                (SceneType)SceneManager.GetActiveScene().buildIndex
            )
        );
    }

    public void AreYouSure()
    {
        _gameExit.gameObject.SetActive(true);
        _returnGame.gameObject.SetActive(false);
    }

    public void AreYouSure2()
    {
        _gameExit2.gameObject.SetActive(true);
        _returnGame2.gameObject.SetActive(false);
    }

    private IEnumerator CameraMoveThenStartGame()
    {
        GameManager.Instance.StartGame();
        yield return StartCoroutine(MoveCamera(mainCamera.transform, firstCameraMove.transform.position, firstCameraMove.transform.rotation, moveDuration));
        yield return StartCoroutine(MoveCamera(mainCamera.transform, topViewCamera.transform.position, topViewCamera.transform.rotation, moveDuration));
        ReadyState();
    }

    private IEnumerator CameraMoveThenClearGame()
    {
        if (_destroyEffectPrefab != null)
        {
            Instantiate(_destroyEffectPrefab, removeSPSet.transform.position, Quaternion.identity);
        }
        removeSPSet.SetActive(false);
        yield return StartCoroutine(MoveCamera(mainCamera.transform, thirdCameraMove.transform.position, thirdCameraMove.transform.rotation, moveDuration));
        yield return StartCoroutine(MoveCamera(mainCamera.transform, fourthCameraMove.transform.position, fourthCameraMove.transform.rotation, moveDuration));
        var currentSceneType = (SceneType)SceneManager.GetActiveScene().buildIndex;
        var nextSceneType = (SceneType)((int)currentSceneType + 1);

        GameEventBus.Raise(new LoadSceneRequestedEvent(nextSceneType));
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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

    private IEnumerator ViewTrans(Transform cam, Vector3 targetPos, Quaternion targetRot)
    {
        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;
        float elapsed = 0f;
        _cameraMoving = true;

        while (elapsed < _viewTransTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _viewTransTime);
            t = Mathf.SmoothStep(0f, 1f, t);

            cam.position = Vector3.Lerp(startPos, targetPos, t);
            cam.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        elapsed = 0f;
        _cameraMoving = false;

        cam.position = targetPos;
        cam.rotation = targetRot;
    }

    private void StageCleared(StageClearedEvent evt)
    {
        GameManager.Instance.StartGame();
        StartCoroutine(CameraMoveThenClearGame());
    }
}
