using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialLevelSelect : MonoBehaviour
{
    public static TutorialLevelSelect instance;

    [SerializeField] PlayerMenuMovement pmm;
    public bool InTutorial = false;

    private float TimeTillNextText;

    [SerializeField] LevelManager lm;
    [SerializeField] GameObject SquidChatBox;
    private TextMeshProUGUI SquidText;
    [SerializeField] GameObject SkipPrompt;
    private TextMeshProUGUI skipPromptText;
    private int currentIndex = 0;

    private bool SkipPromptActive; // Check to see if the skipprompt is active
    private int skipprompttimechange; // affects whether the timer is + or - by deltatime

    // Blinking effect for skip prompt
    [SerializeField] float BlinkDuration = 1;
    private float blinktimer = 2;

    [System.Serializable]
    public class TutorialPopUp
    {
        public string TextDisplay; // display the text shown on the chatbox
        public float TimeTillPromptSkip; // Set the time for when the prompt to skip comes in
        public bool Skippable; //set as the last one
    }

    [SerializeField] List<Collider> DisableLevel;
    [SerializeField] List<TutorialPopUp> tut;

    private bool StartFirst = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("TutorialComplete") == 0)
        {
            pmm.DisablePlayerControls();
            InTutorial = true;
            SquidText = SquidChatBox.GetComponentInChildren<TextMeshProUGUI>();
            skipPromptText = SkipPrompt.GetComponent<TextMeshProUGUI>();
            SkipPrompt.SetActive(false); // A variable that set the "Press Space to continue" to false first

            if (lm.DaySelected == 2)
            {
                currentIndex = 9;
            }
        }
        else
            SquidChatBox.SetActive(false);
    }


    private void Update()
    {
        if (InTutorial)
        {
            // If it is a dialogue, reduce the timer till skipprompt appear
            if (!SkipPromptActive && tut[currentIndex].Skippable)
            {
                TimeTillNextText -= Time.deltaTime;
                // set the prompt to appear
                if (TimeTillNextText <= 0)
                {
                    blinktimer = 0;
                    SetTransparency();
                    SkipPromptActive = true;
                    SkipPrompt.SetActive(true);
                }
            }

            else
            {
                // make it blink by setting alpha from 0 to 1 in timegiven/2 and vise verca.
                if (blinktimer <= 0)
                {
                    skipprompttimechange = 1;
                }

                if (blinktimer >= BlinkDuration * 0.5f)
                {
                    skipprompttimechange = -1;
                }

                blinktimer += skipprompttimechange * Time.deltaTime;
                SetTransparency();
            }

            // if it is an instruction that has been completed, or a dialogue and user press SPACE, skip to next dialogue
            // Note: The first run automatically trigger this function
            if ((Input.GetKeyDown(KeyCode.Space) && tut[currentIndex].Skippable) || StartFirst)
            {
                StartFirst = false;
                // Set Skipprompt to not active
                SkipPrompt.SetActive(false);
                SkipPromptActive = false;

                // Set what is next in the List to the textbox and values required.
                if (currentIndex < tut.Count)
                {
                    SquidText.text = tut[currentIndex].TextDisplay;
                    TimeTillNextText = tut[currentIndex].TimeTillPromptSkip;

                    currentIndex++;

                    if (currentIndex >= tut.Count)
                        return;

                    if (!tut[currentIndex].Skippable)
                    {
                        pmm.EnablePlayerControls();

                        for (int i = 0; i < DisableLevel.Count; i++)
                        {
                            DisableLevel[i].enabled = false;
                        }
                    }
                }
                // if it finish running the list, stop the tutorial, and let the game run normally.
                else
                {
                    InTutorial = false;
                    SquidChatBox.SetActive(false);
                    pmm.EnablePlayerControls();
                    PlayerPrefs.SetFloat("TutorialComplete", 1);
                }
            }
        }
    }

    public void SetTransparency()
    {
        Color color = skipPromptText.color;
        float ratioTimer = blinktimer / (BlinkDuration * 0.5f);
        color.a = Mathf.Lerp(0, 1, ratioTimer);
        skipPromptText.color = color;
    }
}