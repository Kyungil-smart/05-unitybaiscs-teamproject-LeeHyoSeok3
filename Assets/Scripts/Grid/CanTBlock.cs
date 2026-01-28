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
            if(!_board.TUpList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.TUpList.Add(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
        else
        {
            if(_board.TUpList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.TUpList.Remove(new Vector2Int((int)transform.position.x,(int)transform.position.z));
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
            if(!_board.TRightList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.TRightList.Add(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
        else
        {
            if(_board.TRightList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.TRightList.Remove(new Vector2Int((int)transform.position.x,(int)transform.position.z));
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
            if(!_board.TDownList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.TDownList.Add(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
        else
        {
            if(_board.TDownList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.TDownList.Remove(new Vector2Int((int)transform.position.x,(int)transform.position.z));
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
            if(!_board.TLeftList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.TLeftList.Add(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
        else
        {
            //리스트에서 해제
            if(_board.TLeftList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.TLeftList.Remove(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
    }
}