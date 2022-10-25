using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Controller;
using Runtime.Interaction;
using Runtime.Interaction.Loot;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Character.Player
{
  public class Player : Character
  {
    [SerializeField]
    private Transform playerMesh;
    public int PlayerId { get; private set; }

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction interactAction;
    
    private ISensor<Loot> lootSensor;
    private ISensor<CraftingController> craftingSensor;
    public IReadOnlyList<Loot> SensedLoot => lootSensor.SensedObjects;
    public IReadOnlyList<CraftingController> SensedCraft => craftingSensor.SensedObjects;

    private Transform meshTransform;

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

    private void Start()
    {
      name = PlayerId switch
      {
        0 => "CrafterPlayer",
        1 => "GathererPlayer",
        _ => name
      };
        var spawnPosition = new Vector3(200f, 1, 200f);

        gameObject.transform.DOMove(spawnPosition, 0.1f, snapping: true);
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
    }

    private void OnUnsensedCrafter(CraftingController crafting)
    {
    }

    private void OnSensedLoot(Loot loot)
    {
    }

    private void OnUnsensedLoot(Loot loot)
    {
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
      if(PlayerId == 1)
        OnLootInteraction(context);
      if(PlayerId == 0)
        OnCraftingInteraction(context);
    }

    private void OnLootInteraction(InputAction.CallbackContext context)
    {
      if(SensedLoot.Count <= 0)
        return;
      
      var current = SensedLoot[0];
      
      if(context.ReadValueAsButton())
        current.OnInteractStarted();
      else
        current.OnInteractFinished();
    }

    private void OnCraftingInteraction(InputAction.CallbackContext context)
    {
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
      UpdateModelOrientation(direction);
      // var d = Vector2.SignedAngle(Vector2.up, direction);
      // Debug.Log(d);
    }

    private void UpdateModelOrientation(Vector2 direction)
    {
      if(direction == Vector2.zero)
        return;

      var signedAngle = Vector2.SignedAngle(Vector2.up, direction) * -1;
      Debug.Log(signedAngle);
      playerMesh.DORotate(new Vector3(0,signedAngle,0), 0.1f);

    }
  }
}