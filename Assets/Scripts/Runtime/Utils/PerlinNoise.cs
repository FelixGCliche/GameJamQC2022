using UnityEngine;

namespace Runtime.Utils
{
  public static class PerlinNoise
  {
    public static float[,] Map(int width, int height, float scale)
    {
      var noise = new float[width, height];

      for (var x = 0; x < width; x++)
      {
        for (int y = 0; y < height; y++)
        {
          var sampleX = (float)x / width * scale;
          var sampleY = (float)y / height * scale;
          var sample = Mathf.PerlinNoise(sampleX, sampleY);
          noise[x, y] = sample;
        }
      }
      return noise;
    }
  }
}