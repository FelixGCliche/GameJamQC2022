using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Interaction
{
  [RequireComponent(typeof(Collider))]
  public partial class Sensor : MonoBehaviour, ISensor<GameObject>
  {
    private SphereCollider sensorTrigger;
    private List<GameObject> sensedObjects;
    private Transform parentTransform;

    public event SensorEventHandler<GameObject> OnSensedObject;
    public event SensorEventHandler<GameObject> OnUnsensedObject;
    public IReadOnlyList<GameObject> SensedObjects => sensedObjects;

    private void Awake()
    {
      parentTransform = transform.parent;
      sensorTrigger = GetComponent<SphereCollider>();
      gameObject.AddComponent<Rigidbody>().isKinematic = true;
    }

    private void OnEnable()
    {
      sensorTrigger.enabled = true;
      sensorTrigger.isTrigger = true;
    }

    private void OnDisable()
    {
      sensorTrigger.enabled = false;
      sensorTrigger.isTrigger = false;
      sensedObjects.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
      if(!IsSelf(other.transform)) 
        AddSensedObject(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
      if(!IsSelf(other.transform)) 
        RemoveSensedObject(other.gameObject);
    }

    public Sensor()
    {
      sensedObjects = new List<GameObject>();
    }

    public ISensor<T> For<T>()
    {
      return new Sensor<T>(this);
    }


    private bool IsSelf(Transform otherParentTransform)
    {
      return parentTransform == otherParentTransform;
    }

    private void AddSensedObject(GameObject sensedObject)
    {
      if (SensedObjects.Contains(sensedObject)) return;
      sensedObjects.Add(sensedObject);
      OnSensedObject?.Invoke(sensedObject);
    }

    private void RemoveSensedObject(GameObject sensedObject)
    {
      if (!SensedObjects.Contains(sensedObject)) return;
      sensedObjects.Remove(sensedObject);
      OnUnsensedObject?.Invoke(sensedObject);
    }
  }
}