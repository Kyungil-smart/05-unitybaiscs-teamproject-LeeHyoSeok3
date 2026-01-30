using UnityEngine;

public class GhostBlock
{
    public Transform Transform { get; private set; }

    public GhostBlock(BlockView orighin, Transform parent, Material material)
    {
        Transform = Object.Instantiate(orighin.gameObject, parent).transform;
        var rednerer = Transform.GetComponentInChildren<Renderer>();
        rednerer.material = material;
    }
    
}