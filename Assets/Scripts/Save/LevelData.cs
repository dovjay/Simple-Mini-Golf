using System;

[Serializable]
public struct LevelWrapper {
    public string name;
    public bool unlocked;
    public int totalStar;

    public LevelWrapper(string levelName, bool levelUnlocked, int levelTotalStar)
    {
        this.name = levelName;
        this.unlocked = levelUnlocked;
        this.totalStar = levelTotalStar;
    }
}