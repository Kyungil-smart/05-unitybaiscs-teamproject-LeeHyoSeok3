using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClear : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RectTransform uiGroup; // 부모
    [SerializeField] private RectTransform button;  // 버튼 기준용
    [SerializeField] private GameObject _gameExit;

    [Header("Move Settings")]
    [SerializeField] private float speed = 200f;
    [SerializeField] private float stopOffsetY = 100f;

    private bool isMoving = true;

    void Start()
    {
        Time.timeScale = 1;
        StartCoroutine(MoveUp());
    }

    IEnumerator MoveUp()
    {
        while (isMoving)
        {
            // 그룹 전체 위로 이동
            uiGroup.anchoredPosition += Vector2.up * speed * Time.deltaTime;

            float buttonY = uiGroup.anchoredPosition.y + button.anchoredPosition.y;

            if (buttonY >= stopOffsetY)
            {
                isMoving = false;
            }

            yield return null;
        }
    }

    public void ReturnTitle()
    {
        GameManager.Instance.InitializeGame();
    }

    public void AreYouSure()
    {
        _gameExit.gameObject.SetActive(true);
    }

    public void NO()
    {
        _gameExit.gameObject.SetActive(false);
    }
}
