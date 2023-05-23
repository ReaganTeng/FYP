using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class mainMenuManager : MonoBehaviour
{
    bool SkipTutorial = false;
    [SerializeField] LevelManager LM;

    private void Start()
    {
        GameSoundManager.PlayMusic("LevelSelect");
    }

    public void NewGame()
    {
        CreateNewGame();
    }
    public void LoadGame()
    {
        if (PlayerPrefs.GetInt("TutorialComplete", 0) == 0 && !SkipTutorial)
        {
            CreateNewGame();
        }
        else
        {
            SaveFile.instance.LoadSave();
            SceneManager.LoadScene("Level Select");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void CreateNewGame()
    {
        LM.DaySelected = -2;
        SceneManager.LoadScene("VNScene");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("TutorialComplete", 0);
    }

    // temp
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            SkipTutorial = true;
            PlayerPrefs.SetInt("TutorialComplete", 1);
        }
    }
}
