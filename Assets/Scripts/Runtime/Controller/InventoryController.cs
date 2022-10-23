using System;
using System.Collections.Generic;
using Runtime.Enum;
using Runtime.Event;
using UnityEngine;

namespace Runtime.Controller
{
  public class InventoryController : MonoBehaviour
  {
    private Dictionary<CraftingComponentType, int> craftingComponentInventory;
    public IReadOnlyDictionary<CraftingComponentType, int> CraftingComponentInventory => craftingComponentInventory;

    private void Awake()
    {
      craftingComponentInventory = new Dictionary<CraftingComponentType, int>();

      foreach (CraftingComponentType key in System.Enum.GetValues(typeof(CraftingComponentType)))
      {
        if(key != CraftingComponentType.None)
          craftingComponentInventory.Add(key, 0);
      }
    }

    private void OnEnable()
    {
      InventoryUpdatedEvent.OnPublished += OnInventoryUpdated;
    }

    private void OnInventoryUpdated(InventoryUpdateEventArgs args)
    {
      craftingComponentInventory[args.CraftingComponentType] += args.Count;
      Debug.Log($"You have {craftingComponentInventory[args.CraftingComponentType]} {args.CraftingComponentType}");
    }
  }
}