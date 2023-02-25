using System.IO;
using UnityEngine;

public class ReaderWriterJSON : MonoBehaviour
{
    private static string persistentPath = Application.persistentDataPath;

    public static void SaveToJSON(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        // for some reason "Path.Combine" does not work here ...
        string path = persistentPath + "/" + data.filename;

        if (!File.Exists(path)) File.Create(path).Close();

        File.WriteAllText(path, json);
    }

    public static void LoadFromJSON<T>(ref T data) where T : SaveData
    {
        string path = Path.Combine(persistentPath, data.filename);

        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        JsonUtility.FromJsonOverwrite(json, data);
    }
}
