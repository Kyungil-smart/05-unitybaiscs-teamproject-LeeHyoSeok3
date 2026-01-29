using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterView : MonoBehaviour, IPoolable
{
    public MonsterController Controller { get; private set; }

    // 플레이어 좌표

    public void Initialize(MonsterController controller) => Controller = controller;

    private void Update()
    {
        // 플레이어에 도착해 충돌 했을 때
        // 충돌하지못했을 때

        // Despawn
    }

    private void FixedUpdate()
    {
        // A* 알고리즘으로 플레이어 추적
    }

    public void OnSpawn()
    {
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        gameObject.SetActive(false);
    }
}
