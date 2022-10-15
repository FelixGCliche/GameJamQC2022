using System;
using Runtime.Enum;
using UnityEngine;

namespace Runtime.Terrain
{
  public class TerrainBlock
  {
    private readonly TerrainGrid terrainGrid;
    
    private readonly Vector2Int gridPosition;
    public Vector2Int GridPosition => gridPosition;

    public TerrainType BlockType { get; }

    public TerrainEdge BlockEdges { get; }

    public TerrainBlock(TerrainGrid terrainGrid, Vector2Int gridPosition, TerrainType blockType, TerrainEdge blockEdges)
    {
      this.terrainGrid = terrainGrid;
      this.gridPosition = gridPosition;
      BlockType = blockType;
      BlockEdges = blockEdges;
    }

    public float Scale => terrainGrid.Scale;

    public bool IsEdge => BlockEdges != TerrainEdge.None;

    public Vector3 WorldPosition
    {
      get
      {
        var blockSize = terrainGrid.Scale;
        var blockHeight = BlockType switch
        {
          TerrainType.Grass => terrainGrid.GrassBlockHeight,
          TerrainType.Dirt => terrainGrid.DirtBlockHeight,
          _ => throw new ArgumentOutOfRangeException()
        };

        var localPosition = new Vector3(gridPosition.x * blockSize, blockHeight, gridPosition.y * blockSize);
        return localPosition + terrainGrid.transform.position - terrainGrid.WorldSize / 2f;
      }
    }
    
    public Vector3 Pivot
    {
      get
      {
        var halfBlockSize = terrainGrid.Scale / 2f;
        var blockCenterOffset = new Vector3(halfBlockSize, 0, halfBlockSize);
        return WorldPosition + blockCenterOffset;
      }
    }

    public float Height => BlockType switch
    {
      TerrainType.Grass => Scale * (1 + terrainGrid.GrassBlockHeight),
      TerrainType.Dirt => Scale * (1 + terrainGrid.DirtBlockHeight),
      _ => throw new ArgumentOutOfRangeException()
    };
  }
}