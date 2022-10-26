using Runtime.Enum;
using Runtime.Utils;
using UnityEngine;

namespace Runtime.Terrain
{
  public class TerrainGenerator : MonoBehaviour
  {
    [Header("Terrain")] 
    [SerializeField] 
    [Range(1, 1000)]
    private int width = 100;
    
    [SerializeField] 
    [Range(1, 1000)]
    private int height = 100;

    [SerializeField]
    [Range(0.1f, 100.0f)]
    private float scale = 1f ;
    
    [Header("Dirt")] 
    [Range(0, 1)] 
    [SerializeField] 
    private float dirtMaxHeight = 0.5f;

    [SerializeField]
    private TerrainGrid terrainGrid;

    public void Generate()
    {
      var noiseMap = PerlinNoise.Map(width, height, scale);
      var terrainMap = new TerrainType[width, height];

      for (var x = 0; x < width; x++)
      {
        for (var y = 0; y < height; y++)
        {
          if (noiseMap[x, y] < dirtMaxHeight)
            terrainMap[x, y] = TerrainType.Dirt;
          else
            terrainMap[x, y] = TerrainType.Grass;
        }
      }

      var terrainBlocks = new TerrainBlock[width, height];
      for (var x = 0; x < width; x++)
      {
        for (var y = 0; y < height; y++)
        {
          var position = new Vector2Int(x, y);
    
          var terrainType = terrainMap[x, y];
    
          var terrainEdges = TerrainEdge.None;
          if (y == 0 || terrainMap[x, y - 1] != terrainType)
            terrainEdges |= TerrainEdge.North;
          if (x == width - 1 || terrainMap[x + 1, y] != terrainType)
            terrainEdges |= TerrainEdge.East;
          if (y == height - 1 || terrainMap[x, y + 1] != terrainType)
            terrainEdges |= TerrainEdge.South;
          if (x == 0 || terrainMap[x - 1, y] != terrainType)
            terrainEdges |= TerrainEdge.West;
    
          terrainBlocks[x, y] = new TerrainBlock(terrainGrid, position, terrainType, terrainEdges);
        }
      }
    
      terrainGrid.Blocks = terrainBlocks;
    }
  }
}