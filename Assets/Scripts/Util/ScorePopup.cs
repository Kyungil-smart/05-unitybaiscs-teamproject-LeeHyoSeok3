using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    [SerializeField] private TextMeshPro _scoreText3D; // TextMeshPro (3D) 타입
    [SerializeField] private float moveUpDistance = 1f;
    [SerializeField] private float duration = 1f;

    private Vector3 startPos;
    private float timer;

    // 점수 표시
    public void Show(int score, Transform box)
    {
        _scoreText3D.text = $"+{score}";
        timer = 0f;
        startPos = box.position + Vector3.up * 1f; // 박스 위
        _scoreText3D.transform.position = startPos;
        _scoreText3D.alpha = 1f; // 완전히 보이게
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;

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
