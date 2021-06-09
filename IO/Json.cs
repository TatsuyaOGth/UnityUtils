using System.IO;
using System.Text;
using UnityEngine;

namespace Ogsn.Utils.IO
{
    public static class Json
    {
        public static void SaveToPlayerPrefs(string key, object obj)
        {
            var json = JsonUtility.ToJson(obj);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public static void SaveToPlayerPrefs(Object Obj)
        {
            string key = Obj.name;
            SaveToPlayerPrefs(key, Obj);
        }

        public static T LoadFromPlayerPrefs<T>(string key)
        {
            if (!PlayerPrefs.HasKey(key))
                return default;

            string json = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<T>(json);
        }

        public static bool LoadFromPlayerPrefs(string key, object objectToOverwrite)
        {
            if (!PlayerPrefs.HasKey(key))
                return false;

            string json = PlayerPrefs.GetString(key);
            JsonUtility.FromJsonOverwrite(json, objectToOverwrite);
            return true;
        }

        public static bool LoadFromPlayerPrefsAsJson<T>(Object ObjectToOverwrite)
        {
            string key = ObjectToOverwrite.name;
            return LoadFromPlayerPrefs(key, ObjectToOverwrite);
        }



        
        public static void SaveToFile(string filename, FileLocation fileLocation, object obj, bool prettyPrint = true)
        {
            var json = JsonUtility.ToJson(obj, prettyPrint);
            var path = GetPath(fileLocation, filename);
            File.WriteAllText(path, json, Encoding.UTF8);
        }

        public static void SaveToFile(FileLocation fileLocation, Object Obj, bool prettyPrint = true)
        {
            string filename = Obj.name + ".json";
            SaveToFile(filename, fileLocation, Obj, prettyPrint);
        }

        public static T LoadFromFile<T>(string filename, FileLocation fileLocation)
        {
            var path = GetPath(fileLocation, filename);
            if (!File.Exists(path))
                return default;

            var json = File.ReadAllText(path, Encoding.UTF8);
            return JsonUtility.FromJson<T>(json);
        }

        public static bool LoadFromFile(string filename, FileLocation fileLocation, object objectToOverwrite)
        {
            var path = GetPath(fileLocation, filename);
            if (!File.Exists(path))
                return false;

            var json = File.ReadAllText(path, Encoding.UTF8);
            JsonUtility.FromJsonOverwrite(json, objectToOverwrite);
            return true;
        }

        public static bool LoadFromFile(FileLocation fileLocation, Object ObjectToOverwrite)
        {
            string filename = ObjectToOverwrite.name + ".json";
            return LoadFromFile(filename, fileLocation, ObjectToOverwrite);
        }

        static private string GetPath(FileLocation fileLocation, string filename)
        {
            switch (fileLocation)
            {
                case FileLocation.AbsolutePath:
                    return filename;
                case FileLocation.RelativeToDataFolder:
                    return Path.Combine(Application.dataPath, filename);
                case FileLocation.RelativeToProjectFolder:
                    return Path.Combine(Path.GetDirectoryName(Application.dataPath), filename);
                case FileLocation.RelativeToStreamingAssetsFolder:
                    return Path.Combine(Application.streamingAssetsPath, filename);
                default:
                    break;
            }
            return null;
        }
    }
}
