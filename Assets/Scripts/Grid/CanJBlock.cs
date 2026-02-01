using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanJBlock : MonoBehaviour
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
        if(!gameObject.GetComponent<GridTile>()._blockOn && 
           !gameObject.GetComponent<GridTile>()._upBlock._blockOn &&
           !gameObject.GetComponent<GridTile>()._downBlock._blockOn &&
           !gameObject.GetComponent<GridTile>()._leftBlock._downBlock._blockOn )
        {
            if(!_board.JUpList.Contains(gameObject.GetComponent<GridTile>()))
                _board.JUpList.Add(gameObject.GetComponent<GridTile>());
        }
        else
        {
            if( _board.JUpList.Contains(gameObject.GetComponent<GridTile>()))
                _board.JUpList.Remove(gameObject.GetComponent<GridTile>());
        }
    }
    void CanRight()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._rightBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._leftBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._upBlock._leftBlock._blockOn 
            )
        {
            if(!_board.JRightList.Contains(gameObject.GetComponent<GridTile>()))
                _board.JRightList.Add(gameObject.GetComponent<GridTile>());
        }
        else
        {
            if( _board.JRightList.Contains(gameObject.GetComponent<GridTile>()))
                _board.JRightList.Remove(gameObject.GetComponent<GridTile>());
        }
    }
    void CanDown()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._downBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._upBlock._rightBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._upBlock._blockOn )
        {
            if(!_board.JDownList.Contains(gameObject.GetComponent<GridTile>()))
                _board.JDownList.Add(gameObject.GetComponent<GridTile>());
        }
        else
        {
            if( _board.JDownList.Contains(gameObject.GetComponent<GridTile>()))
                _board.JDownList.Remove(gameObject.GetComponent<GridTile>());
        }
    }
    void CanLeft()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn &&
            !gameObject.GetComponent<GridTile>()._leftBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._rightBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._rightBlock._downBlock._blockOn )
        {
            //리스트에 업
            if(!_board.JLeftList.Contains(gameObject.GetComponent<GridTile>()))
                _board.JLeftList.Add(gameObject.GetComponent<GridTile>());
        }
        else
        {
            //리스트에서 해제
            if( _board.JLeftList.Contains(gameObject.GetComponent<GridTile>()))
                _board.JLeftList.Remove(gameObject.GetComponent<GridTile>());
        }
    }
}
