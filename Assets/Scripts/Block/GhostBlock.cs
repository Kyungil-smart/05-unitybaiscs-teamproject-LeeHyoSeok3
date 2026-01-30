using UnityEngine;

public class GhostBlock : MonoBehaviour, IPoolable
{
    [SerializeField] private Renderer _renderer;

    private MaterialPropertyBlock _mpb;
    private static readonly int BaseColorId =
        Shader.PropertyToID("_BaseColor"); // Built-in이면 "_Color"

    private void Awake()
    {
        _mpb = new MaterialPropertyBlock();
    }
    
    // IPoolable
    public void OnSpawn()
    {
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        gameObject.SetActive(false);
    }
    
}