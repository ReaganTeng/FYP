using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject WinScreen;
    [SerializeField] GameObject LoseScreen;
    [SerializeField] float TimeDisplay = 3;
    [SerializeField] LevelManager lm;
    private float displayTimer;

    private void Awake()
    {
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);
    }

    TextMeshProUGUI[] text;
    Button[] buttons;
    GameObject image;
    

    private void Start()
    {
        displayTimer = 0;
        if (SaveFile.instance != null)
        {
            // check to see if player won the game if yes, load win screen
            if (SaveFile.instance.GetIfWin())
            {
                EnableWin();
            }
            // else load lose screen
            else
            {
                EnableLose();
            }
        }
        else
        {
            EnableLose();
        }

        for (int i = 0; i < 2; i++)
        {
            Color color = text[i].color;
            color.a = 0;
            text[i].color = color;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }

        Color icolor = image.GetComponent<Image>().color;
        icolor.a = 0;
        image.GetComponent<Image>().color = icolor;
    }

    bool LoadingScene = true;
    bool LoadingText = true;
    bool LoadFinish = false;
    int index = 0;
    private void Update()
    {
        if (LoadingScene)
        {
            displayTimer += Time.deltaTime;
            float timeValue = displayTimer / TimeDisplay;
            Color ImageColor = image.GetComponent<Image>().color;
            Color color = ImageColor;
            color.a = Mathf.Lerp(0, 1, timeValue);
            image.GetComponent<Image>().color = color;

            if (displayTimer >= TimeDisplay)
            {
                displayTimer = 0;
                LoadingScene = false;
            }
        }

        else if (LoadingText)
        {
            if (index < 2)
            {
                displayTimer += Time.deltaTime;
                float timeValue = displayTimer / TimeDisplay;
                Color TextColor = text[index].color;
                Color color = TextColor;
                color.a = Mathf.Lerp(0, 1, timeValue);
                text[index].color = color;

                if (displayTimer >= TimeDisplay)
                {
                    index++;
                    displayTimer = 0;
                }
            }
            else
            {
                LoadingText = false;
                LoadFinish = true;
            }
        }

        else if (LoadFinish)
        {
            LoadFinish = false;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(true);
            }
        }
    }

    public void RestartLevel()
    {
        lm.DaySelected = 8;
        SceneManager.LoadScene("Game Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToLevelSelection()
    {
        SceneManager.LoadScene("Level Select");
    }

    void EnableWin()
    {
        WinScreen.SetActive(true);
        text = WinScreen.GetComponentsInChildren<TextMeshProUGUI>();
        buttons = WinScreen.GetComponentsInChildren<Button>();
        image = WinScreen.GetComponentInChildren<Image>().gameObject;
    }

    void EnableLose()
    {
        LoseScreen.SetActive(true);
        text = LoseScreen.GetComponentsInChildren<TextMeshProUGUI>();
        buttons = LoseScreen.GetComponentsInChildren<Button>();
        image = LoseScreen.GetComponentInChildren<Image>().gameObject;
    }
}
