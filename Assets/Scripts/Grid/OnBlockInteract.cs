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
            // Ŭ������ ���ο� �ִ� ���ϵ��� ��� ���·� �����ϱ� ���� ���� �׷� ����
            BlockGroup RemainBlocksGroup = _onBlock.Controler.Group;
            foreach(var block in RemainBlocksGroup.GetBlockList())
            {
                block.SetState(BlockState.Locked);
            }
            // ���� ���� ����
            _onBlock.Controler.Release();
            _onBlock = null;
            IsNeedClear = false;
            gameObject.GetComponent<GridTile>()._blockOn = false;
        }
    }
    
    // ������ ���� �׷��� �����ϱ� ���� ���� �� ��ȯ �޼���
    public BlockView GetBlockView()
    {
        return _onBlock;
    }

    void BlockStateCheck()
    {
        if(_onBlock.Controler.State == BlockState.Held)
        {
            gameObject.GetComponent<GridTile>()._blockOn = false;
            _onBlock = null;
        }
        // 헬드 이벤트 때 블럭 상태가 헬드이면 정보 갱신
    }
    void Nullact (HeldEvent evt)
    {
        if(_onBlock != null)
        {
            BlockStateCheck();
        }
    }

    void OnEnable()
    {
        GameEventBus.Subscribe<HeldEvent>(Nullact);
    }
    void OnDisable()
    {
        GameEventBus.Unsubscribe<HeldEvent>(Nullact);
    }
}
