using System;
using Runtime.Character.Player;
using Runtime.Enum;
using Runtime.Interaction.Interactable;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Interaction
{
  public class Loot : MonoBehaviour, IInteractable
  {
    [SerializeField]
    private LootType lootType = LootType.None;
    
    private Sensor sensor;
    private ISensor<Player> playerSensor;

    private void Awake()
    {
      sensor = GetComponentInChildren<Sensor>();
      playerSensor = sensor.For<Player>();
    }

    private void OnEnable()
    {
      playerSensor.OnSensedObject += OnLooted;
      
    }

    private void OnDisable()
    {
      playerSensor.OnSensedObject -= OnLooted;
    }

    private void OnLooted(Player player)
    {
      OnInteract();
    }

    public void OnInteract()
    {
      gameObject.SetActive(false);
      Debug.Log($"Looted {lootType}");
    }
  }
}