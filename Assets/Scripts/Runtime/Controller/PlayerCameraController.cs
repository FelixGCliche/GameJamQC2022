using System;
using Runtime.Character.Player;
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
        private Player player;
        
        private Transform playerTransform;
        private Camera playerCamera;
        private Transform cameraTransform;
        private Vector3 cameraOffset;
        private Quaternion initialRotation;

        private void Awake()
        {
            SetCameraProperties();
            playerTransform = player.transform;
        }

        private void SetCameraProperties()
        {
            playerCamera = GetComponent<Camera>();
            cameraTransform = transform;
            initialRotation = cameraTransform.rotation;
            
            if (player.PlayerId == 1)
            {
                playerCamera.rect = new Rect(0f, 0f, 0.5f, 1f );
            }
            else if(player.PlayerId == 0)
            {
                playerCamera.rect = new Rect(0.5f, 0f, 0.5f, 1f );
            }
        }

        private void Start()
        {
            cameraOffset = transform.position - playerTransform.position;
        }

        private void LateUpdate()
        {
            cameraTransform.rotation = initialRotation;
            var newPos = playerTransform.position + cameraOffset;
            cameraTransform.position = Vector3.Slerp(cameraTransform.position, newPos, smoothing);
        }
    }
}