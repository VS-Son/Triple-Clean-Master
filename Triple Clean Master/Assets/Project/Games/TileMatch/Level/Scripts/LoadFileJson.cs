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
            return JsonUtility.FromJson<T>(_json.text);;
        }

    }
}
