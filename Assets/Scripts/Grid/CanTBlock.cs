using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanTBlock : MonoBehaviour
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
            !gameObject.GetComponent<GridTile>()._downBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._leftBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._rightBlock._blockOn )
        {
            if(!_board.TUpList.Contains(GetComponent<GridTile>()))
                _board.TUpList.Add(GetComponent<GridTile>());
        }
        else
        {
            if( _board.TUpList.Contains(GetComponent<GridTile>()))
                _board.TUpList.Remove(GetComponent<GridTile>());
        }
    }
    void CanRight()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._upBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._downBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._leftBlock._blockOn )
        {
            if(!_board.TRightList.Contains(GetComponent<GridTile>()))
                _board.TRightList.Add(GetComponent<GridTile>());
        }
        else
        {
            if( _board.TRightList.Contains(GetComponent<GridTile>()))
                _board.TRightList.Remove(GetComponent<GridTile>());
        }
    }
    void CanDown()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._upBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._leftBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._rightBlock._blockOn )
        {
            if(!_board.TDownList.Contains(GetComponent<GridTile>()))
                _board.TDownList.Add(GetComponent<GridTile>());
        }
        else
        {
            if( _board.TDownList.Contains(GetComponent<GridTile>()))
                _board.TDownList.Remove(GetComponent<GridTile>());
        }
    }
    void CanLeft()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._upBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._downBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._rightBlock._blockOn )
        {
            //리스트에 업
            if(!_board.TLeftList.Contains(GetComponent<GridTile>()))
                _board.TLeftList.Add(GetComponent<GridTile>());
        }
        else
        {
            //리스트에서 해제
            if( _board.TLeftList.Contains(GetComponent<GridTile>()))
                _board.TLeftList.Remove(GetComponent<GridTile>());
        }
    }
}