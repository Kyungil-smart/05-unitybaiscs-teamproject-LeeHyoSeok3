using System.Collections.Generic;
using UnityEngine;

public class BlockFactory
{
    private readonly ObjectPool<BlockView>[] _blockPools;
    private readonly ObjectPool<GhostBlock>[] _ghostPools;

    private readonly float _blockSize;

    public BlockFactory(float blockSize)
    {
        _blockSize = blockSize;
        int count = System.Enum.GetValues(typeof(BlockPoolType)).Length;
        _blockPools = new ObjectPool<BlockView>[count];
        _ghostPools = new ObjectPool<GhostBlock>[count];

        for (int i = 0; i < count; i++)
        {
            _blockPools[i] = PoolManager.Instance.GetPool<BlockView>(i);
            _ghostPools[i] = PoolManager.Instance.GetPool<GhostBlock>(i);
        }
    }

    public BlockGroup Create(BlockType type, BlockPoolType poolType, Vector2Int baseGrid, float dropY)
    {
        Vector2Int[] shape = BlockShape.Shapes[type];
        List<BlockControler> blocks = new(shape.Length);

        foreach (var offset in shape)
        {
            // Vector2Int grid = baseGrid + offset;
            Vector2Int grid = offset;
            // BlockView view = _pool.Get();
            BlockView view = _blockPools[(int)poolType].Get();

            var controler = new BlockControler(view, grid, dropY, _blockSize, poolType);
            view.Initialize(controler);

            blocks.Add(controler);
        }

        return new BlockGroup(type, poolType, baseGrid, blocks);
    }

    public void Release(List<BlockControler> blocks)
    {
        foreach (var block in blocks)
            block.Release();
    }
}