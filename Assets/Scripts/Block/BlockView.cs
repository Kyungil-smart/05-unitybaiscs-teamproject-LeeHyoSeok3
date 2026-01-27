using System;
using UnityEngine;

public class BlockView : MonoBehaviour
{
    private BlockControler _controler;

    public void Bind(BlockControler controler)
    {
        _controler = controler;
    }

    private void Update()
    {
        
    }

    public void DestroyView()
    {
        gameObject.SetActive(false);
    }
    
}