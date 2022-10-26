using System;

namespace Runtime.Event
{
  public static class OnLootedEvent
  {
    public static Action<Loot.Loot> OnPublished;

    public static void Publish(Loot.Loot loot) =>
      OnPublished?.Invoke(loot);
  }
}