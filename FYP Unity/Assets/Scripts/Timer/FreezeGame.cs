using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FreezeGame : MonoBehaviour
{
    public static FreezeGame instance;

    // For game startup freeze (Freeze certain part of the game)
    public bool startUpfreeze = true;
    [SerializeField] float timeTillGameStart = 3;
    private float freezeGameTimer;

    // For pause menu(Freezing whole game)
    [SerializeField] float timeTillGameResume = 3;
    bool MenuPause = false;
    float gameResumeTimer;
    bool startGameResumeTimer;
    [SerializeField] GameObject ActionText;


    // The pause menu
    GameObject PauseMenu;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameResumeTimer = timeTillGameResume;
        freezeGameTimer = timeTillGameStart;
        startGameResumeTimer = false;
        PauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        PauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !startGameResumeTimer)
        {
            PauseGame();
        }


        if (startUpfreeze)
        {
            freezeGameTimer -= Time.deltaTime;
            if (freezeGameTimer <= 0)
            {
                startUpfreeze = false;
                ActionText.SetActive(false);
            }

            // Greather than 2/3 of the time remaining
            if (freezeGameTimer / timeTillGameStart > (float)2 / 3)
            {
                ActionText.GetComponent<TextMeshProUGUI>().text = "READY?";
            }
            // Greather than 1/3 of the time remaining
            else if (freezeGameTimer / timeTillGameStart > (float)1 / 3)
            {
                ActionText.GetComponent<TextMeshProUGUI>().text = "GET SET";
            }
            else
            {
                ActionText.GetComponent<TextMeshProUGUI>().text = "COOK!";
            }
        }

        if (MenuPause && startGameResumeTimer)
        {
            gameResumeTimer -= Time.unscaledDeltaTime;
            if (gameResumeTimer <= 0)
            {
                FreezeTheGame();
                startGameResumeTimer = false;
                ActionText.SetActive(false);
            }

            // Greather than 2/3 of the time remaining
            if (gameResumeTimer/timeTillGameResume > (float) 2/3)
            {
                ActionText.GetComponent<TextMeshProUGUI>().text = "READY?";
            }
            // Greather than 1/3 of the time remaining
            else if (gameResumeTimer / timeTillGameResume > (float) 1 / 3)
            {
                ActionText.GetComponent<TextMeshProUGUI>().text = "GET SET";
            }
            else
            {
                ActionText.GetComponent<TextMeshProUGUI>().text = "COOK!";
            }
        }
    }

    void FreezeTheGame()
    {
        if (!MenuPause)
        {
            Time.timeScale = 0;
            MenuPause = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().DisablePlayerControls();
        }
        else
        {
            Time.timeScale = 1;
            MenuPause = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().EnablePlayerControls();
        }
    }

    public void PauseGame()
    {
        FreezeTheGame();
        PauseMenu.SetActive(true);
        gameResumeTimer = timeTillGameResume;
        Debug.Log("Pause");
    }

    public void UnpauseGame()
    {
        ActionText.SetActive(true);
        startGameResumeTimer = true;
        PauseMenu.SetActive(false);
    }

    public void IgnoreStartUp()
    {
        startUpfreeze = false;
        ActionText.SetActive(false);
    }
}
