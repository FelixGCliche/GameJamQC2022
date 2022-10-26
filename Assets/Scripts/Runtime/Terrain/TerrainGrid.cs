using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Enum;
using Runtime.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Terrain
{
  public class TerrainGrid : MonoBehaviour
  {
    private const string TerrainRootName = "TerrainMeshes";
    private const string GrassTerrainName = "Grass";
    private const string DirtTerrainName = "Dirt";
    private const string TerrainBaseName = "Base";
    
    [SerializeField] 
    [Range(1, 10)]
    private float scale = 1f;
    
    [SerializeField] 
    private Material baseMaterial = null;
    
    [Header("Grass")] 
    [SerializeField] 
    private Material grassMaterial = null;
    [SerializeField] 
    [Range(0.1f, 1f)]
    private float grassBlockHeight = 0.3f;
    
    [Header("Dirt")] 
    [SerializeField] 
    private Material dirtMaterial = null;
    [SerializeField] 
    [Range(0.1f, 1f)]
    private float dirtBlockHeight = 0.2f; 

    private GameObject terrainRoot;
    private GameObject terrainBase;
    private Mesh grassMesh;
    private Mesh dirtMesh;
    private TerrainBlock[,] blocks;

    private void Awake()
    {
      terrainRoot = CreateTerrainRoot(TerrainRootName);
      terrainBase = CreateTerrainBaseMesh(TerrainBaseName);
      grassMesh = CreateEmptyTerrainMesh(GrassTerrainName, grassMaterial);
      dirtMesh = CreateEmptyTerrainMesh(DirtTerrainName, dirtMaterial);
    }

    private void OnEnable()
    {
      terrainRoot.SetActive(true);
    }

    private void OnDisable()
    {
      terrainRoot.SetActive(false);
    }

    public float Scale => scale;
    public float GrassBlockHeight => grassBlockHeight;
    public float DirtBlockHeight => dirtBlockHeight;
    public Vector2Int GridSize => new(blocks.GetLength(0), blocks.GetLength(1));
    public Vector3 WorldSize => new(GridSize.x * scale, scale, GridSize.y * scale);

    public TerrainBlock[,] Blocks
    {
      get => blocks;
      set
      {
        blocks = value;
        UpdateTerrainMesh();
      }
    }

    public IEnumerable<TerrainBlock> GrassBlocks => EnumerateBlockType(TerrainType.Grass);
    public IEnumerable<TerrainBlock> DirtBlocks => EnumerateBlockType(TerrainType.Dirt);

    public TerrainBlock GetRandomGrassBlock()
    {
      var grass = GrassBlocks.ToArray();
      var index = Mathf.FloorToInt(grass.Length * Random.value);
      return grass[index];
    }

    public TerrainBlock GetRandomDirtBlock()
    {
      var dirt = DirtBlocks.ToArray();
      var index = Mathf.FloorToInt(dirt.Length * Random.value);
      return dirt[index];
    }

    private Mesh CreateEmptyTerrainMesh(string meshName, Material meshMaterial)
    {
      var meshGameObject = new GameObject
      {
        name = meshName,
        layer = Layers.Terrain
      };

      var meshTransform = meshGameObject.transform;
      meshTransform.parent = terrainRoot.transform;

      var mesh = new Mesh {name = meshName};
      var meshFilter = meshGameObject.AddComponent<MeshFilter>();
      meshFilter.mesh = mesh;

      var meshRenderer = meshGameObject.AddComponent<MeshRenderer>();
      meshRenderer.material = meshMaterial;

      meshGameObject.AddComponent<MeshCollider>();
      

      return mesh;
    }

    private GameObject CreateTerrainBaseMesh(string meshName)
    {
      var meshGameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
      meshGameObject.layer = Layers.Terrain;
      meshGameObject.name = meshName;
      meshGameObject.transform.parent = terrainRoot.transform;
      meshGameObject.transform.Rotate(-90, 0, 0);
      meshGameObject.GetComponent<MeshRenderer>().material = baseMaterial;
      return meshGameObject;
    }
    private void UpdateTerrainMesh()
    {
      grassMesh.Clear();
      dirtMesh.Clear();
      terrainBase.SetActive(false);

      if (blocks == null) 
        return;
      
      var gridSize = GridSize;
      var grassMeshBuilder = new TerrainMeshBuilder();
      var dirtMeshBuilder = new TerrainMeshBuilder();

      for (var x = 0; x < gridSize.x; x++)
      {
        for (var y = 0; y < gridSize.y; y++)
        {
          var block = blocks[x, y];

          var meshBuilder = block.BlockType switch
          {
            TerrainType.Grass => grassMeshBuilder,
            TerrainType.Dirt => dirtMeshBuilder,
            _ => throw new ArgumentOutOfRangeException()
          };

          meshBuilder.Add(block);
        }
      }

      grassMeshBuilder.Build(grassMesh);
      dirtMeshBuilder.Build(dirtMesh);

      var worldSize = WorldSize;
      terrainBase.transform.localScale = new Vector3(worldSize.x, worldSize.z, 1);
      terrainBase.transform.localPosition = new Vector3(0, -scale - grassBlockHeight - dirtBlockHeight, 0);
      terrainBase.SetActive(isActiveAndEnabled);
    }
    private IEnumerable<TerrainBlock> EnumerateBlockType(TerrainType terrainType)
    {
      return blocks.Cast<TerrainBlock>()
        .Where(terrainBlock => terrainBlock.BlockType == terrainType);
    }
    private GameObject CreateTerrainRoot(string gameObjectName)
    {
      var rootGameObject = new GameObject(gameObjectName);
      rootGameObject.transform.parent = transform;
      return rootGameObject;
    }
  }
}