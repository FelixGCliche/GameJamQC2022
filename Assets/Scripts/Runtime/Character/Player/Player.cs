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

    protected override void Awake()
    {
      base.Awake();

      playerInput = GetComponent<PlayerInput>();
      playerInput.actions.Enable();
      moveAction = playerInput.actions["Move"];
      interactAction = playerInput.actions["Interact"];
      PlayerId = playerInput.playerIndex;
    }

    private void OnEnable()
    {
      interactAction.performed += OnInteract;
    }

    private void OnDisable()
    {
      interactAction.performed -= OnInteract;
      playerInput.actions.Disable();
    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
      Debug.Log($"Interact {obj}");
    }

    private void FixedUpdate()
    {
      var direction = moveAction.ReadValue<Vector2>();
      Mover.OnPlaneMove(direction);
    }
  }
}