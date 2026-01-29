using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Vector2Int GridPos { get; private set; }
    public MonsterState State { get; private set; }

    public float YPos;

    private readonly MonsterView _monView;
    private readonly MonsterPoolType _poolType;
    
    public MonsterController(MonsterView view, Vector2Int gridPos, MonsterPoolType poolType)
    {
        _monView = view;

        _poolType = poolType;

        YPos = 0f;

       SetGridPosition(gridPos);
    }

    public void SetGridPosition(Vector2Int gridPos)
    {
        GridPos = gridPos;
        //Vector3 worldPos = new Vector3(gridPos.x, YPos, gridPos.y);
        //_monView.SetWorldPosition(worldPos);
    }

    public void SetState(MonsterState state) => State = state;

    public void Release()
    {
        PoolManager.Instance.GetPool<MonsterView>((int)_poolType).Release(_monView);
    }
}
