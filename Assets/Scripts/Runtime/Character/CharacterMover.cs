using System;
using UnityEngine;

namespace Runtime.Character
{
  public class CharacterMover : MonoBehaviour
  {
    [SerializeField]
    [Range(1f, 10f)]
    private float baseSpeed = 2f;

    private CharacterController characterController;

    private void Awake()
    {
      if (!TryGetComponent(out characterController))
        characterController = GetComponentInParent<CharacterController>();

      if (characterController == null)
        throw new NullReferenceException("No player found for character mover");
    }

    public void OnPlaneMove(Vector2 direction)
    {
      var velocity = direction * baseSpeed;
      var yVelocity = 0f;
      if (!characterController.isGrounded)
        yVelocity += Physics.gravity.y;
      
      var acceleration = new Vector3(velocity.x, yVelocity, velocity.y) * Time.deltaTime;
      Debug.Log(acceleration);
      characterController.Move(acceleration);
    }
  }
}