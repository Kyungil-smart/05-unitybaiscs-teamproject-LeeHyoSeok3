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
            if(!_board.JUpList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
                _board.JUpList.Add(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
        else
        {
            if( _board.JUpList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
                _board.JUpList.Remove(new Vector2Int((int)transform.position.x,(int)transform.position.z));
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
            if(!_board.JRightList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.JRightList.Add(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
        else
        {
            if(_board.JRightList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.JRightList.Remove(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
    }
    void CanDown()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._downBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._downBlock._downBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._rightBlock._blockOn )
        {
            if(!_board.JDownList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.JDownList.Add(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
        else
        {
            if(_board.JDownList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.JDownList.Remove(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
    }
    void CanLeft()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn &&
            !gameObject.GetComponent<GridTile>()._leftBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._leftBlock._leftBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._downBlock._blockOn )
        {
            //리스트에 업
            if(!_board.JLeftList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.JLeftList.Add(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
        else
        {
            //리스트에서 해제
            if(_board.JLeftList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.JLeftList.Remove(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
    }
}
