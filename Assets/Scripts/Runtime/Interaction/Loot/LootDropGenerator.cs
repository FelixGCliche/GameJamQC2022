using Runtime.Enum;
using Runtime.Event;
using UnityEngine;

namespace Runtime.Interaction.Loot
{
  public class LootDropGenerator : MonoBehaviour
  {
    [SerializeField]
    private LootDropType lootDropType = LootDropType.None;

    [SerializeField]
    [Range(0.01f, 1f)]
    private float dropChance = 0.5f;

    [SerializeField]
    private Vector2Int minMax = new(1,1);

    public void GenerateDrop()
    {
      Debug.Log($"Generating {lootDropType}");
      var rnd = Random.value;
      
      Debug.Log($"{dropChance}, {rnd}");
      if (rnd >= dropChance) 
        return;
      
      var range = minMax.y - minMax.x;
      var count = minMax.x + Mathf.RoundToInt(range * rnd);
      
      Debug.Log($"{lootDropType} +{count}");
      
      InventoryUpdatedEvent.Publish(new InventoryUpdateEventArgs(lootDropType, count));
    }
  }
}