using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private LevelList levelList;

    private JsonSaving jsonSave;

    private void Awake() {
        Time.timeScale = 1;
        jsonSave = FindObjectOfType<JsonSaving>();
        LoadProgress();
    }

    public void LoadProgress() {
        LevelWrapper[] levels = jsonSave.Load();

        if (levels == null) return;

        for (int i = 0; i < levelList.levels.Count; i++)
        {
            levelList.levels[i].OverwriteData(levels[i]);
        }
    }
}
