using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlockControler
{
    public BlockType Type { get; }
    public BlockState State { get; protected set; }

    protected BlockControler(BlockType type)
    {
        Type = type;
        State = BlockState.Idle;
    }

    public virtual void OnSpawn()
    {
        State = BlockState.Falling;
    }

    public virtual void OnHit()
    {
        State = BlockState.Destroyed;
    }
    
}
