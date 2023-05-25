using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayLevel : MonoBehaviour
{    
    public Level level;
    public LevelManager levelManager;

    public void PlayTheLevel()
    {        
        for (int i = 0; i < levelManager.levelInfo.Count; i++)
        {
            if (level == levelManager.levelInfo[i])
            {
                levelManager.DaySelected = levelManager.levelInfo[i].WhatDay;
                SceneManager.LoadScene("VNScene");
                return;
            }
        }
    }
}
