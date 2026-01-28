using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GridTile : MonoBehaviour
{
    // 자신 블록 정보
    public bool _blockOn;

    // 주변 블록 정보
    public GridTile _upBlock;
    public GridTile _downBlock;
    public GridTile _leftBlock;
    public GridTile _rightBlock;
    

    void Awake()
    {
        Init();
    }

    void Init()
    {
        _blockOn = false;
    }

    
    // 블록이 올라왔을 때 반응
    void OnTriggerEnter(Collider other)
    {
	    if(other.tag == "Block")
        {
            _blockOn = true;
        } // 자신의 블록 정보 수정
        
    }


    //블록이 사라졌을 떄 반응
    void OnTriggerExit(Collider other)
    {
	    if(other.tag == "Block")
        {
            _blockOn = false;
        } // 자신의 블록 정보 수정
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(transform.position, 0.2f);
    // }

    
}
