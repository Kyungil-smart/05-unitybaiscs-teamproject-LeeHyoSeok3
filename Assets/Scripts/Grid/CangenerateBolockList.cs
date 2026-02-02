using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CangenerateBolockList : MonoBehaviour
{
    
    public List<GridTile> Grids;
    public List<GridTile> JUpList;
    public List<GridTile> JRightList;
    public List<GridTile> JDownList;
    public List<GridTile> JLeftList;
    public List<GridTile> LUpList;
    public List<GridTile> LRightList;
    public List<GridTile> LDownList;
    public List<GridTile> LLeftList;
    public List<GridTile> OUpList;
    public List<GridTile> ORightList;
    public List<GridTile> ODownList;
    public List<GridTile> OLeftList;
    public List<GridTile> TUpList;
    public List<GridTile> TRightList;
    public List<GridTile> TDownList;
    public List<GridTile> TLeftList;
    public List<GridTile> SUpList;
    public List<GridTile> SRightList;
    public List<GridTile> SDownList;
    public List<GridTile> SLeftList;
    public List<GridTile> ZUpList;
    public List<GridTile> ZRightList;
    public List<GridTile> ZDownList;
    public List<GridTile> ZLeftList;
    public List<GridTile> IUpList;
    public List<GridTile> IRightList;
    public List<GridTile> IDownList;
    public List<GridTile> ILeftList;
    public List<GridTile> ObList;

    void Awake()
    {
    
        ObList = new List<GridTile>(Grids.Count);
        GetNonBlockList();
        
        
        JUpList = new List<GridTile>();
        JRightList = new List<GridTile>();
        JDownList = new List<GridTile>();
        JLeftList = new List<GridTile>();
        LUpList = new List<GridTile>();
        LRightList = new List<GridTile>();
        LDownList = new List<GridTile>();
        LLeftList = new List<GridTile>();
        OUpList = new List<GridTile>();
        ORightList = new List<GridTile>();
        ODownList = new List<GridTile>();
        OLeftList = new List<GridTile>();
        TUpList = new List<GridTile>();
        TRightList = new List<GridTile>();
        TDownList = new List<GridTile>();
        TLeftList = new List<GridTile>();
        SUpList = new List<GridTile>();
        SRightList = new List<GridTile>();
        SDownList = new List<GridTile>();
        SLeftList = new List<GridTile>();
        ZUpList = new List<GridTile>();
        ZRightList = new List<GridTile>();
        ZDownList = new List<GridTile>();
        ZLeftList = new List<GridTile>();
        IUpList = new List<GridTile>();
        IRightList = new List<GridTile>();
        IDownList = new List<GridTile>();
        ILeftList = new List<GridTile>();
    
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
        GetNonBlockList();
    }
    void GetNonBlockList()
    {
        ObList.Clear();
        foreach(GridTile grid in Grids)
        {
            if(!grid._blockOn)
            {
                ObList.Add(grid);
            }
        }
    }
}
