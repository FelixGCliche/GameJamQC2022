using System;
using Runtime.Enum;
using UnityEngine;

namespace Runtime.Event
{
    public struct InventoryUpdateEventArgs
    {
      public CraftingComponentType CraftingComponentType { get; private set; }
      public int Count { get; private set; }

      public InventoryUpdateEventArgs(CraftingComponentType craftingComponentType, int count)
      {
        CraftingComponentType = craftingComponentType;
        Count = count;
      }
    }

    public static class InventoryUpdatedEvent
    {
      public static Action<InventoryUpdateEventArgs> OnPublished;

      public static void Publish(InventoryUpdateEventArgs args) =>
        OnPublished?.Invoke(args);
    }
  
}