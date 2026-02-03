using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanTBlock : MonoBehaviour
{
    [SerializeField] CangenerateBolockList _board;
    private GridTile _this;
    CanTBlock _down;
    CanTBlock _left;
    CanTBlock _right;
    CanTBlock _up;
    

    void Awake()
    {
        _this = GetComponent<GridTile>();
    }
    void Start()
    {
        _down = _this._downBlock.GetComponent<CanTBlock>();
        _left = _this._leftBlock.GetComponent<CanTBlock>();
        _right = _this._rightBlock.GetComponent<CanTBlock>();
        _up = _this._upBlock.GetComponent<CanTBlock>();
        
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
        _up._right._right._right.CheckAll();
        _up._right._right.CheckAll();
        _up._right.CheckAll();
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
        _down._right._right._right.CheckAll();
        _down._right._right.CheckAll();
        _down._right.CheckAll();
    }
    void CanUp()
    {
        //가능성 판독
        if( !_this.OnPre() && 
            !_this._downBlock.OnPre() &&
            !_this._leftBlock.OnPre() &&
            !_this._rightBlock.OnPre() )
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
        if( !_this.OnPre() && 
            !_this._upBlock.OnPre() &&
            !_this._downBlock.OnPre() &&
            !_this._leftBlock.OnPre() )
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
        if( !_this.OnPre() && 
            !_this._upBlock.OnPre() &&
            !_this._leftBlock.OnPre() &&
            !_this._rightBlock.OnPre() )
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
        if( !_this.OnPre() && 
            !_this._upBlock.OnPre() &&
            !_this._downBlock.OnPre() &&
            !_this._rightBlock.OnPre() )
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