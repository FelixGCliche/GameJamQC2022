using System;
using UnityEditor.Rendering.HighDefinition;
using UnityEngine;

namespace Runtime.Controller
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField]
        [Range(0.01f, 1.0f)]
        private float smoothing;
        
        [SerializeField]
        private Transform playerTransform;
        
        private Vector3 cameraOffset;
        private Quaternion initialRotation;

        private void Awake()
        {
            initialRotation = transform.rotation;
        }

        private void Start()
        {
            cameraOffset = transform.position - playerTransform.position;
        }

        private void LateUpdate()
        {
            transform.rotation = initialRotation;
            var newPos = playerTransform.position + cameraOffset;
            transform.position = Vector3.Slerp(transform.position, newPos, smoothing);
        }
    }
}