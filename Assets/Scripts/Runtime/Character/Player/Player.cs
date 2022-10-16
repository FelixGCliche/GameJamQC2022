using System.Collections.Generic;
using Runtime.Controller;
using Runtime.Interaction;
using Runtime.Interaction.Loot;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Character.Player
{
  public class Player : Character
  {
    public int PlayerId { get; private set; }

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction interactAction;
    
    private ISensor<Loot> lootSensor;
    private ISensor<CraftingController> craftingSensor;
    public IReadOnlyList<Loot> SensedLoot => lootSensor.SensedObjects;
    public IReadOnlyList<CraftingController> SensedCraft => craftingSensor.SensedObjects;

    protected override void Awake()
    {
      base.Awake();

      playerInput = GetComponent<PlayerInput>();
      playerInput.actions.Enable();
      moveAction = playerInput.actions["Move"];
      interactAction = playerInput.actions["Interact"];
      PlayerId = playerInput.playerIndex;

      lootSensor = Sensor.For<Loot>();
      craftingSensor = Sensor.For<CraftingController>();
    }

    private void OnEnable()
    {
      interactAction.performed += OnInteractPerformed;
      
      lootSensor.OnSensedObject += OnSensedLoot;
      lootSensor.OnUnsensedObject += OnUnsensedLoot;
      
      craftingSensor.OnSensedObject += OnSensedCrafter;
      craftingSensor.OnUnsensedObject += OnUnsensedCrafter;
    }

    private void OnDisable()
    {
      interactAction.performed -= OnInteractPerformed;
      playerInput.actions.Disable();
      
      lootSensor.OnSensedObject -= OnSensedLoot;
      lootSensor.OnUnsensedObject -= OnUnsensedLoot;
      
      craftingSensor.OnSensedObject -= OnSensedCrafter;
      craftingSensor.OnUnsensedObject -= OnUnsensedCrafter;
    }

    private void OnSensedCrafter(CraftingController crafting)
    {
      Debug.Log($"{crafting.name} in range");
    }

    private void OnUnsensedCrafter(CraftingController crafting)
    {
      Debug.Log($"{crafting.name} in range");
    }

    private void OnSensedLoot(Loot loot)
    {
      Debug.Log($"{loot.LootType} in range");
    }

    private void OnUnsensedLoot(Loot loot)
    {
      Debug.Log($"{loot.LootType} out of range");
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
      if(PlayerId == 0)
        OnLootInteraction(context);
      if(PlayerId == 1)
        OnCraftingInteraction(context);
    }

    private void OnLootInteraction(InputAction.CallbackContext context)
    {
      if(SensedLoot.Count <= 0)
        return;
      
      var current = SensedLoot[0];
      Debug.Log(current.LootType);
      
      if(context.ReadValueAsButton())
        current.OnInteractStarted();
      else
        current.OnInteractFinished();
    }

    private void OnCraftingInteraction(InputAction.CallbackContext context)
    {
      Debug.Log(SensedCraft.Count);
      if(SensedCraft.Count <= 0)
        return;

      var current = SensedCraft[0];
      if(current.TryCraft())
        Debug.Log($"{current.gameObject.name} successful");
      else
        Debug.Log($"{current.gameObject.name} fail");
    }

    private void Update()
    {
      var direction = moveAction.ReadValue<Vector2>();
      
      Mover.OnPlaneMove(direction);
    }
  }
}