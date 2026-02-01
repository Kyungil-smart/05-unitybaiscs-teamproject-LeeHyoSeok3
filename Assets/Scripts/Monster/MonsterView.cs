using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterView : MonoBehaviour, IPoolable
{
    public MonsterController Controller { get; private set; }

    // 플레이어 좌표
    public Transform PlayerPos { get; private set; }

    // 그리드
    private GridTile[] Tiles;
    private GridTile[,] gridTiles;
    private LineCheker[] _lineRow;

    private MonsterAttack _attack;

    public void Initialize(MonsterController controller)
    {
        _attack = GetComponent<MonsterAttack>();
        Controller = controller;
        SetGridTile();
        Controller._movement._rb = GetComponent<Rigidbody>();
        Controller.SetState(MonsterState.Chasing);
    }

    private void FixedUpdate()
    {
        SetGridTile();
        PlayerPos = GameObject.Find("Player").transform;

        // A* 알고리즘으로 플레이어 추적
        if(Controller.State == MonsterState.Chasing)
            Controller.ChasePlayer(transform.position, PlayerPos.position);
    }

    // 충돌 시 어택
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _attack.SlowPlayer();
            Controller.Release();
        }
    }

    public void AttackPlayer()
    {
        _attack.SlowPlayer();
    }

    public void OnSpawn()
    {
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        // view 초기화
        // 컨트롤러 초기화
        gameObject.SetActive(false);
    }

    public void SetWorldPos(Vector3 pos) => transform.position = pos;

    private void SetGridTile()
    {
        Tiles = GameObject.Find("Grid").GetComponentsInChildren<GridTile>();
        _lineRow = GameObject.Find("Grid").GetComponentsInChildren<LineCheker>();
        ConvertTileArray();
        Controller._movement.GridTiles = gridTiles;
        Controller._movement.NullTile = GameObject.Find("boolBox").GetComponent<GridTile>();
        Controller._movement._rb = GetComponent<Rigidbody>();
    }

    private void ConvertTileArray()
    {
        gridTiles = new GridTile[_lineRow.Length , _lineRow[0]._tielOnLine.Length ];

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
