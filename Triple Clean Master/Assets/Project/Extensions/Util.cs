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
        
        public static float SetSpacing(int level)
        {
            float spacing = 0;
            if (level ==  1)
            {
                spacing = 1f;
            }
            if (level == 2)
            {
                spacing = 0.90f;
            }
            if (level > 2 )
            {
                spacing = 0.7f;
            }
            return spacing;
        }
        public static Vector3 SetScale(int level)
        {
            float scale = 1;
            if (level == 1 )
            {
                scale = 1.1f;
            }
            if (level == 2)
            {
                scale = 0.95f;
            }
            if (level > 2 )
            {
                scale = 0.7f;
            }
            return new Vector3(scale, scale);
        }
    }
}