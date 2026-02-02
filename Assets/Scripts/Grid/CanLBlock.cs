using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanLBlock : MonoBehaviour

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
            !_this._upBlock._blockOn &&
            !_this._downBlock._blockOn &&
            !_this._rightBlock._downBlock._blockOn )
        {
            if(!_board.LUpList.Contains(_this))
                _board.LUpList.Add(_this);
        }
        else
        {
            if( _board.LUpList.Contains(_this))
                _board.LUpList.Remove(_this);
        }
    }
    void CanRight()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._leftBlock._blockOn &&
            !_this._rightBlock._blockOn &&
            !_this._leftBlock._downBlock._blockOn )
        {
            if(!_board.LRightList.Contains(_this))
                _board.LRightList.Add(_this);
        }
        else
        {
            if( _board.LRightList.Contains(_this))
                _board.LRightList.Remove(_this);
        }
    }
    void CanDown()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._downBlock._blockOn &&
            !_this._upBlock._blockOn &&
            !_this._upBlock._leftBlock._blockOn )
        {
            if(!_board.LDownList.Contains(_this))
                _board.LDownList.Add(_this);
        }
        else
        {
            if( _board.LDownList.Contains(_this))
                _board.LDownList.Remove(_this);
        }
    }
    void CanLeft()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._leftBlock._blockOn &&
            !_this._rightBlock._blockOn &&
            !_this._rightBlock._upBlock._blockOn )
        {
            //리스트에 업
            if(!_board.LLeftList.Contains(_this))
                _board.LLeftList.Add(_this);
        }
        else
        {
            //리스트에서 해제
            if( _board.LLeftList.Contains(_this))
                _board.LLeftList.Remove(_this);
        }
    }
}
