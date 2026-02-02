using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanIBlock : MonoBehaviour
{
    [SerializeField] CangenerateBolockList _board;
    private GridTile _this;
    CanIBlock _down;
    CanIBlock _left;
    CanIBlock _right;
    CanIBlock _up;
    

    void Awake()
    {
        _this = GetComponent<GridTile>();
    }
    void Start()
    {
        _down = _this._downBlock.GetComponent<CanIBlock>();
        _left = _this._leftBlock.GetComponent<CanIBlock>();
        _right = _this._rightBlock.GetComponent<CanIBlock>();
        _up = _this._upBlock.GetComponent<CanIBlock>();
        
        CanUp();
        CanRight();
        CanDown();
        CanLeft();
    }

    public void CheckAll()
    {
        CanUp();
        CanRight();
        CanDown();
        CanLeft();
    }

    public void CheckNear()
    {
        _up._up._up._up.CheckAll();
        _up._up._up.CheckAll();
        _up._up._up._left.CheckAll();
        _up._up._up._right.CheckAll();
        _up._up.CheckAll();
        _up._up._left.CheckAll();
        _up._up._left._left.CheckAll();
        _up._up._right.CheckAll();
        _up._up._right._right.CheckAll();
        _up.CheckAll();
        _up._left.CheckAll();
        _up._left._left.CheckAll();
        _up._left._left._left.CheckAll();
        _left.CheckAll();
        _left._left.CheckAll();
        _left._left._left.CheckAll();
        _left._left._left._left.CheckAll();
        CheckAll();
        _right.CheckAll();
        _right._right.CheckAll();
        _right._right._right.CheckAll();
        _right._right._right._right.CheckAll();
        _down._down._down._down.CheckAll();
        _down._down._down.CheckAll();
        _down._down._down._left.CheckAll();
        _down._down._down._right.CheckAll();
        _down._down.CheckAll();
        _down._down._left.CheckAll();
        _down._down._left._left.CheckAll();
        _down._down._right.CheckAll();
        _down._down._right._right.CheckAll();
        _down.CheckAll();
        _down._left.CheckAll();
        _down._left._left.CheckAll();
        _down._left._left._left.CheckAll();
        
    }
    
    void CanUp()
    {
        //가능성 판독
        if( !_this.OnPre() && 
            !_this._rightBlock.OnPre() &&
            !_this._rightBlock._rightBlock.OnPre() &&
            !_this._leftBlock.OnPre() )
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
        if( !_this.OnPre() && 
            !_this._downBlock.OnPre() &&
            !_this._downBlock._downBlock.OnPre() &&
            !_this._upBlock.OnPre() )
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
        if( !_this.OnPre() && 
            !_this._leftBlock.OnPre() &&
            !_this._leftBlock._leftBlock.OnPre() &&
            !_this._rightBlock.OnPre() )
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
        if( !_this.OnPre() && 
            !_this._upBlock.OnPre() &&
            !_this._upBlock._upBlock.OnPre() &&
            !_this._downBlock.OnPre() )
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
