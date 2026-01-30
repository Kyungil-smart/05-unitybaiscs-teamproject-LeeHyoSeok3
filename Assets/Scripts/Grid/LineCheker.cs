using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LineCheker : MonoBehaviour
{
    public GridTile[] _tielOnLine; // 라인에 있는 타일 그리드 


    void Awake()
    {
       
    }
    
    public bool IsLineFull()  // 라인에 전부 블록이 있는지 체크
    {
        foreach (GridTile boxON in _tielOnLine)
        {
            if(!boxON._blockOn)
            {
                return false;
            }
        }
        foreach(GridTile boxON in _tielOnLine)
        {
            boxON.GetComponent<OnBlockInteract>().IsNeedClear = true;
        }
        return true;
    }
    
}
