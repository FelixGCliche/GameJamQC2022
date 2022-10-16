using System.Collections.Generic;
using System.Linq;
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
    public IReadOnlyList<Loot> SensedLoot => lootSensor.SensedObjects;

    protected override void Awake()
    {
      base.Awake();

      playerInput = GetComponent<PlayerInput>();
      playerInput.actions.Enable();
      moveAction = playerInput.actions["Move"];
      interactAction = playerInput.actions["Interact"];
      PlayerId = playerInput.playerIndex;

      lootSensor = Sensor.For<Loot>();
    }

    private void OnEnable()
    {
      interactAction.performed += OnInteract;
      lootSensor.OnSensedObject += OnSensedLoot;
      lootSensor.OnUnsensedObject += OnUnsensedLoot;
    }

    private void OnDisable()
    {
      interactAction.performed -= OnInteract;
      lootSensor.OnSensedObject -= OnSensedLoot;
      lootSensor.OnUnsensedObject -= OnUnsensedLoot;
      playerInput.actions.Disable();
    }

    private void OnSensedLoot(Loot loot)
    {
      Debug.Log($"{loot.LootType} in range");
    }

    private void OnUnsensedLoot(Loot loot)
    {
      Debug.Log($"{loot.LootType} out of range");
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
      if(SensedLoot.Count <= 0)
        return;
      
      var current = SensedLoot[0];
      
      if(context.ReadValueAsButton())
        current.OnInteractStarted();
      else
        current.OnInteractFinished();
    }

    private void Update()
    {
      var direction = moveAction.ReadValue<Vector2>();
      
      Mover.OnPlaneMove(direction);
    }
  }
}