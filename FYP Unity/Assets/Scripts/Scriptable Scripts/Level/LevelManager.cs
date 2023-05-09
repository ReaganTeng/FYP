using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelManager", menuName = "LevelManager")]
public class LevelManager : ScriptableObject
{
    public List<Level> levelInfo;
    public int DaySelected;

    public List<Level> TutorialLevel;
    public bool TutorialStage = false;

    public void ResetGameLevel()
    {
        for(int i = 0; i < levelInfo.Count; i++)
        {
            levelInfo[i].ResetLevel();
        }
    }
}
