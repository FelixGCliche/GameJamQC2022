using System;
using UnityEngine;

namespace Runtime.Character.Player
{
  public class PlayerFactory : MonoBehaviour
  {
    [SerializeField]
    private Player crafterPlayer;
    public Player CrafterPlayer => crafterPlayer;
    
    [SerializeField]
    private Player gathererPlayer;
    public Player GathererPlayer => gathererPlayer;

    public void SpawnPlayer(int playerId, Transform spawnPoint)
    {
      switch (playerId)
      {
        case 0:
          Instantiate(gathererPlayer);
          gathererPlayer.transform.position = spawnPoint.position;
          break;
        case 1:
          Instantiate(crafterPlayer);
          crafterPlayer.transform.position = spawnPoint.position;
          break;
      }
    }
  }
}