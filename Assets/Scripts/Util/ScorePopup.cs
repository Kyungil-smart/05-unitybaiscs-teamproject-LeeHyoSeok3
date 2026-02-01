using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    [SerializeField] private TextMeshPro _scoreText3D;
    [SerializeField] private float moveUpDistance = 1f;
    [SerializeField] private float duration = 1f;

    private Camera mainCam;

    void Awake()
    {
        mainCam = Camera.main;
    }

    private Vector3 startPos;
    private float timer;

    // 점수 표시
    public void Show(int score, Vector3 worldPos)
    {
        _scoreText3D.text = $"+{score}";
        timer = 0f;

        transform.position = worldPos + Vector3.up * 1f;
        startPos = transform.position;

        _scoreText3D.alpha = 1f;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;

        transform.forward = mainCam.transform.forward;

        timer += Time.deltaTime;
        float t = timer / duration;

        // 위로 이동
        _scoreText3D.transform.position = startPos + Vector3.up * moveUpDistance * t;

        // 점점 투명해지기
        _scoreText3D.alpha = 1f - t;

        // 시간 끝나면 비활성화
        if (timer >= duration)
        {
            gameObject.SetActive(false);
        }
    }
}
