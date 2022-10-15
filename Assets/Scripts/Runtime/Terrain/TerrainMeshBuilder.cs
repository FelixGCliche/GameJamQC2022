using System.Collections.Generic;
using Runtime.Enum;
using UnityEngine;

namespace Runtime.Terrain
{
  public class TerrainMeshBuilder
  {
    private readonly List<Vector3> vertices;
    private readonly List<int> indices;

    public TerrainMeshBuilder()
    {
      vertices = new List<Vector3>();
      indices = new List<int>();
    }

    public void Add(TerrainBlock block)
    {
      var position = block.WorldPosition;
      var scale = block.Scale;
      var height = block.Height;

      var edges = block.BlockEdges;

      var initialVerticesCount = vertices.Count;
      //Vertices
      // -- Top
      vertices.Add(new Vector3(position.x, position.y, position.z));
      vertices.Add(new Vector3(position.x + scale, position.y, position.z));
      vertices.Add(new Vector3(position.x, position.y, position.z + scale));
      vertices.Add(new Vector3(position.x + scale, position.y, position.z + scale));
      
      if (edges.HasFlag(TerrainEdge.North))
      {
        vertices.Add(new Vector3(position.x + scale, position.y, position.z));
        vertices.Add(new Vector3(position.x, position.y, position.z));
        vertices.Add(new Vector3(position.x + scale, position.y - height, position.z));
        vertices.Add(new Vector3(position.x, position.y - height, position.z));
      }
      // -- East
      if (edges.HasFlag(TerrainEdge.East))
      {
        vertices.Add(new Vector3(position.x + scale, position.y, position.z + scale));
        vertices.Add(new Vector3(position.x + scale, position.y, position.z));
        vertices.Add(new Vector3(position.x + scale, position.y - height, position.z + scale));
        vertices.Add(new Vector3(position.x + scale, position.y - height, position.z));
      }
      // -- South
      if (edges.HasFlag(TerrainEdge.South))
      {
        vertices.Add(new Vector3(position.x, position.y, position.z + scale));
        vertices.Add(new Vector3(position.x + scale, position.y, position.z + scale));
        vertices.Add(new Vector3(position.x, position.y - height, position.z + scale));
        vertices.Add(new Vector3(position.x + scale, position.y - height, position.z + scale));
      }

      // -- West
      if (edges.HasFlag(TerrainEdge.West))
      {
        vertices.Add(new Vector3(position.x, position.y, position.z));
        vertices.Add(new Vector3(position.x, position.y, position.z + scale));
        vertices.Add(new Vector3(position.x, position.y - height, position.z));
        vertices.Add(new Vector3(position.x, position.y - height, position.z + scale));
      }
      //Indices
      void AddIndices()
      {
        indices.Add(2 + initialVerticesCount);
        indices.Add(1 + initialVerticesCount);
        indices.Add(0 + initialVerticesCount);
        indices.Add(2 + initialVerticesCount);
        indices.Add(3 + initialVerticesCount);
        indices.Add(1 + initialVerticesCount);
      }
      
      // -- Top
      AddIndices();

      // -- North
      if (edges.HasFlag(TerrainEdge.North))
      {
        initialVerticesCount += 4;
        AddIndices();
      }

      // -- East
      if (edges.HasFlag(TerrainEdge.East))
      {
        initialVerticesCount += 4;
        AddIndices();
      }

      // -- South
      if (edges.HasFlag(TerrainEdge.South))
      {
        initialVerticesCount += 4;
        AddIndices();
      }

      // -- East
      if (edges.HasFlag(TerrainEdge.West))
      {
        initialVerticesCount += 4;
        AddIndices();
      }
    }
    
    public void Build(Mesh target)
    {
      target.vertices = vertices.ToArray();
      target.triangles = indices.ToArray();

      target.RecalculateNormals();
      target.Optimize();
    }
  }
}