using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "Level/Create Level List", order = 0)]
public class LevelList : ScriptableObject {
    public List<Level> levels;
}