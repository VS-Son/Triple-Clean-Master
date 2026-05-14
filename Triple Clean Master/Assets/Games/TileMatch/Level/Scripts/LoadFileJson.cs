using System;
using Games.TileMatch.Level.Data;
using Project.Manager;
using System.Threading.Tasks;
using System.IO;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace Games.TileMatch.Level.Scripts
{
    [Serializable]
    public class FileConstants
    {
        public const string FilePath = "LevelTile.json";
        public const string FolderPath = "Assets/Resources/Json";
        public static string FullPath => Path.Combine(FolderPath,FilePath);


    }
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
            return JsonUtility.FromJson<T>(_json.text);;
        }

    }
}
