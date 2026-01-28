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
        if( gameObject.GetComponent<GridTile>()._blockOn && 
            gameObject.GetComponent<GridTile>()._rightBlock._blockOn &&
            gameObject.GetComponent<GridTile>()._upBlock._blockOn &&
            gameObject.GetComponent<GridTile>()._upBlock._upBlock._blockOn )
        {
            if(!_board.SUpList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.SUpList.Add(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
        else
        {
            if(_board.SUpList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.SUpList.Remove(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
    }
    void CanRight()
    {
        //가능성 판독
        if( gameObject.GetComponent<GridTile>()._blockOn && 
            gameObject.GetComponent<GridTile>()._leftBlock._blockOn &&
            gameObject.GetComponent<GridTile>()._downBlock._blockOn &&
            gameObject.GetComponent<GridTile>()._upBlock._leftBlock._blockOn )
        {
            if(!_board.SRightList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.SRightList.Add(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
        else
        {
            if(_board.SRightList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.SRightList.Remove(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
    }
    void CanDown()
    {
        //가능성 판독
        if( gameObject.GetComponent<GridTile>()._blockOn && 
            gameObject.GetComponent<GridTile>()._upBlock._blockOn &&
            gameObject.GetComponent<GridTile>()._leftBlock._blockOn &&
            gameObject.GetComponent<GridTile>()._upBlock._rightBlock._blockOn )
        {
            if(!_board.SDownList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.SDownList.Add(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
        else
        {
            if(_board.SDownList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.SDownList.Add(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
    }
    void CanLeft()
    {
        //가능성 판독
        if( gameObject.GetComponent<GridTile>()._blockOn && 
            gameObject.GetComponent<GridTile>()._upBlock._blockOn &&
            gameObject.GetComponent<GridTile>()._rightBlock._blockOn &&
            gameObject.GetComponent<GridTile>()._downBlock._rightBlock._blockOn )
        {
            //리스트에 업
            if(!_board.SLeftList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.SLeftList.Add(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
        else
        {
            //리스트에서 해제
            if(_board.SLeftList.Contains(new Vector2Int((int)transform.position.x,(int)transform.position.z)))
            _board.SLeftList.Add(new Vector2Int((int)transform.position.x,(int)transform.position.z));
        }
    }
}