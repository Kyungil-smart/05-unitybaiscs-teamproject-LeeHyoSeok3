using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform _holdPoint;
    [SerializeField] private float interactRange = 2f;
    
    public BlockControler HoldingBlock {get; private set;}

    public void TryPickUp()
    {
        if (!Physics.Raycast(
                transform.position + new Vector3(0, 0.3f, 0),
                transform.forward,
                out RaycastHit hit,
                interactRange)) return;

        if (!hit.collider.TryGetComponent(out BlockView view))
            return;

        BlockControler block = view.Controler;
        if (block == null || !block.IsMoveable())
            return;
        
        HoldingBlock = block;
        block.PickUp(_holdPoint);
    }

    public void Drop()
    {
        if (HoldingBlock == null) return;
        Vector2Int dropPosition = new Vector2Int((int)_holdPoint.position.x, (int)_holdPoint.position.z);
        
        HoldingBlock.Drop(dropPosition, _holdPoint.position.y);
        HoldingBlock = null;
    }

    private void OnDrawGizmos()
    {

        Vector3 origin = transform.position + new Vector3(0, 0.3f, 0);
        Vector3 dir = transform.forward;
        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(origin, dir *  interactRange);
    }
}