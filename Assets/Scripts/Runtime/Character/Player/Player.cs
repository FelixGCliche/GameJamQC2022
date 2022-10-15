using Runtime.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Character.Player
{
  public class Player : Character
  {
    [SerializeField]
    private int playerId = 1;

    public int PlayerId => playerId;

    private InputAction moveActions;

    protected override void Awake()
    {
      base.Awake();
      
      moveActions = Inputs.Actions.Player.Move;
      moveActions.Enable();
    }

    private void OnDisable()
    {
      moveActions.Disable();
    }

    private void Update()
    {
      var direction = moveActions.ReadValue<Vector2>();
      Mover.OnPlaneMove(direction);
    }
  }
}