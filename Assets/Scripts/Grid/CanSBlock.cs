using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSBlock : MonoBehaviour
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
            !gameObject.GetComponent<GridTile>()._upBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._upBlock._rightBlock._blockOn )
        {
            if(!_board.SUpList.Contains(GetComponent<GridTile>()))
                _board.SUpList.Add(GetComponent<GridTile>());
        }
        else
        {
            if( _board.SUpList.Contains(GetComponent<GridTile>()))
                _board.SUpList.Remove(GetComponent<GridTile>());
        }
    }
    void CanRight()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._upBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._rightBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._downBlock._rightBlock._blockOn )
        {
            if(!_board.SRightList.Contains(GetComponent<GridTile>()))
                _board.SRightList.Add(GetComponent<GridTile>());
        }
        else
        {
            if( _board.SRightList.Contains(GetComponent<GridTile>()))
                _board.SRightList.Remove(GetComponent<GridTile>());
        }
    }
    void CanDown()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._rightBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._downBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._downBlock._leftBlock._blockOn )
        {
            if(!_board.SDownList.Contains(GetComponent<GridTile>()))
                _board.SDownList.Add(GetComponent<GridTile>());
        }
        else
        {
            if( _board.SDownList.Contains(GetComponent<GridTile>()))
                _board.SDownList.Remove(GetComponent<GridTile>());
        }
    }
    void CanLeft()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._downBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._leftBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._leftBlock._upBlock._blockOn )
        {
            //리스트에 업
            if(!_board.SLeftList.Contains(GetComponent<GridTile>()))
                _board.SLeftList.Add(GetComponent<GridTile>());
        }
        else
        {
            //리스트에서 해제
            if( _board.SLeftList.Contains(GetComponent<GridTile>()))
                _board.SLeftList.Remove(GetComponent<GridTile>());
        }
    }
}