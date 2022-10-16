using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Enum;
using Runtime.Terrain;
using UnityEngine;

namespace Runtime.Crafting
{
  public class CraftingRecipe : MonoBehaviour
  {
    public Dictionary<CraftingComponentType, int> CraftingComponents;

    private void Awake()
    {
      CraftingComponents = GetComponentsInChildren<CraftingComponent>().ToDictionary
      (
        c => c.ComponentType, 
        c => c.Count
      );
    }
  }
}