using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterView : MonoBehaviour, IPoolable
{
    public MonsterController Controller { get; private set; }

    // 플레이어 좌표
    [SerializeField] public Transform PlayerPos { get; private set; }

    // 그리드
    private GridTile[] Tiles;
    private GridTile[,] gridTiles;

    public void Initialize(MonsterController controller)
    {
        Controller = controller;
        SetGridTile();
        Controller._movement._rb = GetComponent<Rigidbody>();
        Controller.SetState(MonsterState.Chasing);
    }

    private void OnEnable()
    {
        
    }

    private void Update()
    {
        // 플레이어에 도착해 충돌 했을 때
        // 충돌하지못했을 때

        // Despawn
    }

    private void FixedUpdate()
    {
        SetGridTile();
        PlayerPos = GameObject.Find("Player").transform;

        // A* 알고리즘으로 플레이어 추적
        if(Controller.State == MonsterState.Chasing)
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

    private void SetGridTile()
    {
        Tiles = GameObject.Find("Grid").GetComponentsInChildren<GridTile>();
        ConvertTileArray();
        Controller._movement.GridTiles = gridTiles;
        Controller._movement.NullTile = GameObject.Find("boolBox").GetComponent<GridTile>();
        Controller._movement._rb = GetComponent<Rigidbody>();
    }

    private void ConvertTileArray()
    {
        int i = 0;

        while (!(Mathf.Pow(i, 2) == Tiles.Length-1))
            i++;

        gridTiles = new GridTile[i, i];

        foreach (GridTile tile in Tiles)
        {
            if (tile != GameObject.Find("boolBox").GetComponent<GridTile>())
            {
                gridTiles[(int)tile.transform.position.z,
                (int)tile.transform.position.x] = tile;
            }
        }
    }

}
