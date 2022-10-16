using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Interaction
{
  public delegate void SensorEventHandler<in T>(T otherObject);

  public interface ISensor<out T>
  {
    event SensorEventHandler<T> OnSensedObject;
    event SensorEventHandler<T> OnUnsensedObject;
    IReadOnlyList<T> SensedObjects { get; }
  }

  public class Sensor<T> : ISensor<T>
  {
    private readonly Sensor sensor;
    
    private readonly List<T> sensedObjects;
    private SensorEventHandler<T> onSensedObject;
    private SensorEventHandler<T> onUnsensedObject;
    
    public IReadOnlyList<T> SensedObjects => sensedObjects;

    public Sensor(Sensor sensor)
    {
      this.sensor = sensor;
      sensedObjects = new List<T>();
    }

    public event SensorEventHandler<T> OnSensedObject
    {
      add
      {
        if (onSensedObject == null || onSensedObject.GetInvocationList().Length == 0)
          sensor.OnSensedObject += OnSensedObjectInternal;
        onSensedObject += value;
      }
      remove
      {
        if (onSensedObject != null && onSensedObject.GetInvocationList().Length == 1)
          sensor.OnSensedObject -= OnSensedObjectInternal;
        onSensedObject -= value;
      }
    }

    public event SensorEventHandler<T> OnUnsensedObject
    {
      add
      {
        if (onUnsensedObject == null || onUnsensedObject.GetInvocationList().Length == 0)
          sensor.OnUnsensedObject += OnUnsensedObjectInternal;
        onUnsensedObject += value;
      }
      remove
      {
        if (onUnsensedObject != null && onUnsensedObject.GetInvocationList().Length == 1)
          sensor.OnUnsensedObject -= OnUnsensedObjectInternal;
        onUnsensedObject -= value;
      }
    }

    private void OnSensedObjectInternal(GameObject otherGameObject)
    {
      if (!otherGameObject.TryGetComponent<T>(out var sensedObject))
        sensedObject = otherGameObject.GetComponentInChildren<T>();
      
      if(sensedObject == null)
        return;
      
      sensedObjects.Add(sensedObject);
      onSensedObject?.Invoke(sensedObject);
    }

    private void OnUnsensedObjectInternal(GameObject otherGameObject)
    {
      if (!otherGameObject.TryGetComponent<T>(out var sensedObject))
        sensedObject = otherGameObject.GetComponentInChildren<T>();
      
      if(sensedObject == null)
        return;
      
      sensedObjects.Remove(sensedObject);
      onUnsensedObject?.Invoke(sensedObject);
    }
  }
}