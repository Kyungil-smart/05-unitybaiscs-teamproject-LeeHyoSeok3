using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanZBlock : MonoBehaviour
{
    [SerializeField] CangenerateBolockList _board;
    private GridTile _this;
    CanZBlock _down;
    CanZBlock _left;
    CanZBlock _right;
    CanZBlock _up;
    

    void Awake()
    {
        _this = GetComponent<GridTile>();
    }
    void Start()
    {
        _down = _this._downBlock.GetComponent<CanZBlock>();
        _left = _this._leftBlock.GetComponent<CanZBlock>();
        _right = _this._rightBlock.GetComponent<CanZBlock>();
        _up = _this._upBlock.GetComponent<CanZBlock>();
        
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
            !_this._leftBlock.OnPre() &&
            !_this._downBlock.OnPre() &&
            !_this._downBlock._rightBlock.OnPre() )
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
        if( !_this.OnPre() && 
            !_this._leftBlock.OnPre() &&
            !_this._upBlock.OnPre() &&
            !_this._downBlock._leftBlock.OnPre() )
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
        if( !_this.OnPre() && 
            !_this._upBlock.OnPre() &&
            !_this._rightBlock.OnPre() &&
            !_this._upBlock._leftBlock.OnPre() )
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
        if( !_this.OnPre() && 
            !_this._downBlock.OnPre() &&
            !_this._rightBlock.OnPre() &&
            !_this._upBlock._rightBlock.OnPre() )
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
