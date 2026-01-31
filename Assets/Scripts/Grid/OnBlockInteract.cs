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
        if(_onBlock != null && IsNeedClear)
        {
            // 클리어할 라인에 있는 블록들을 잠금 상태로 변경하기 위한 블록 그룹 참조
            BlockGroup RemainBlocksGroup = _onBlock.Controler.Group;
            foreach(var block in RemainBlocksGroup.GetBlockList())
            {
                block.SetState(BlockState.Locked);
            }
            // 기존 삭제 로직
            _onBlock.Controler.Release();
            _onBlock = null;
            IsNeedClear = false;
            gameObject.GetComponent<GridTile>()._blockOn = false;
        }
    }
    
    // 삭제할 블록 그룹을 참조하기 위한 블록 뷰 반환 메서드
    public BlockView GetBlockView()
    {
        return _onBlock;
    }
}
