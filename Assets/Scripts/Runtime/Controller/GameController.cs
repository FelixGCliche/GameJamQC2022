using System;
using Runtime.Generator;
using Runtime.Terrain;
using UnityEngine;

namespace Runtime.Controller
{
  public class GameController : MonoBehaviour
  {
    [SerializeField]
    private TerrainGenerator terrainGenerator;

    private LootController lootController;

    private void Awake()
    {
      lootController = GetComponent<LootController>();
    }

    private void Start()
    {
      terrainGenerator.Generate();
      terrainGenerator.gameObject.transform.position = new Vector3(0, 0, 0);
      lootController.Generate();
    }
  }
}