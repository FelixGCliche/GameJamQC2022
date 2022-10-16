using System;
using Runtime.Enum;
using Runtime.Interaction.Interactable;
using UnityEngine;

namespace Runtime.Interaction.Loot
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
      Debug.Log(lootDrops.Length);
      ResetInteraction();
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
      Debug.Log("Looting canceled");
      ResetInteraction();
    }

    private void Update()
    {
      if (isLooting)
      {
        elapsedTime += Time.deltaTime / lootDuration;
        if(LootingComplete)
          OnLoot();
      }
    }

    private void OnLoot()
    {
      foreach (var drop in lootDrops)
        drop.GenerateDrop();
      
      Debug.Log("Looting finished");
      ResetInteraction();
    }

    private void ResetInteraction()
    {
      elapsedTime = 0f;
      isLooting = false;
    }
  }
}