using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterView : MonoBehaviour, IPoolable
{
    public MonsterController Controller { get; private set; }

    // 플레이어 좌표
    public Transform PlayerPos { get; private set; }

    // 그리드
    private GridTile[] Tiles;

    public void Initialize(MonsterController controller)
    {
        Controller = controller;
        Tiles = GameObject.Find("Grid").GetComponents<GridTile>();
    }

    private void OnEnable()
    {
        // 플레이어 좌표가져오기
        PlayerPos = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        // 플레이어에 도착해 충돌 했을 때
        // 충돌하지못했을 때

        // Despawn
    }

    private void FixedUpdate()
    {
        // A* 알고리즘으로 플레이어 추적
        Controller.ChasePlayer(transform.position, PlayerPos.position);
    }

    public void OnSpawn()
    {
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        gameObject.SetActive(false);
    }

    public void SetWorldPos(Vector3 pos) => transform.position = pos;

}
