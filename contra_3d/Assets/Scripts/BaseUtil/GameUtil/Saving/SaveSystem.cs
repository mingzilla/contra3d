using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace BaseUtil.GameUtil.Saving
{
    public static class SaveSystem
    {
        public static void SaveData<T>(T data, string fileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = $"{Application.persistentDataPath}/{fileName}";
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static T LoadData<T>(string fileName) where T : Object
        {
            string path = $"{Application.persistentDataPath}/{fileName}";

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                T data = formatter.Deserialize(stream) as T;
                return data;
            }
            else
            {
                Debug.LogError("Save filed does not exist in " + path);
                return null;
            }
        }
    }
}