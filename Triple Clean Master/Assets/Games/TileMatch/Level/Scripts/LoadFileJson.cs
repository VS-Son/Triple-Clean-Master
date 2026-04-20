using System;
using Games.TileMatch.Level.Data;
using Project.Manager;
using System.Threading.Tasks; 
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace Games.TileMatch.Level.Scripts
{
    public static class LoadFileJson 
    {
        private static TextAsset _json;
        
        public static void LoadResource(string filePath)
        {
            _json = Resources.Load<TextAsset>(filePath);
        }

        public static T GetData<T>()
        {
            if (_json == null)
            {
                return default;
            }
            //Debug.Log(_json.text);
            return JsonUtility.FromJson<T>(_json.text);;
        }

    }
}
