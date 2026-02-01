using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterView : MonoBehaviour, IPoolable
{
    public MonsterController Controller { get; private set; }

    // 占시뤄옙占싱억옙 占쏙옙표
    public Transform PlayerPos { get; private set; }

    // 占쌓몌옙占쏙옙
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

        // 紐ъㅽ  곕  珥湲고
        if(Controller.PoolType() == MonsterPoolType.Scout)
        {
            Controller.SetState(MonsterState.Chasing);
        }
        else if(Controller.PoolType() == MonsterPoolType.Patrol)
        {
            Controller.SetState(MonsterState.Patrol);
        }
    }

    private void FixedUpdate()
    {
        SetGridTile();
        PlayerPos = GameObject.Find("Player").transform;

        // A* 占싯곤옙占쏙옙占쏙옙占쏙옙占쏙옙 占시뤄옙占싱억옙 占쏙옙占쏙옙
        if(Controller.State == MonsterState.Chasing)
            Controller.ChasePlayer(transform.position, PlayerPos.position);
    }

    // 占썸돌 占쏙옙 占쏙옙占쏙옙
    private void OnEnable()
    {
        //Initialize(Controller);
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

    

    private void OnCollisionEnter(Collision collision)  // ⑥댁 釉 우 泥由
    {
        if (collision.gameObject.CompareTag("Player") && (Controller.PoolType() == MonsterPoolType.Scout))
        {
            AttackPlayer();
            Controller.Release();
        }
        
        if (!collision.gameObject.CompareTag("Block"))
            return;

        var blockView = collision.gameObject.GetComponent<BlockView>();
        if (blockView == null)
            return;

        var blockController = blockView.Controler;
        if (!(blockController.State == BlockState.Falling))
            return;
        
        if (Controller.PoolType() == MonsterPoolType.Scout)
        {
            Controller.Release();
        }
        else if (Controller.PoolType() == MonsterPoolType.Patrol)
        {
            _attack.Explosion();
            Controller.Release();
        }
    }

}
