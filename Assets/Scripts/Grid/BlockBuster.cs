using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBuster : MonoBehaviour
{
    // [SerializeField] OnBlockInteract[] _blocks;
    [SerializeField] LineCheker[] _check;

    private Coroutine _clearCoroutine;

    void OnEnable()
    {
        GameEventBus.Subscribe<GridUpdateEvent>(NullAct);
    }
    void OnDisable()
    {
        GameEventBus.Unsubscribe<GridUpdateEvent>(NullAct);
    }
    void NullAct(GridUpdateEvent evt)
    {
        ClearLineAction();
    }

    

    public void ClearLineAction()
    {
        if(FullLine() != 0)
        {
            GameEventBus.Raise(new LineClearedEvent(FullLine()));
            Debug.Log($"{FullLine()} 라인 클리어 발행");
            LineClear();
            Debug.Log("재 업데이트");
            GameEventBus.Raise(new GridUpdateEvent());
            
        }
        
    }

    int FullLine()
    {
        int _line = 0;
        foreach(LineCheker _l in _check)
        {
            if(_l.IsLineFull()) _line++;
        }
        return _line;
    }

    
    void LineClear()
    {
        foreach (LineCheker _l in _check)
        {
            foreach(GridTile grid in _l._tielOnLine)
            {
                grid.GetComponent<OnBlockInteract>().ClearBlock();
            }
        }
        
    }
   
}
