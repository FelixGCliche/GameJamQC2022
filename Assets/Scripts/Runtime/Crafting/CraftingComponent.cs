using Runtime.Enum;
using UnityEngine;

namespace Runtime.Crafting
{
  public class CraftingComponent : MonoBehaviour
  {
    [SerializeField]
    private CraftingComponentType componentType;
    public CraftingComponentType ComponentType => componentType;

    [SerializeField]
    [Range(1, 10)]
    private int count;
    public int Count => count;
  }
}