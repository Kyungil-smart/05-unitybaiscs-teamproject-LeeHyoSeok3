using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraObstacleFade : MonoBehaviour
{
    private Renderer _renderer;
    [SerializeField] private float _fade = 0.1f;
    private readonly float _originFade = 1f;

    // 블럭이 서서히 선명해지게 변경
    IEnumerator FadeOut(Renderer targetRend)
    {
        Color c = targetRend.material.color;
        while (c.a < _originFade)
        {
            c.a += Time.deltaTime * 1f; 
            targetRend.material.color = c;
            yield return null;
        }
        c.a = _originFade;
        targetRend.material.color = c;
    }

    // Trigger 충돌 시 투명해지게 변경
    private void OnTriggerEnter(Collider other) 
    {
        _renderer = other.GetComponent<Renderer>();
        if (_renderer != null)
        {
            Color c = _renderer.material.color;
            c.a = _fade;
            _renderer.material.color = c;
        }
    }

    // Trigger 범위에서 벗어나면 투명도를 서서히 원상복구하는 코루틴 실행
    private void OnTriggerExit(Collider other)
    {
        Renderer targetRend = other.GetComponent<Renderer>();
        if (targetRend != null)
        {
            StartCoroutine(FadeOut(targetRend));
        }
    }
}