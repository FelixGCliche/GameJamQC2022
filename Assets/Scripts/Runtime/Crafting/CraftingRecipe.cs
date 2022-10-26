using System.Collections.Generic;
using System.Linq;
using Runtime.Controller;
using Runtime.Enum;
using Runtime.Event;
using Runtime.Interaction;
using UnityEngine;

namespace Runtime.Crafting
{
  public class CraftingRecipe : MonoBehaviour, IInteractable
  {
    [SerializeField]
    private InventoryController inventoryController;

    [SerializeField]
    [Range(0.1f, 5f)]
    [Tooltip("Time it takes to craft recipe, in seconds")]
    private float craftDuration = 1f;

    private bool isCrafting;
    private float elapsedTime;
    private bool CraftignComplete => elapsedTime >= craftDuration;
    private IReadOnlyDictionary<CraftingComponentType, int> craftingComponents;

    private void Awake()
    {
      craftingComponents = GetComponentsInChildren<CraftingComponent>().ToDictionary
      (
        c => c.ComponentType, 
        c => c.Count
      );
    }

    private void Update()
    {
      if(!isCrafting)
        return;
      elapsedTime += Time.deltaTime / craftDuration;
      if(CraftignComplete)
        OnCraft();
    }

    public void OnInteractStarted()
    {
      isCrafting = true;
    }

    public void OnInteractFinished()
    {
      if(!CraftignComplete)
        OnInteractCanceled();
    }

    public void OnInteractCanceled()
    {
      ResetInteraction();
    }
    private void OnCraft()
    {
      foreach (var (key, count) in craftingComponents)
      {
        var inventoryValue = inventoryController.CraftingComponentInventory[key];
        if (inventoryValue < count)
          return;
      }
      foreach (var (key, count) in craftingComponents)
        InventoryUpdatedEvent.Publish(new InventoryUpdateEventArgs(key, -count));
      
      ResetInteraction();
    }

    private void ResetInteraction()
    {
      elapsedTime = 0f;
      isCrafting = false;
    }
  }
}