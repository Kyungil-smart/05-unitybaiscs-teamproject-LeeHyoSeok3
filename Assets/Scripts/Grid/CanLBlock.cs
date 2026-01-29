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
            if(!_board.LUpList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.LUpList.Add(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
        else
        {
            if(_board.LUpList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.LUpList.Remove(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
    }
    void CanRight()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._rightBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._rightBlock._rightBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._downBlock._blockOn )
        {
            if(!_board.LRightList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.LRightList.Add(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
        else
        {
            if(_board.LRightList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.LRightList.Remove(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
    }
    void CanDown()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._downBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._downBlock._downBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._leftBlock._blockOn )
        {
            if(!_board.LDownList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.LDownList.Add(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
        else
        {
            if(_board.LDownList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.LDownList.Remove(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
    }
    void CanLeft()
    {
        //가능성 판독
        if( !gameObject.GetComponent<GridTile>()._blockOn && 
            !gameObject.GetComponent<GridTile>()._leftBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._leftBlock._leftBlock._blockOn &&
            !gameObject.GetComponent<GridTile>()._upBlock._blockOn )
        {
            //리스트에 업
            if(!_board.LLeftList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.LLeftList.Add(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
        else
        {
            //리스트에서 해제
            if(_board.LLeftList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.LLeftList.Remove(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
    }
}
