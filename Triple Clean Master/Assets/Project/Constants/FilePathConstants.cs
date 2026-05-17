using System;
using System.IO;
namespace Project.Constants
{
    [Serializable]
    public class FilePathConstants
    {
        public const string FilePathJson = "LevelTile.json";
        public const string FolderPathJson = "Assets/Resources/Json";
        public static string FullPath => Path.Combine(FolderPathJson,FilePathJson);
    }
}
