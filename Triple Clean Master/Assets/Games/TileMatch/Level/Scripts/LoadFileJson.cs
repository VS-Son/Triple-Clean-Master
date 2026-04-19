using System;
using Games.TileMatch.Level.Data;
using Project.Manager;
using UnityEngine;

namespace Games.TileMatch.Level.Scripts
{
    public static class LoadFileJson 
    {
        private static TextAsset _tileJson;
        
        public static void LoadJsonPath(string pathJson)
        {
            _tileJson = Resources.Load<TextAsset>(pathJson);
        }

        public static T GetJsonTile<T>()
        {
            Debug.Log(_tileJson.text);
            return JsonUtility.FromJson<T>(_tileJson.text);;
        }

    }
}
