using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanLBlock : MonoBehaviour

{
    [SerializeField] CangenerateBolockList _board;
    private GridTile _this;
    CanLBlock _down;
    CanLBlock _left;
    CanLBlock _right;
    CanLBlock _up;
    

    void Awake()
    {
        _this = GetComponent<GridTile>();
    }
    void Start()
    {
        _down = _this._downBlock.GetComponent<CanLBlock>();
        _left = _this._leftBlock.GetComponent<CanLBlock>();
        _right = _this._rightBlock.GetComponent<CanLBlock>();
        _up = _this._upBlock.GetComponent<CanLBlock>();
        
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
        _up._up.CheckAll();
        _up._up._left.CheckAll();
        _up._up._left._left.CheckAll();
        _up._up._right.CheckAll();
        _up._up._right._right.CheckAll();
        _up.CheckAll();
        _up._left.CheckAll();
        _up._left._left.CheckAll();
        _up._right.CheckAll();
        _up._right._right.CheckAll();
        _left.CheckAll();
        _left._left.CheckAll();
        CheckAll();
        _right.CheckAll();
        _right._right.CheckAll();
        _down.CheckAll();
        _down._left.CheckAll();
        _down._left._left.CheckAll();
        _down._right.CheckAll();
        _down._right._right.CheckAll();
        _down._down.CheckAll();
        _down._down._left.CheckAll();
        _down._down._left._left.CheckAll();
        _down._down._right.CheckAll();
        _down._down._right._right.CheckAll();
    }
    
    void CanUp()
    {
        //가능성 판독
        if( !_this.OnPre() && 
            !_this._upBlock.OnPre() &&
            !_this._downBlock.OnPre() &&
            !_this._rightBlock._downBlock.OnPre() )
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
        if( !_this.OnPre() && 
            !_this._leftBlock.OnPre() &&
            !_this._rightBlock.OnPre() &&
            !_this._leftBlock._downBlock.OnPre() )
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
        if( !_this.OnPre() && 
            !_this._downBlock.OnPre() &&
            !_this._upBlock.OnPre() &&
            !_this._upBlock._leftBlock.OnPre() )
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
        if( !_this.OnPre() && 
            !_this._leftBlock.OnPre() &&
            !_this._rightBlock.OnPre() &&
            !_this._rightBlock._upBlock.OnPre() )
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
