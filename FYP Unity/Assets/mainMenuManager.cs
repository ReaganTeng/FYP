using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class mainMenuManager : MonoBehaviour
{
    public void LoadGame()
    {
        if (PlayerPrefs.GetInt("TutorialComplete") == 0)
        {
            SceneManager.LoadScene("Game Scene");
        }

        else
        {
            SceneManager.LoadScene("Level Select");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
