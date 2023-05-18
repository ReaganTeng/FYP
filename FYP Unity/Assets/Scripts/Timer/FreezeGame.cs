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
        // if player press escape when it is not paused(except if during a menupause)
        if (Input.GetKeyDown(KeyCode.Escape) && !startGameResumeTimer && !startUpfreeze)
        {
            TogglePause();
        }

        // for startUpFreeze, when it reach 0, the game officially start
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

        // For any pauses during the game, when it hit 0, the game will unfreeze
        if (MenuPause && startGameResumeTimer)
        {
            gameResumeTimer -= Time.unscaledDeltaTime;
            if (gameResumeTimer <= 0)
            {
                TogglePause(true);
                startGameResumeTimer = false;
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

    public void TogglePause(bool UnPauseGame = false)
    {
        // if game is not pause, pause the game
        if (!MenuPause)
        {
            PauseMenu.SetActive(true); // Render the pause menu
            gameResumeTimer = timeTillGameResume; // reset the countdown timer
            Debug.Log("Pause");
            // pause the game
            Time.timeScale = 0;
            MenuPause = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().DisablePlayerControls();
        }
        // if game is paused, toggling it will start the countdown
        else if (MenuPause && !startGameResumeTimer)
        {
            ActionText.SetActive(true);
            startGameResumeTimer = true;
            PauseMenu.SetActive(false);
        }

        // if the countdown is over, unpause the game
        else if (UnPauseGame)
        {
            // stop rendering the Action text
            ActionText.SetActive(false);
            // Unpause the game
            Time.timeScale = 1;
            MenuPause = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().EnablePlayerControls();
        }
    }

    public void IgnoreStartUp()
    {
        startUpfreeze = false;
        ActionText.SetActive(false);
    }
}