using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public int Score { get; private set; }

    // 테스트 코드
    // private void Update()
    // {
    //     if(Input.GetKeyDown(KeyCode.Space))
    //         GameEventBus.Raise(new LineClearedEvent(1));
    // }

    private void OnEnable()
    {
        GameEventBus.Subscribe<LineClearedEvent>(OnLineCleared);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe<LineClearedEvent>(OnLineCleared);
    }

    private void OnLineCleared(LineClearedEvent evt)
    {
        Score += (evt.Count * evt.Count) * 100;
        Debug.Log($"Score : {Score}");
        GameEventBus.Raise(new ScoreUpdatedEvent(Score));
    }
}
