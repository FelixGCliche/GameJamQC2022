using System;
using System.Collections.Generic;
using Runtime.Enum;
using Runtime.Event;
using UnityEngine;

namespace Runtime.Controller
{
  public class InventoryController : MonoBehaviour
  {
    private Dictionary<LootDropType, int> lootDropInventory;
    public IReadOnlyDictionary<LootDropType, int> LootDropInventory => lootDropInventory;

    private void Awake()
    {
      lootDropInventory = new Dictionary<LootDropType, int>();

      foreach (LootDropType key in System.Enum.GetValues(typeof(LootDropType)))
      {
        if(key != LootDropType.None)
          lootDropInventory.Add(key, 0);
      }
    }

    private void OnEnable()
    {
      InventoryUpdatedEvent.OnPublished += OnInventoryUpdated;
    }

    private void OnInventoryUpdated(InventoryUpdateEventArgs args)
    {
      lootDropInventory[args.LootDropType] += args.Count;
      Debug.Log($"You have {lootDropInventory[args.LootDropType]} {args.LootDropType}");
    }
  }
}