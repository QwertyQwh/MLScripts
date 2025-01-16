using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;

internal class DataBaseEditorConfig
{
    public string kWorkingPath;
    
    private const string kEidtorConfigPath = "Assets/Scripts/Editor/Configs/MonsterEditor.json";

    public static DataBaseEditorConfig Load()
    {
        if (!File.Exists(kEidtorConfigPath))
            return new DataBaseEditorConfig();

        var data = File.ReadAllText(kEidtorConfigPath);
        var config = JsonConvert.DeserializeObject<DataBaseEditorConfig>(data);
        return config;
    }

    public void Save()
    {
        FileUtil.DeleteFileOrDirectory(kEidtorConfigPath);
        File.WriteAllText(kEidtorConfigPath, JsonConvert.SerializeObject(this));
    }
}

