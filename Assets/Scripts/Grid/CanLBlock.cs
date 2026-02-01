using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanLBlock : MonoBehaviour

{
    [SerializeField] CangenerateBolockList _board;

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
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._upBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._downBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._rightBlock._downBlock._blockOn )
        {
            if(!_board.LUpList.Contains(GetComponent<GridTile>()))
                _board.LUpList.Add(GetComponent<GridTile>());
        }
        else
        {
            if( _board.LUpList.Contains(GetComponent<GridTile>()))
                _board.LUpList.Remove(GetComponent<GridTile>());
        }
    }
    void CanRight()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._leftBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._rightBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._leftBlock._downBlock._blockOn )
        {
            if(!_board.LRightList.Contains(GetComponent<GridTile>()))
                _board.LRightList.Add(GetComponent<GridTile>());
        }
        else
        {
            if( _board.LRightList.Contains(GetComponent<GridTile>()))
                _board.LRightList.Remove(GetComponent<GridTile>());
        }
    }
    void CanDown()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._downBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._upBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._upBlock._leftBlock._blockOn )
        {
            if(!_board.LDownList.Contains(GetComponent<GridTile>()))
                _board.LDownList.Add(GetComponent<GridTile>());
        }
        else
        {
            if( _board.LDownList.Contains(GetComponent<GridTile>()))
                _board.LDownList.Remove(GetComponent<GridTile>());
        }
    }
    void CanLeft()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._leftBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._rightBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._rightBlock._upBlock._blockOn )
        {
            //리스트에 업
            if(!_board.LLeftList.Contains(GetComponent<GridTile>()))
                _board.LLeftList.Add(GetComponent<GridTile>());
        }
        else
        {
            //리스트에서 해제
            if(_board.LLeftList.Contains(GetComponent<GridTile>()))
            _board.LLeftList.Remove(GetComponent<GridTile>());
        }
    }
}
