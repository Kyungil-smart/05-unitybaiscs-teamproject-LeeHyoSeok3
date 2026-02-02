using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanOBlock : MonoBehaviour
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
            !_this._rightBlock._blockOn &&
            !_this._upBlock._blockOn &&
            !_this._upBlock._rightBlock._blockOn )
        {
            if(!_board.OUpList.Contains(_this))
                _board.OUpList.Add(_this);
        }
        else
        {
            if( _board.OUpList.Contains(_this))
                _board.OUpList.Remove(_this);
        }
    }
    void CanRight()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._rightBlock._blockOn &&
            !_this._downBlock._blockOn &&
            !_this._downBlock._rightBlock._blockOn )
        {
            if(!_board.ORightList.Contains(_this))
                _board.ORightList.Add(_this);
        }
        else
        {
            if( _board.ORightList.Contains(_this))
                _board.ORightList.Remove(_this);
        }
    }
    void CanDown()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._downBlock._blockOn &&
            !_this._leftBlock._blockOn &&
            !_this._downBlock._leftBlock._blockOn )
        {
            if(!_board.ODownList.Contains(_this))
                _board.ODownList.Add(_this);
        }
        else
        {
            if( _board.ODownList.Contains(_this))
                _board.ODownList.Remove(_this);
        }
    }
    void CanLeft()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._upBlock._blockOn &&
            !_this._leftBlock._blockOn &&
            !_this._upBlock._leftBlock._blockOn )
        {
            //리스트에 업
            if(!_board.OLeftList.Contains(_this))
                _board.OLeftList.Add(_this);
        }
        else
        {
            //리스트에서 해제
            if( _board.OLeftList.Contains(_this))
                _board.OLeftList.Remove(_this);
        }
    }
}
