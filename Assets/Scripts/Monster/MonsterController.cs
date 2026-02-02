using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController 
{
    public MonsterMovement _movement { get; private set; }

    public Vector2Int GridPos { get; private set; }
    public MonsterState State { get; private set; }

    public float YPos;

    private readonly MonsterView _monView;
    private readonly MonsterPoolType _poolType;

    private Vector3 _nextPos;
    
    public MonsterController(MonsterView view, Vector2Int gridPos, MonsterPoolType poolType)
    {
        _monView = view;
        _movement = new MonsterMovement();

        State = MonsterState.Spawn;
        _poolType = poolType;

        YPos = 1f;

       SetGridPosition(gridPos);
    }

    public void ChasePlayer(Vector3 Start, Vector3 target)
    {
        Start = new Vector3(Start.x, 0f, Start.z);
        if (State == MonsterState.Chasing) { _movement.ChasePlayer(Start, target); }

        if (_movement._pathList.Count > 0)
        {
            _nextPos = _movement._pathList.Pop().transform.position;
            Vector3 dir = (_nextPos - _monView.GetVector()).normalized;
            _movement.SetMoveDirection(dir);
            //if(_monView.StartMoveCouroutine != null) { _monView.StopCoroutine(_monView.StartMoveCouroutine); }

            //_monView.StartMoveCouroutine = _monView.StartCoroutine(MovementCoroutine());
        }

        else
        {
            Vector3 dir = (_monView.PlayerPos.position - _monView.GetVector()).normalized;
            _movement.SetMoveDirection(dir);
        }

        _movement.Rotate();
        _movement.Move();
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

    public IEnumerator MovementCoroutine()
    {
        _nextPos = _movement._pathList.Pop().transform.position;
        Vector3 dir = (_nextPos - _monView.GetVector()).normalized;
        _movement.SetMoveDirection(dir);
        

        yield return new WaitUntil( () => Mathf.Abs(_monView.GetVector().x - _nextPos.x) <= 0.3f ||
            Mathf.Abs(_monView.GetVector().z - _nextPos.z) <= 0.3f);
    }

    public MonsterPoolType PoolType()
    {
        return _poolType;
    }
}
