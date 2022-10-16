using System;
using Runtime.Interaction.Loot;

namespace Runtime.Event
{
  public static class OnLootedEvent
  {
    public static Action<Loot> OnPublished;

    public static void Publish(Loot loot) =>
      OnPublished?.Invoke(loot);
  }
}