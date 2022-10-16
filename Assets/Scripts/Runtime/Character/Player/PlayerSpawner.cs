using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Character.Player
{
  public class PlayerSpawner : MonoBehaviour
  {
    [SerializeField]
    private Transform[] spawnPoints;
    
    public void OnPlayerJoined(PlayerInput player)
    {
      Debug.Log($"Player {player.playerIndex} joined");

      var spawnPosition = spawnPoints[player.playerIndex].position;
      Debug.Log(spawnPosition);
      player.gameObject.transform.DOMove(spawnPosition, 0.1f, snapping: true);
      Debug.Log($"Player {player.playerIndex} spawned at {spawnPosition}");
    }
  }
}