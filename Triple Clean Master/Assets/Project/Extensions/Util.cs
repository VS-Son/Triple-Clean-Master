using System.Collections.Generic;
using Games.TileMatch.Level.Data;
using Project.Manager;
using UnityEngine;
namespace Project.Extensions
{
    public static class Util
    {
        private static int Level => GameplayManager.CurrentLevel; 
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
        
        public static float SetSpacing()
        {
           
            float spacing = 0;
            if (Level ==  1)
            {
                spacing = 1f;
            }
            if (Level == 2)
            {
                spacing = 0.90f;
            }
            if (Level > 2 )
            {
                spacing = 0.7f;
            }
            return spacing;
        }
        public static Vector3 SetScale()
        {
          
            float scale = 1;
            if (Level == 1 )
            {
                scale = 1.1f;
            }
            if (Level == 2)
            {
                scale = 0.95f;
            }
            if (Level > 2 )
            {
                scale = 0.7f;
            }
            return new Vector3(scale, scale);
        }
    }
}