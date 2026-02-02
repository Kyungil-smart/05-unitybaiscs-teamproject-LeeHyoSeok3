using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanTBlock : MonoBehaviour
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
            !_this._downBlock._blockOn &&
            !_this._leftBlock._blockOn &&
            !_this._rightBlock._blockOn )
        {
            if(!_board.TUpList.Contains(_this))
                _board.TUpList.Add(_this);
        }
        else
        {
            if( _board.TUpList.Contains(_this))
                _board.TUpList.Remove(_this);
        }
    }
    void CanRight()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._upBlock._blockOn &&
            !_this._downBlock._blockOn &&
            !_this._leftBlock._blockOn )
        {
            if(!_board.TRightList.Contains(_this))
                _board.TRightList.Add(_this);
        }
        else
        {
            if( _board.TRightList.Contains(_this))
                _board.TRightList.Remove(_this);
        }
    }
    void CanDown()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._upBlock._blockOn &&
            !_this._leftBlock._blockOn &&
            !_this._rightBlock._blockOn )
        {
            if(!_board.TDownList.Contains(_this))
                _board.TDownList.Add(_this);
        }
        else
        {
            if( _board.TDownList.Contains(_this))
                _board.TDownList.Remove(_this);
        }
    }
    void CanLeft()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._upBlock._blockOn &&
            !_this._downBlock._blockOn &&
            !_this._rightBlock._blockOn )
        {
            //리스트에 업
            if(!_board.TLeftList.Contains(_this))
                _board.TLeftList.Add(_this);
        }
        else
        {
            //리스트에서 해제
            if( _board.TLeftList.Contains(_this))
                _board.TLeftList.Remove(_this);
        }
    }
}