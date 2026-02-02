using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GridTile : MonoBehaviour
{
    // 자신 블록 정보
    public bool _blockOn;
    public bool _predict;
    public Vector2Int GridPos { get; private set; }
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
        GridPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        Parents = null;
        _predict = false;
    }

    
    // 블록이 올라왔을 때 반응
    void OnTriggerEnter(Collider other)
    {
	    if(other.tag == "Block")
        {
            // Debug.Log(gameObject.name + " 타일 트리거");
            _blockOn = true;
            _predict = false;
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

    // public bool CanSetBlocks(Vector2Int position)
    // {
    //     return CanSetTargetBlock(position);
    //     
    //     // for(int i = 0; i < shape.Length; i++)
    //     // {
    //     //     if(CanSetTargetBlock(shape[i]))
    //     //         return false;
    //     // }
    //     // return true;
    // }

    public bool CanSetTargetBlock(Vector2Int position)
    {
        // _blockOn과 _predict가 모두 false일때 false
        if (!Vec2ChangeLink(position)._blockOn && !Vec2ChangeLink(position)._predict)
        {
            return false;
        }
        
        return true;        
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
        else if(xz.y < 0)
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
    public void Predict(BlockType blockType, int rotation)
    {
        Vector2Int[] arr = new Vector2Int[4];
        for (int i = 0; i < 4; i++)
        {
            arr[i] = BlockShape.Shapes[blockType][i];
        }
        arr[0] = Rotate(arr[0], rotation);
        arr[1] = Rotate(arr[1], rotation);
        arr[2] = Rotate(arr[2], rotation);
        arr[3] = Rotate(arr[3], rotation);

        foreach (Vector2Int vec in arr)
        {
            Vec2ChangeLink(vec)._predict = true;
        }
        
    }
    private Vector2Int Rotate(Vector2Int v, int rotation)
    {
        
        return rotation switch
        {
            0 => v,
            1 => new Vector2Int(-v.y,  v.x), // 90
            2 => new Vector2Int(-v.x, -v.y), // 180
            3 => new Vector2Int( v.y, -v.x), // 270
            _ => v
        };
    }

}
