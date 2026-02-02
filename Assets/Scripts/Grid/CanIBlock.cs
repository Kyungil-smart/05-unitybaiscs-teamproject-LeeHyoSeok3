using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanIBlock : MonoBehaviour
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
            !_this._rightBlock._rightBlock._blockOn &&
            !_this._rightBlock._rightBlock._rightBlock._blockOn )
        {
            if(!_board.IUpList.Contains(_this))
                _board.IUpList.Add(_this);            
        }
        else
        {
            if( _board.IUpList.Contains(_this))
                _board.IUpList.Remove(_this);
        }
        
    }
    void CanRight()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._downBlock._blockOn &&
            !_this._downBlock._downBlock._blockOn &&
            !_this._downBlock._downBlock._downBlock._blockOn )
        {
            if(!_board.IRightList.Contains(_this))
                _board.IRightList.Add(_this);
        }
        else
        {
            if( _board.IRightList.Contains(_this))
                _board.IRightList.Remove(_this);
        }
    }
    void CanDown()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._leftBlock._blockOn &&
            !_this._leftBlock._leftBlock._blockOn &&
            !_this._leftBlock._leftBlock._leftBlock._blockOn )
        {
            if(!_board.IDownList.Contains(_this))
                _board.IDownList.Add(_this);
        }
        else
        {
            if( _board.IDownList.Contains(_this))
                _board.IDownList.Remove(_this);
        }
    }
    void CanLeft()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._upBlock._blockOn &&
            !_this._upBlock._upBlock._blockOn &&
            !_this._upBlock._upBlock._upBlock._blockOn )
        {
            //리스트에 업
            if(!_board.ILeftList.Contains(_this))
                _board.ILeftList.Add(_this);
        }
        else
        {
            //리스트에서 해제
            if( _board.ILeftList.Contains(_this))
                _board.ILeftList.Remove(_this);
        }
    }
}
