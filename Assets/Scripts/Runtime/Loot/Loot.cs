using Runtime.Enum;
using Runtime.Event;
using Runtime.Interaction;
using Runtime.Terrain;
using UnityEngine;

namespace Runtime.Loot
{
  public class Loot : MonoBehaviour, IInteractable
  {
    [SerializeField]
    private LootType lootType = LootType.None;
    public LootType LootType => lootType;

    [SerializeField]
    [Range(0.1f, 5f)]
    [Tooltip("Time it takes to gather item, in seconds")]
    private float lootDuration = 1f;

    private float elapsedTime;
    private bool isLooting;
    private LootDropGenerator[] lootDrops;
    private bool LootingComplete => elapsedTime >= lootDuration;

    private void Awake()
    {
      lootDrops = GetComponentsInChildren<LootDropGenerator>();
      ResetInteraction();
    }

    private void Update()
    {
      if (!isLooting) 
        return;
      elapsedTime += Time.deltaTime / lootDuration;
      if(LootingComplete)
        OnLoot();
    }

    public void OnInteractStarted()
    {
      isLooting = true;
    }

    public void OnInteractFinished()
    {
      if (!LootingComplete)
        OnInteractCanceled();
    }

    public void OnInteractCanceled()
    {
      ResetInteraction();
    }

    public void MoveToBlock(TerrainBlock block)
    {
      transform.position = block.Pivot;
      gameObject.SetActive(true);
    }

    private void OnLoot()
    {
      foreach (var drop in lootDrops)
        drop.GenerateDrop();
      
      OnLootedEvent.Publish(this);
      ResetInteraction();
    }

    private void ResetInteraction()
    {
      elapsedTime = 0f;
      isLooting = false;
    }
  }
}