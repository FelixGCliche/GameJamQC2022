using Runtime.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Character.Player
{
  public class Player : Character
  {
    public int PlayerId { get; private set; }

    private PlayerInput playerInput;
    private InputAction moveAction;

    protected override void Awake()
    {
      base.Awake();

      playerInput = GetComponent<PlayerInput>();
      playerInput.actions.Enable();
      moveAction = playerInput.actions["Move"];
      PlayerId = playerInput.playerIndex;
      Debug.Log(PlayerId);
    }

    private void Update()
    {
      var direction = moveAction.ReadValue<Vector2>();
      Mover.OnPlaneMove(direction);
    }
  }
}