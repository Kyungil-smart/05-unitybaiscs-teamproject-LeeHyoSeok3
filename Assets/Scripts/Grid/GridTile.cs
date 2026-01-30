using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GridTile : MonoBehaviour
{
    // 자신 블록 정보
    public bool _blockOn;

    public int _g;
    public int _h;
    public int _f;
    public GridTile Parents;

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
        if(!_blockOn) _blockOn = false;
        Parents = null;
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

    public bool CanSetBlocks(Vector2Int[] shape)
    {
        for(int i = 0; i < shape.Length; i++)
        {
            if(!CanSetTargetBlock(shape[i]))
                return false;
        }
        return true;
    }

    bool CanSetTargetBlock(Vector2Int position)
    {
        return Vec2ChangeLink(position)._blockOn;        
    }
    GridTile Vec2ChangeLink(Vector2Int xz)
    {
        GridTile _current = this;

        // x좌표 이동
        // 음수 판정
        if(xz.x > 0)
        {
            for(int i = 0; i < xz.x; i++ )
            {
                _current = _current._rightBlock;
            }
        }
        else if(xz.x < 0)
        {
            for(int i = 0; i < (xz.x * -1); i++ )
            {
                _current = _current._leftBlock;
            }
        }
        // y좌표 이동
        if(xz.y > 0)
        {
            for(int i = 0; i < xz.y; i++ )
            {
                _current = _current._upBlock;
            }
        }
        else if(xz.x < 0)
        {
            for(int i = 0; i < (xz.y * -1); i++ )
            {
                _current = _current._downBlock;
            }
        }

        return _current;
    }
    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(transform.position, 0.2f);
    // }

}
