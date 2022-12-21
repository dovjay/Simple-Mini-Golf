using UnityEngine;
using System.IO;

[DefaultExecutionOrder(-10)]
public class JsonSaving : MonoBehaviour
{
    [SerializeField]
    private LevelList levelList;

    private string persistentPath = "";

    void Awake()
    {
        SetPaths();
    }

    private void SetPaths() {
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "Save.json";
    }

    public void Save() {
        LevelWrapper[] levels = new LevelWrapper[levelList.levels.Count];

        for (int i = 0; i < levelList.levels.Count; i++)
        {
            levels[i] = new LevelWrapper(levelList.levels[i].name, 
                                      levelList.levels[i].isUnlocked(), 
                                      levelList.levels[i].GetTotalStar());
        }

        string json = JsonHelper.ToJson(levels);

        using StreamWriter writer = new StreamWriter(persistentPath);
        writer.Write(json);
        writer.Close();
    }

    public LevelWrapper[] Load() {
        if (!File.Exists(persistentPath))
        {
            return null;
        }
        using StreamReader reader = new StreamReader(persistentPath);
        string json = reader.ReadToEnd();
        reader.Close();

        LevelWrapper[] levels = JsonHelper.FromJson<LevelWrapper>(json);

        return levels;
    }

    public void DeleteSaveFile() {
        if (File.Exists(persistentPath))
            File.Delete(persistentPath);

        levelList.levels[0].OverwriteData(new LevelWrapper(levelList.levels[0].name, true, 0));
        for (int i = 1; i < levelList.levels.Count; i++)
        {
            LevelWrapper newData = new LevelWrapper(levelList.levels[i].name, false, 0);
            levelList.levels[i].OverwriteData(newData);
        }

        
    }
}
