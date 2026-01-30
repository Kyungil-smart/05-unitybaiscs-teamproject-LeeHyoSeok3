using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController 
{
    private MonsterMovement _movement;

    public Vector2Int GridPos { get; private set; }
    public MonsterState State { get; private set; }

    public float YPos;

    private readonly MonsterView _monView;
    private readonly MonsterPoolType _poolType;
    
    public MonsterController(MonsterView view, Vector2Int gridPos, MonsterPoolType poolType)
    {
        _monView = view;
        _movement = new MonsterMovement();
        State = MonsterState.Spawn;
        _poolType = poolType;

        YPos = 0f;

       SetGridPosition(gridPos);
    }

    public void ChasePlayer(Vector3 Start, Vector3 target)
    {
        if(State == MonsterState.Chasing) { _movement.ChasePlayer(Start, target); }
    }

    public void SetGridPosition(Vector2Int gridPos)
    {
        GridPos = gridPos;
        Vector3 worldPos = new Vector3(gridPos.x, YPos, gridPos.y);

        _monView.SetWorldPos(worldPos);
    }

    public void SetState(MonsterState state) => State = state;

    public void Release()
    {
        PoolManager.Instance.GetPool<MonsterView>((int)_poolType).Release(_monView);
    }
}
