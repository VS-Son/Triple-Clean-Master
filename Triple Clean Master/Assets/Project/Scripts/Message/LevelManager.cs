using System.Collections.Generic;
using Project.Scripts.TileMatch.Level.Data;
using UnityEngine;

namespace Project.Scripts.Message
{
    public static class LevelManager
    {
        [Min(1)]private static readonly int _currentLv = 1;
        private static float _sizeToLoad;
        private static float _spacingToLoad;
        private static List<LevelData> _levelData;
         
        public static List<LayersData> LoadLayerConfigs()
        {
            var level = _levelData.Find(l =>
            {
                if (l.level == _currentLv) return true;
                return false;
            });

            return level.layers;
        }
 
    }
}
