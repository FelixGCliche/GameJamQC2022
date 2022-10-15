using System;
using Runtime.Character.Player;
using Runtime.Generator;
using UnityEditor;
using UnityEngine;

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

    private void Start()
    {
      terrainGenerator.Generate();
      terrainGenerator.gameObject.transform.position = new Vector3(0, 0, 0);
    }

    private void OnEnable()
    {
      Instantiate(gathererPlayer);
      gathererPlayer.gameObject.name = nameof(gathererPlayer);
      gathererPlayer.transform.position += Vector3.left * 2f;
      
      // Instantiate(crafterPlayer);
      // crafterPlayer.gameObject.name = nameof(crafterPlayer);
      // crafterPlayer.transform.position += Vector3.right * 2f;
    }
  }
}