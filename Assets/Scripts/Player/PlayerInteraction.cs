using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform _holdPoint;
    [SerializeField] private float interactRange = 2f;
    
    private BlockControler _holding;
    
}