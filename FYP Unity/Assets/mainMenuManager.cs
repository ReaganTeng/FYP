using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class mainMenuManager : MonoBehaviour
{

    [SerializeField] LevelManager LM;

    private void Start()
    {
        GameSoundManager.PlayMusic("MainMenu");
    }

    public void NewGame()
    {
        LM.DaySelected = 1;
        LM.TutorialStage = true;
        SceneManager.LoadScene("VNScene");
    }
    public void LoadGame()
    {
        if (PlayerPrefs.GetInt("TutorialComplete") == 0)
        {
            LM.DaySelected = 1;
            LM.TutorialStage = true;
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
