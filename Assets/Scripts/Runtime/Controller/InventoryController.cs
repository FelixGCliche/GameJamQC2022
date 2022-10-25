using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Enum;
using Runtime.Event;
using UI.Components.Inventory;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Controller
{
  public class InventoryController : MonoBehaviour
  {
    [SerializeField]
    private UIDocument document;

    private List<InventoryItem> inventoryUI;

    private Dictionary<CraftingComponentType, int> craftingComponentInventory;
    public IReadOnlyDictionary<CraftingComponentType, int> CraftingComponentInventory => craftingComponentInventory;

    private void Awake()
    {
      inventoryUI = document.rootVisualElement.Query<InventoryItem>().ToList();
      if (inventoryUI == null || inventoryUI.Count == 0)
        throw new NullReferenceException("Inventory Item list is null");
      else 
      inventoryUI.ForEach(i => Debug.Log($"Init {i.ComponentType} * {i.Count}"));
      
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
      Debug.Log($"Inventory Updated: {args.CraftingComponentType}");
      var inventoryItem = inventoryUI
        .FirstOrDefault(i=> i.ComponentType == args.CraftingComponentType);
      // if (inventoryItem == null) 
      //   return;
      
      craftingComponentInventory[args.CraftingComponentType] += args.Count;
      inventoryItem.Count += args.Count;
    }
  }
}