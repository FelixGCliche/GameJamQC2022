using System;
using Runtime.Enum;
using Runtime.Event;
using Runtime.Terrain;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Controller
{
  public class LootController : MonoBehaviour
  {
    [SerializeField]
    [Min(1)]
    private int tombstonePoolSize = 25; 
    
    [SerializeField]
    [Min(1)]
    private int pumpkinPoolSize = 25;
    
    [SerializeField]
    private Loot.Loot tombstonePrefab;
    
    [SerializeField]
    private Loot.Loot pumpkinPrefab;
    
    [SerializeField]
    private TerrainGrid terrainGrid;
    
    [SerializeField]
    private Transform lootContainer;

    private Loot.Loot[] tombstonePool;
    private Loot.Loot[] pumpkinPool;

    private void Awake()
    {
      lootContainer = transform;

      tombstonePool = new Loot.Loot[tombstonePoolSize];
      Populate(tombstonePool, tombstonePrefab);
      
      pumpkinPool = new Loot.Loot[pumpkinPoolSize];
      Populate(pumpkinPool, pumpkinPrefab);
    }

    private void OnEnable()
    {
      OnLootedEvent.OnPublished += OnLooted;
    }

    private void OnDisable()
    {
      OnLootedEvent.OnPublished -= OnLooted;
    }

    private void OnLooted(Loot.Loot loot)
    {
      switch (loot.LootType)
      {
        case LootType.Tombstone:
          MoveTombstone(loot);
          break;
        case LootType.Pumpkin:
          MovePumpkin(loot);
          break;
      }
    }

    public void Generate()
    {
      foreach (var tombstone in tombstonePool)
        MoveTombstone(tombstone);
      foreach (var pumpkin in pumpkinPool)
        MovePumpkin(pumpkin);
    }

    private void Populate(Loot.Loot[] pool, Loot.Loot prefab)
    {
      for (var i = 0; i < pool.Length; i++)
      {
        pool[i] = Instantiate(prefab);
        pool[i].transform.SetParent(lootContainer);
      }
    }

    private void MoveTombstone(Loot.Loot tombstone)
    {
      var block = terrainGrid.GetRandomDirtBlock();
      tombstone.MoveToBlock(block);
    }

    private void MovePumpkin(Loot.Loot pumpkin)
    {
      var block = terrainGrid.GetRandomGrassBlock();
      pumpkin.MoveToBlock(block);
    }
  }
}