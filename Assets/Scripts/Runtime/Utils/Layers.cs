using UnityEngine;

namespace Runtime.Utils
{
  public static class Layers
  {
    public static int Terrain => LayerMask.NameToLayer(nameof(Terrain));
  }

  public static class LayerMasks
  {
    public static int Terrain => LayerMask.GetMask(nameof(Terrain));
  }
}