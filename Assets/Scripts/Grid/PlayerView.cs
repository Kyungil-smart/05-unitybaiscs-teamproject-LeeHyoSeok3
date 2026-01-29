using System;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private PlayerControler _controler;
    private PlayerMovement _movement;

    public void Initialize(PlayerControler controler)
    {
        _controler = controler;
        _movement = new PlayerMovement();
    }

    private void Update()
    {
        
    }
}