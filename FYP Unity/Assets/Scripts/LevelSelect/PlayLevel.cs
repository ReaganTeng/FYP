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
                levelManager.DaySelected = i + 1;
                levelManager.TutorialStage = false;
                SceneManager.LoadScene("Game Scene");
                return;
            }
        }

        // check to see if it is a tutorial stage
        for (int i = 0; i < levelManager.TutorialLevel.Count; i++)
        {
            if (level == levelManager.TutorialLevel[i])
            {
                levelManager.DaySelected = i + 1;
                levelManager.TutorialStage = true;
                SceneManager.LoadScene("Game Scene");
            }
        }
    }
}
