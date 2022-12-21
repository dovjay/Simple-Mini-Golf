using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private LevelList levelList;

    public Level currentLevel;
    public Level nextLevel = null;
    
    private JsonSaving jsonSave;

    void Awake()
    {
        jsonSave = FindObjectOfType<JsonSaving>();
        GetLevel();
    }

    private void GetLevel()
    {
        foreach (Level level in levelList.levels)
        {
            if (level.scenePath == SceneManager.GetActiveScene().path)
            {
                currentLevel = level;
                int nextIndex = levelList.levels.IndexOf(currentLevel) + 1;
                if (levelList.levels.ElementAtOrDefault(nextIndex) != null)
                {
                    nextLevel = levelList.levels.ElementAt(nextIndex);
                }
                break;
            }
        }
    }
}
