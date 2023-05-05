using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class mainMenuManager : MonoBehaviour
{

    [SerializeField] LevelManager LM;

    public void NewGame()
    {
        LM.DaySelected = 1;
        SceneManager.LoadScene("VNScene");
    }
    public void LoadGame()
    {
        if (PlayerPrefs.GetInt("TutorialComplete") == 0)
        {
            SceneManager.LoadScene("VNScene");
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
