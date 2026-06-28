using UnityEngine;

namespace Project.Scripts.Json
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
