using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanZBlock : MonoBehaviour
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
            !gameObject.GetComponent<GridTile>()._leftBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._downBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._downBlock._rightBlock._blockOn )
        {
            if(!_board.ZUpList.Contains(GetComponent<GridTile>()))
                _board.ZUpList.Add(GetComponent<GridTile>());
        }
        else
        {
            if( _board.ZUpList.Contains(GetComponent<GridTile>()))
                _board.ZUpList.Remove(GetComponent<GridTile>());
        }
    }
    void CanRight()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._leftBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._upBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._downBlock._leftBlock._blockOn )
        {
            if(!_board.ZRightList.Contains(GetComponent<GridTile>()))
                _board.ZRightList.Add(GetComponent<GridTile>());
        }
        else
        {
            if( _board.ZRightList.Contains(GetComponent<GridTile>()))
                _board.ZRightList.Remove(GetComponent<GridTile>());
        }
    }
    void CanDown()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._upBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._rightBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._upBlock._leftBlock._blockOn )
        {
            if(!_board.ZDownList.Contains(GetComponent<GridTile>()))
                _board.ZDownList.Add(GetComponent<GridTile>());
        }
        else
        {
            if( _board.ZDownList.Contains(GetComponent<GridTile>()))
                _board.ZDownList.Remove(GetComponent<GridTile>());
        }
    }
    void CanLeft()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._downBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._rightBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._upBlock._rightBlock._blockOn )
        {
            //리스트에 업
            if(!_board.ZLeftList.Contains(GetComponent<GridTile>()))
                _board.ZLeftList.Add(GetComponent<GridTile>());
        }
        else
        {
            //리스트에서 해제
            if( _board.ZLeftList.Contains(GetComponent<GridTile>()))
                _board.ZLeftList.Remove(GetComponent<GridTile>());
        }
    }
}
