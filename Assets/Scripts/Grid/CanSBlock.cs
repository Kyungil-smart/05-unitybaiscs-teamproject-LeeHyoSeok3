using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSBlock : MonoBehaviour
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
            !_this._upBlock._blockOn &&
            !_this._upBlock._rightBlock._blockOn )
        {
            if(!_board.SUpList.Contains(_this))
                _board.SUpList.Add(_this);
        }
        else
        {
            if( _board.SUpList.Contains(_this))
                _board.SUpList.Remove(_this);
        }
    }
    void CanRight()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._upBlock._blockOn &&
            !_this._rightBlock._blockOn &&
            !_this._downBlock._rightBlock._blockOn )
        {
            if(!_board.SRightList.Contains(_this))
                _board.SRightList.Add(_this);
        }
        else
        {
            if( _board.SRightList.Contains(_this))
                _board.SRightList.Remove(_this);
        }
    }
    void CanDown()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._rightBlock._blockOn &&
            !_this._downBlock._blockOn &&
            !_this._downBlock._leftBlock._blockOn )
        {
            if(!_board.SDownList.Contains(_this))
                _board.SDownList.Add(_this);
        }
        else
        {
            if( _board.SDownList.Contains(_this))
                _board.SDownList.Remove(_this);
        }
    }
    void CanLeft()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._downBlock._blockOn &&
            !_this._leftBlock._blockOn &&
            !_this._leftBlock._upBlock._blockOn )
        {
            //리스트에 업
            if(!_board.SLeftList.Contains(_this))
                _board.SLeftList.Add(_this);
        }
        else
        {
            //리스트에서 해제
            if( _board.SLeftList.Contains(_this))
                _board.SLeftList.Remove(_this);
        }
    }
}