using System.Collections.Generic;
using Games.TileMatch.Level.Data;
using UnityEngine;
namespace Project.Extensions
{
    public static class Util
    {
        public static void ShuffleList<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int randIndex = Random.Range(i, list.Count);
                (list[i], list[randIndex]) = (list[randIndex], list[i]);
            }
        }
        public static Color SetAlphaSprite(int rgb)
        {
            float value = rgb / 255f;
            var color = new Color(value,value,value, 1f);
            return color;
        }
        public static float SetSpacing(LayersData layer)
        {
            float spacing = 0;
            if (layer.layer % 2 == 0)
            {
                spacing = layer.cols switch
                {
                    <= 2 => 1.05f,
                    <= 4 and > 2 => 0.83f,
                    <= 7 and > 4 => 0.3f,
                    _ => spacing
                };
            }
            else
            {
                spacing = layer.cols switch
                {
                    <= 3 => 1.05f,
                    <= 5 and > 2 => 0.83f,
                    <= 8 and > 4 => 0.3f,
                    _ => spacing
                };
            }

            return spacing;
        }
        public static Vector3 SetScale(LayersData layer)
        {
            float scale = 1;
            if (layer.layer % 2 == 0)
            {
                scale = layer.cols switch
                {
                    <= 2 => 1f,
                    <= 4 and > 2 => 0.8f,
                    <= 7 and > 4 => 0.5f,
                    _ => scale
                };
            }
            else
            {
                scale = layer.cols switch
                {
                    <= 3 => 1,
                    <= 5 and > 3 => 0.8f,
                    <= 8 and > 5 => 0.5f,
                    _ => scale
                };
            }
            return new Vector3(scale, scale);
        }
    }
}