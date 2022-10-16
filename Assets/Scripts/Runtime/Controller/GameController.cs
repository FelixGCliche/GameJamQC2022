using System;
using Runtime.Character.Player;
using Runtime.Generator;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Controller
{
  public class GameController : MonoBehaviour
  {
    [SerializeField]
    private Player gathererPlayer;
    
    [SerializeField]
    private Player crafterPlayer;
    
    [SerializeField]
    private TerrainGenerator terrainGenerator;

    private PlayerInputManager playerManager;

    private void Awake()
    {
      playerManager = GetComponent<PlayerInputManager>();
    }

    private void Start()
    {
      terrainGenerator.Generate();
      terrainGenerator.gameObject.transform.position = new Vector3(0, 0, 0);
    }
  }
}