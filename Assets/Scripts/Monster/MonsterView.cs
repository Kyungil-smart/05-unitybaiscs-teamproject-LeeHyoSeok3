using CartoonFX;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterView : MonoBehaviour, IPoolable
{
    public MonsterController Controller { get; private set; }

    public Transform PlayerPos { get; private set; }
    private Vector3 _playerGridPos;
    [SerializeField] public Vector3 GridPos;

    public Coroutine StartMoveCouroutine;
    public Coroutine StopMoveCouroutine;

    [SerializeField] private CFXR_Effect _destroyEffectPrefab;

    private GridTile[] Tiles;
    private GridTile[,] gridTiles;
    private LineCheker[] _lineRow;

    private MonsterAttack _attack;

    public void Initialize(MonsterController controller)
    {
        PlayerPos = GameObject.Find("Player").transform;
        _attack = GetComponent<MonsterAttack>();
        Controller = controller;
        Controller._movement._rb = GetComponent<Rigidbody>();

        if(Controller.PoolType() == MonsterPoolType.Scout)
        {
            Controller.SetState(MonsterState.Chasing);
        }
        else if(Controller.PoolType() == MonsterPoolType.Patrol)
        {
            Controller.SetState(MonsterState.Patrol);
        }

        SetGridTile();
    }
    private void FixedUpdate()
    {
        if (Controller.State == MonsterState.Chasing)
        {
            if (GridPos == null || GridPos == Vector3.zero)
            {
                Controller.ChasePlayer(transform.position, PlayerPos.position);
            }

            Controller.ChasePlayer(GridPos, PlayerPos.position);
        }
    }


    public Vector3 GetVector()
    {
        return new Vector3(transform.position.x, 0, transform.position.z);
    }
    public void OnSpawn()
    {
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        // 이펙트 표출하는 기능
        if (_destroyEffectPrefab != null)
        {
            Instantiate(_destroyEffectPrefab, transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
    }

    public void SetWorldPos(Vector3 pos) => transform.position = pos;

    public void SetCurrentPos(Vector3 pos)
    {
        GridPos = pos;
    }

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
            tile.Parents = null;
            if (tile != GameObject.Find("boolBox").GetComponent<GridTile>())
            {
                gridTiles[(int)tile.transform.position.z,
                (int)tile.transform.position.x] = tile;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)  
    {
        if (collision.gameObject.CompareTag("Player") && (Controller.PoolType() == MonsterPoolType.Scout))
        {
            _attack.SlowPlayer();
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
