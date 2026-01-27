using System.Collections.Generic;
using UnityEngine;

public class BlockFactory
{
    private readonly ObjectPool<BlockView> _pool;
    private readonly float _blockSize;

    public BlockFactory(float blockSize)
    {
        _blockSize = blockSize;
        _pool = PoolManager.Instance.GetPool<BlockView>();
    }

    public List<BlockControler> Create(BlockType type, Vector2Int baseGrid)
    {
        Vector2Int[] shape = BlockShape.Shapes[type];
        List<BlockControler> blocks = new(shape.Length);
    
        foreach (var offset in shape)
        {
            Vector2Int grid = baseGrid + offset;
            BlockView view = _pool.Get();
            
            var controler = new BlockControler(view, grid, _blockSize);
            blocks.Add(controler);
        }

        return blocks;
    }

    public void Release(List<BlockControler> blocks)
    {
        foreach (var block in blocks)
            block.Release();
    }

}