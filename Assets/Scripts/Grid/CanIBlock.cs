using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanIBlock : MonoBehaviour
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
            !gameObject.GetComponent<GridTile>()._rightBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._rightBlock._rightBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._rightBlock._rightBlock._rightBlock._blockOn )
        {
            if(!_board.IUpList.Contains(gameObject.GetComponent<GridTile>()))
                _board.IUpList.Add(gameObject.GetComponent<GridTile>());            
        }
        else
        {
            if( _board.IUpList.Contains(gameObject.GetComponent<GridTile>()))
                _board.IUpList.Remove(gameObject.GetComponent<GridTile>());
        }
        
    }
    void CanRight()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._downBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._downBlock._downBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._downBlock._downBlock._downBlock._blockOn )
        {
            if(!_board.IRightList.Contains(gameObject.GetComponent<GridTile>()))
                _board.IRightList.Add(gameObject.GetComponent<GridTile>());
        }
        else
        {
            if( _board.IRightList.Contains(gameObject.GetComponent<GridTile>()))
                _board.IRightList.Remove(gameObject.GetComponent<GridTile>());
        }
    }
    void CanDown()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._leftBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._leftBlock._leftBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._leftBlock._leftBlock._leftBlock._blockOn )
        {
            if(!_board.IDownList.Contains(gameObject.GetComponent<GridTile>()))
                _board.IDownList.Add(gameObject.GetComponent<GridTile>());
        }
        else
        {
            if( _board.IDownList.Contains(gameObject.GetComponent<GridTile>()))
                _board.IDownList.Remove(gameObject.GetComponent<GridTile>());
        }
    }
    void CanLeft()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._upBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._upBlock._upBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._upBlock._upBlock._upBlock._blockOn )
        {
            //리스트에 업
            if(!_board.ILeftList.Contains(gameObject.GetComponent<GridTile>()))
                _board.ILeftList.Add(gameObject.GetComponent<GridTile>());
        }
        else
        {
            //리스트에서 해제
            if( _board.ILeftList.Contains(gameObject.GetComponent<GridTile>()))
                _board.ILeftList.Remove(gameObject.GetComponent<GridTile>());
        }
    }
}
