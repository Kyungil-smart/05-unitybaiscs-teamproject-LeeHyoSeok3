using System.Collections.Generic;
using UnityEngine;

public class BlockFactory
{
    private readonly ObjectPool<BlockView>[] _pools;
    private readonly float _blockSize;

    public BlockFactory(float blockSize)
    {
        _blockSize = blockSize;
        int count = System.Enum.GetValues(typeof(BlockPoolType)).Length;
        _pools = new ObjectPool<BlockView>[count];
        for (int i = 0; i < count; i++) {
            _pools[i] = PoolManager.Instance.GetPool<BlockView>(i);
        }
        
        // _pool = PoolManager.Instance.GetPool<BlockView>();
    }

    public BlockGroup Create(BlockType type, BlockPoolType poolType, Vector2Int baseGrid)
    {
        Vector2Int[] shape = BlockShape.Shapes[type];
        List<BlockControler> blocks = new(shape.Length);
    
        foreach (var offset in shape)
        {
            // Vector2Int grid = baseGrid + offset;
            Vector2Int grid = offset;
            // BlockView view = _pool.Get();
            BlockView view = _pools[(int)poolType].Get();
            
            var controler = new BlockControler(view, grid, _blockSize, poolType);
            view.Initialize(controler);
            
            blocks.Add(controler);
        }

        return new BlockGroup(type, baseGrid, blocks);
    }

    public void Release(List<BlockControler> blocks)
    {
        foreach (var block in blocks)
            block.Release();
    }

}