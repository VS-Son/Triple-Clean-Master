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
            return new Color(value, value, value, 1f);
        }

        public static float SetSpacing()
        {
            if (Level == 1) return 1f;
            if (Level == 2) return 0.90f;
            return 0.7f;
        }

        public static Vector3 SetScale()
        {
            float scale;

            if (Level == 1) scale = 1.1f;
            else if (Level == 2) scale = 0.95f;
            else scale = 0.7f;

            return new Vector3(scale, scale, scale);
        }

        public static float GetScreenAspectScale()
        {
            float referenceAspect = 1080f / 1920f;
            float currentAspect = (float)Screen.width / Screen.height;

            float scale = currentAspect / referenceAspect;

            scale = Mathf.Min(1f, scale);

            scale = Mathf.Clamp(scale, 0.75f, 1f);

            return scale;
        }
    }
}