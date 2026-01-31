using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // [SerializeField] private Transform _holdPoint;
    [SerializeField] private HeldPointDetector _heldPoint;
    [SerializeField] private float interactRange = 0.5f;
    [SerializeField] private float _dropY;

    public BlockView LookingView { get; private set; }

    public BlockGroup HoldingGroup { get; private set; }
    public BlockGroup LookingGroup { get; private set; }

    private void LateUpdate()
    {
        if (HoldingGroup != null)
            HoldingGroup.FollowHeld();
    }

    public void InteractableBlockUpdate()
    {
        Vector3 origin = transform.position + Vector3.up * 0.3f;
        Vector3 direction = transform.forward;

        RaycastHit[] hits = Physics.RaycastAll(origin, direction, interactRange);

        BlockView closestView = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            if (!hit.collider.TryGetComponent(out BlockView view))
                continue;

            if (hit.distance < closestDistance)
            {
                closestDistance = hit.distance;
                closestView = view;
            }
        }

        // 아무 것도 안 보고 있을 때
        if (closestView == null)
        {
            if (LookingGroup != null)
                LookingGroup.HideOutline();

            LookingView = null;
            LookingGroup = null;
            return;
        }

        // 같은 그룹을 계속 보고 있으면 처리 안 함
        if (LookingView == closestView)
            return;

        // 이전 아웃라인 제거
        if (LookingGroup != null)
            LookingGroup.HideOutline();

        LookingView = closestView;
        LookingGroup = LookingView.Controler.Group;

        if (LookingGroup != null)
            LookingGroup.SetOutline(Color.red);
    }

    public void PickUpBlock()
    {
        if(LookingGroup == null || !(LookingGroup.IsInteract()))
            return;

        HoldingGroup = LookingGroup;
        LookingGroup = null;
        HoldingGroup.PickUp(_heldPoint, _dropY);
        HoldingGroup.SetOutline(Color.blue);
    }

    public void DropBlock()
    {
        if (HoldingGroup == null)
            return;

        HoldingGroup.Drop();
        HoldingGroup.HideOutline();
        HoldingGroup = null;
    }

    public void RotateBlock(float? rotateInput)
    {
        bool clockwise = rotateInput > 0f;
        HoldingGroup.Rotate(clockwise);
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position + new Vector3(0f, 0.3f, 0f);
        Vector3 dir = transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(origin, dir * interactRange);
    }
}