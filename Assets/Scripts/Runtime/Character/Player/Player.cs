using System.Collections.Generic;
using DG.Tweening;
using Runtime.Crafting;
using Runtime.Event;
using Runtime.Interaction;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Character.Player
{
  public class Player : Character
  {
    private static readonly int IsIdle = Animator.StringToHash("isIdle");
    private static readonly int Attack = Animator.StringToHash("attack");
    
    [SerializeField]
    private Transform playerMesh;

    [SerializeField]
    private List<GameObject> playerModels;
    
    private Animator playerAnimator;
    public int PlayerId { get; private set; }

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction interactAction;
    
    private ISensor<Loot.Loot> lootSensor;
    private ISensor<CraftingRecipe> craftingSensor;
    public IReadOnlyList<Loot.Loot> SensedLoot => lootSensor.SensedObjects;
    public IReadOnlyList<CraftingRecipe> SensedCraft => craftingSensor.SensedObjects;

    protected override void Awake()
    {
      base.Awake();

      playerInput = GetComponent<PlayerInput>();
      playerInput.actions.Enable();
      moveAction = playerInput.actions["Move"];
      interactAction = playerInput.actions["Interact"];
      PlayerId = playerInput.playerIndex;

      lootSensor = Sensor.For<Loot.Loot>();
      craftingSensor = Sensor.For<CraftingRecipe>();
    }

    private void Start()
    {
      name = PlayerId switch
      {
        0 => "CrafterPlayer",
        1 => "GathererPlayer",
        _ => name
      };
      
      var playerModel = Instantiate(playerModels[PlayerId], playerMesh);
      playerAnimator = playerModel.GetComponent<Animator>();
      playerAnimator.SetBool(IsIdle, true);

      var spawnPosition = new Vector3(200f, 1, 200f);
      gameObject.transform.DOMove(spawnPosition, 0.1f, snapping: true);
    }

    private void OnEnable()
    {
      interactAction.performed += OnInteractPerformed;
      OnLootedEvent.OnPublished += OnLooted;
      
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

    private void OnSensedCrafter(CraftingRecipe recipe)
    {
    }

    private void OnUnsensedCrafter(CraftingRecipe recipe)
    {
    }

    private void OnSensedLoot(Loot.Loot loot)
    {
    }

    private void OnUnsensedLoot(Loot.Loot loot)
    {
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
      if(PlayerId == 1 && SensedLoot.Count > 0)
        OnLootInteraction(context);
      if(PlayerId == 0 && SensedCraft.Count > 0)
        OnCraftingInteraction(context);
    }

    private void OnLootInteraction(InputAction.CallbackContext context)
    {
      var current = SensedLoot[0];

      if (context.ReadValueAsButton())
      {
        current.OnInteractStarted();
        playerAnimator.SetBool(Attack, true);
      }
      else
      {
        current.OnInteractFinished();
      }
    }

    private void OnLooted(Loot.Loot loot)
    {
      playerAnimator.SetBool(Attack, false);
    }

    private void OnCraftingInteraction(InputAction.CallbackContext context)
    {
      var current = SensedCraft[0];

      if (context.ReadValueAsButton())
      {
        current.OnInteractStarted();
        // TODO Set crafting animation
      }
      else
      {
        current.OnInteractFinished();
      }
    }

    private void Update()
    {
      var direction = moveAction.ReadValue<Vector2>();
      
      if (direction == Vector2.zero)
        playerAnimator.SetBool(IsIdle, true);
      else
        UpdateModelOrientation(direction);
      
      Mover.OnPlaneMove(direction);
    }

    private void UpdateModelOrientation(Vector2 direction)
    {
      playerAnimator.SetBool(IsIdle, false);
      var signedAngle = Vector2.SignedAngle(Vector2.up, direction) * -1;
      playerMesh.DORotate(new Vector3(0,signedAngle,0), 0.1f);

    }
  }
}