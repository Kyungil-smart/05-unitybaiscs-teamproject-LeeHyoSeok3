using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTestCode : MonoBehaviour
{
    [SerializeField] GridTile tile;
    private void Update()
    {
        if (tile != GameObject.Find("boolBox").GetComponent<GridTile>())
        {
            Debug.Log("같지않다");
        }
        else if (tile == GameObject.Find("boolBox").GetComponent<GridTile>())
        {
            Debug.Log("같다");
        }
    }
}
