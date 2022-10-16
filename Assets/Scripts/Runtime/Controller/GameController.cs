using Runtime.Generator;
using UnityEngine;

namespace Runtime.Controller
{
  public class GameController : MonoBehaviour
  {
    
    [SerializeField]
    private TerrainGenerator terrainGenerator;

    private void Start()
    {
      terrainGenerator.Generate();
      terrainGenerator.gameObject.transform.position = new Vector3(0, 0, 0);
    }
  }
}