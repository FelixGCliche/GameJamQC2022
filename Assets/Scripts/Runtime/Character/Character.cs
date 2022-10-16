using System;
using Runtime.Entity;
using Runtime.Interaction;
using Runtime.Utils;
using UnityEngine;

namespace Runtime.Character
{
  public abstract class Character : MonoBehaviour, IEntity
  {
    protected CharacterMover Mover { get; private set; }

    protected Sensor Sensor;
    public Vector3 Position => transform.position;

    public bool IsGrounded
    {
      get
      {
        var terrainRay = new Ray(transform.position, Vector3.down);
        if (!Physics.Raycast(terrainRay, out var terrainHit, Mathf.Infinity, LayerMasks.Terrain)) 
          return false;
        return terrainHit.distance < Mathf.Epsilon;
      }
    }

    protected virtual void Awake()
    {
      Sensor = GetComponentInChildren<Sensor>();
      
      if (!TryGetComponent(out CharacterMover mover))
        mover = GetComponentInChildren<CharacterMover>();

      if (mover == null)
        throw new NullReferenceException($"No mover found for character {name}");
      Mover = mover;
    }
  }
}