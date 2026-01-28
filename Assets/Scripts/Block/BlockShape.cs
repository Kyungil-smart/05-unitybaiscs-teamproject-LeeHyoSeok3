using System.Collections.Generic;
using UnityEngine;

// XZ 좌표계 기준
public static class BlockShape
{
    public static readonly Dictionary<BlockType, Vector2Int[]> Shapes
        = new()
        {
            {
                BlockType.I,
                new[]
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(1, 0),
                    new Vector2Int(2, 0),
                    new Vector2Int(3, 0),
                }
            },
            {
                BlockType.O,
                new[]
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(1, 0),
                    new Vector2Int(0, 1),
                    new Vector2Int(1, 1),
                }
            },
            {
                BlockType.T,
                new[]
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(-1, 0),
                    new Vector2Int(1, 0),
                    new Vector2Int(0, 1),
                }
            },
            {
                BlockType.Z,
                new[]
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(1, 0),
                    new Vector2Int(0, 1),
                    new Vector2Int(-1, 1),
                }
            },
            {
                BlockType.S,
                new[]
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(-1, 0),
                    new Vector2Int(0, 1),
                    new Vector2Int(1, 1),
                }
            },
            {
                BlockType.J,
                new[]
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(0, 1),
                    new Vector2Int(0, 2),
                    new Vector2Int(-1, 0),
                }
            },
            {
                BlockType.L,
                new[]
                {
                    new Vector2Int(0, 0),
                    new Vector2Int(0, 1),
                    new Vector2Int(0, 2),
                    new Vector2Int(1, 0),
                }
            },
        };
}