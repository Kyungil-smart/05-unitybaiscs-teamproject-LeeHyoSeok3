using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanJBlock : MonoBehaviour
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
        if(!_this._blockOn && 
           !_this._upBlock._blockOn &&
           !_this._downBlock._blockOn &&
           !_this._leftBlock._downBlock._blockOn )
        {
            if(!_board.JUpList.Contains(_this))
                _board.JUpList.Add(_this);
        }
        else
        {
            if( _board.JUpList.Contains(_this))
                _board.JUpList.Remove(_this);
        }
    }
    void CanRight()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._rightBlock._blockOn &&
            !_this._leftBlock._blockOn &&
            !_this._upBlock._leftBlock._blockOn 
            )
        {
            if(!_board.JRightList.Contains(_this))
                _board.JRightList.Add(_this);
        }
        else
        {
            if( _board.JRightList.Contains(_this))
                _board.JRightList.Remove(_this);
        }
    }
    void CanDown()
    {
        //가능성 판독
        if( !_this._blockOn && 
            !_this._downBlock._blockOn &&
            !_this._upBlock._rightBlock._blockOn &&
            !_this._upBlock._blockOn )
        {
            if(!_board.JDownList.Contains(_this))
                _board.JDownList.Add(_this);
        }
        else
        {
            if( _board.JDownList.Contains(_this))
                _board.JDownList.Remove(_this);
        }
    }
    void CanLeft()
    {
        //가능성 판독
        if( !_this._blockOn &&
            !_this._leftBlock._blockOn &&
            !_this._rightBlock._blockOn &&
            !_this._rightBlock._downBlock._blockOn )
        {
            //리스트에 업
            if(!_board.JLeftList.Contains(_this))
                _board.JLeftList.Add(_this);
        }
        else
        {
            //리스트에서 해제
            if( _board.JLeftList.Contains(_this))
                _board.JLeftList.Remove(_this);
        }
    }
}
