using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanZBlock : MonoBehaviour
{
    [SerializeField] CangenerateBolockList _board;
    private GridTile _this;

    void Awake()
    {
        _this = GetComponent<GridTile>();
    }
    void Start()
    {
        CanUp();
        CanRight();
        CanDown();
        CanLeft();
    }


    void OnEnable()
    {
        GameEventBus.Subscribe<GridUpdateEvent>(CheckAll);
    }
    void OnDisable()
    {
        GameEventBus.Unsubscribe<GridUpdateEvent>(CheckAll);
    }
    void CheckAll(GridUpdateEvent evt)
    {
        CanUp();
        CanRight();
        CanDown();
        CanLeft();
    }
    
    void CanUp()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._leftBlock._blockOn &&
            !_this._downBlock._blockOn &&
            !_this._downBlock._rightBlock._blockOn )
        {
            if(!_board.ZUpList.Contains(_this))
                _board.ZUpList.Add(_this);
        }
        else
        {
            if( _board.ZUpList.Contains(_this))
                _board.ZUpList.Remove(_this);
        }
    }
    void CanRight()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._leftBlock._blockOn &&
            !_this._upBlock._blockOn &&
            !_this._downBlock._leftBlock._blockOn )
        {
            if(!_board.ZRightList.Contains(_this))
                _board.ZRightList.Add(_this);
        }
        else
        {
            if( _board.ZRightList.Contains(_this))
                _board.ZRightList.Remove(_this);
        }
    }
    void CanDown()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._upBlock._blockOn &&
            !_this._rightBlock._blockOn &&
            !_this._upBlock._leftBlock._blockOn )
        {
            if(!_board.ZDownList.Contains(_this))
                _board.ZDownList.Add(_this);
        }
        else
        {
            if( _board.ZDownList.Contains(_this))
                _board.ZDownList.Remove(_this);
        }
    }
    void CanLeft()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._downBlock._blockOn &&
            !_this._rightBlock._blockOn &&
            !_this._upBlock._rightBlock._blockOn )
        {
            //리스트에 업
            if(!_board.ZLeftList.Contains(_this))
                _board.ZLeftList.Add(_this);
        }
        else
        {
            //리스트에서 해제
            if( _board.ZLeftList.Contains(_this))
                _board.ZLeftList.Remove(_this);
        }
    }
}
