using Runtime.Crafting;
using UnityEngine;

namespace Runtime.Controller
{
  public class CraftingController : MonoBehaviour
  {
    [SerializeField]
    private CraftingRecipe craftingRecipe;

    [SerializeField]
    private InventoryController inventoryController;

    public bool TryCraft()
    {
      foreach (var component in craftingRecipe.CraftingComponents)
      {
        var key = component.Key;
        var inventoryValue = inventoryController.CraftingComponentInventory[key];
        var count = component.Value;

        if (inventoryValue < count)
          return false;
      }

      return true;
    }
  }
}