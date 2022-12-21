using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Level n", menuName = "Level/Create Level", order = 0)]
public class Level : ScriptableObject {
    [HideInInspector]
    public string scenePath;
    public int remainingShot;
    public float maxTime;
    [SerializeField]
    private bool unlocked = false;
    [SerializeField]
    private int totalStar = 0;

    public int GetTotalStar() => totalStar;

    public void UpdateTotalStar(int finalScore) {
        if (totalStar < finalScore)
            totalStar = finalScore;
    }

    public void UnlockLevel() => unlocked = true;

    public bool isUnlocked() => unlocked;

    public void ResetProgress() {
        unlocked = false;
        totalStar = 0;
    }

    public void OverwriteData(LevelWrapper level) {
        if (this.name != level.name) return;

        unlocked = level.unlocked;
        totalStar = level.totalStar;
    }
}