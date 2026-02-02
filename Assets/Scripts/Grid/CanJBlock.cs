using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanJBlock : MonoBehaviour
{
    [SerializeField] CangenerateBolockList _board;
    private GridTile _this;
    CanJBlock _down;
    CanJBlock _left;
    CanJBlock _right;
    CanJBlock _up;
    

    void Awake()
    {
        _this = GetComponent<GridTile>();
    }
    void Start()
    {
        _down = _this._downBlock.GetComponent<CanJBlock>();
        _left = _this._leftBlock.GetComponent<CanJBlock>();
        _right = _this._rightBlock.GetComponent<CanJBlock>();
        _up = _this._upBlock.GetComponent<CanJBlock>();
        
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
        if(!_this.OnPre() && 
           !_this._upBlock.OnPre() &&
           !_this._downBlock.OnPre() &&
           !_this._leftBlock._downBlock.OnPre() )
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
        if( !_this.OnPre() && 
            !_this._rightBlock.OnPre() &&
            !_this._leftBlock.OnPre() &&
            !_this._upBlock._leftBlock.OnPre() 
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
        if( !_this.OnPre() && 
            !_this._downBlock.OnPre() &&
            !_this._upBlock._rightBlock.OnPre() &&
            !_this._upBlock.OnPre() )
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
        if( !_this.OnPre() &&
            !_this._leftBlock.OnPre() &&
            !_this._rightBlock.OnPre() &&
            !_this._rightBlock._downBlock.OnPre() )
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
