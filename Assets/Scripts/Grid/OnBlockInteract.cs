using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBlockInteract : MonoBehaviour
{
    BlockView _onBlock {get; set;}
    public bool IsNeedClear;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Block")
        _onBlock = other.GetComponent<BlockView>();
    }

    public void ClearBlock()
    {
        if (_onBlock != null && IsNeedClear)
        {
            _onBlock.Controler.Release();
            _onBlock = null;
            IsNeedClear = false;
            gameObject.GetComponent<GridTile>()._blockOn = false;
        }
    }
    
}
