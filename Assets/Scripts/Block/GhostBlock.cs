using UnityEngine;

public class GhostBlock : MonoBehaviour, IPoolable
{
    private Renderer _renderer;
    private MaterialPropertyBlock _mpb;

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _mpb = new MaterialPropertyBlock();
    }

    public void SetColor(Color color)
    {
        _mpb.SetColor("_BaseColor", color);
        _renderer.SetPropertyBlock(_mpb);
    }

    // IPoolable
    public void OnSpawn()
    {
        gameObject.SetActive(true);
    }

    public void OnRelease()
    {
        gameObject.SetActive(false);
    }
    
}