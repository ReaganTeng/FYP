using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndOfDay : MonoBehaviour
{
    public static EndOfDay instance;
    [SerializeField] PlayerProgress ps;
    [SerializeField] OrderSystem os;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] int score;
    [SerializeField] LevelManager lm;
    private Level level;
    PlayerMovement playercontrols;
    private char GradeObtained;
    private bool EndOfDayAnimation;
    private int ScorePenalty;
    bool GameStop;

    [SerializeField] GameObject EndOfDayBackground;

    [SerializeField] List<GameObject> resultDisplay;
    [SerializeField] GameObject ClearedText;
    [SerializeField] GameObject FailedText;
    [SerializeField] GameObject TotalText;


    [SerializeField] GameObject Textbox;
    [SerializeField] string HappyDialogue;
    [SerializeField] string NeutralDialogue;
    [SerializeField] string AngryDialogue;

    [SerializeField] GameObject Squid;
    [SerializeField] Sprite HappySquid;
    [SerializeField] Sprite NeutralSquid;
    [SerializeField] Sprite AngrySquid;

    [SerializeField] GameObject Grade;
    [SerializeField] Sprite Srank;
    [SerializeField] Sprite Arank;
    [SerializeField] Sprite Brank;
    [SerializeField] Sprite Crank;
    [SerializeField] Sprite Frank;

    [SerializeField] float TextFadeInDuration;
    bool DisplayDetails = false;
    float textfadeInTimer = 0;
    int textFadeInIndex = 0;
    bool SetCurrentActive = false;

    [SerializeField] float StarDisplayDuration;
    [SerializeField] GameObject starDisplay;
    GameObject textobj;
    bool DisplayStars = false;
    float displayStarTimer = 0;

    [SerializeField] GameObject GradeSection;
    [SerializeField] GameObject minValue;
    [SerializeField] GameObject maxValue;
    [SerializeField] Slider gradeSlider;
    [SerializeField] float SliderDuration;
    [SerializeField] GameObject creditObtained;

    int index;
    bool LoadGrade = false;
    float BarTimer = 0;

    int CreditObtained = 0;

    private void Start()
    {
        text.text = "0";
        playercontrols = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        Textbox.SetActive(false);
        EndOfDayBackground.SetActive(false);
        GradeSection.SetActive(false);
        EndOfDayAnimation = false;
        index = 0;
        GameStop = false;

        // Set star rating to not render first
        textobj = starDisplay.GetComponentInChildren<TextMeshProUGUI>().gameObject;
        textobj.SetActive(false);
        starDisplay.SetActive(false);

        // Do not render the cc yet
        creditObtained.SetActive(false);
    }

    private void Update()
    {        
        if (DisplayDetails)
        {
            if (!SetCurrentActive)
            {
                resultDisplay[textFadeInIndex].GetComponent<TextMeshProUGUI>().alpha = 0;
                resultDisplay[textFadeInIndex].SetActive(true);
                SetCurrentActive = true;
            }

            textfadeInTimer += Time.deltaTime;
            resultDisplay[textFadeInIndex].SetActive(true);
            float currentalpha = textfadeInTimer / TextFadeInDuration;
            resultDisplay[textFadeInIndex].GetComponent<TextMeshProUGUI>().alpha = currentalpha;

            if (currentalpha > 1)
            {
                SetCurrentActive = false;
                textfadeInTimer = 0;
                textFadeInIndex++;

                if (textFadeInIndex == resultDisplay.Count)
                {
                    DisplayDetails = false;
                    starDisplay.SetActive(true);
                }
            }
        }

        else if (DisplayStars)
        {
            displayStarTimer += Time.deltaTime;
            float timevalue = displayStarTimer / StarDisplayDuration;
            Slider starSlider = starDisplay.GetComponentInChildren<Slider>();
            starSlider.value = Mathf.SmoothStep(starSlider.minValue, os.GetStarRating() / 5, timevalue);

            if (starSlider.value >= os.GetStarRating() / 5)
            {
                DisplayStars = false;
                textobj.SetActive(true);
                textobj.GetComponent<TextMeshProUGUI>().text = os.GetStarRating().ToString();
                GradeSection.SetActive(true);
            }

        }

        else if (EndOfDayAnimation)
        {
            if (!LoadGrade)
            {
                gradeSlider.minValue = level.GetGradeReq(index);
                minValue.GetComponent<TextMeshProUGUI>().text = gradeSlider.minValue.ToString();
                index++;
                gradeSlider.maxValue = level.GetGradeReq(index);
                maxValue.GetComponent<TextMeshProUGUI>().text = gradeSlider.maxValue.ToString();
                UpdateGrade((int)gradeSlider.minValue);

                if (index < 5)
                {
                    LoadGrade = true;
                }
                else
                {
                    EndOfDayAnimation = false;
                    SetSquid();
                    gradeSlider.minValue -= 1;
                    gradeSlider.value = gradeSlider.maxValue;
                    minValue.SetActive(false);
                    maxValue.GetComponent<TextMeshProUGUI>().text = "MAXED";
                }
            }

            else if (LoadGrade)
            {
                float timevalue = BarTimer / SliderDuration;

                float tempscore;
                if (score > level.SReq)
                    tempscore = level.SReq;
                else
                    tempscore = score;

                gradeSlider.value = Mathf.SmoothStep(0, tempscore, timevalue);
                BarTimer += Time.deltaTime;

                // if player exceed S grade req
                if (gradeSlider.value >= score || gradeSlider.value >= level.SReq)
                {
                    UpdateGrade((int)gradeSlider.maxValue);
                    SetSquid();
                    EndOfDayAnimation = false;
                    LoadGrade = false;

                    // Credit Obtained
                    CreditObtained = level.GetCredibility(GradeObtained);
                    ps.AddCredibility(CreditObtained);
                    level.RemoveCredibilityFromLevel(CreditObtained);
                    creditObtained.GetComponent<TextMeshProUGUI>().text = level.GetCreditText();
                    creditObtained.SetActive(true);
                }

                if (gradeSlider.value >= gradeSlider.maxValue)
                {
                    LoadGrade = false;
                    Debug.Log("stop");
                }
            }
        }

        // End the day and load to levelScene
        else if (GameStop)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (score > level.GetHighScore())
                {
                    level.SetHighScore(score);
                    level.SetHighestGrade(GradeObtained);
                }
                if (SaveFile.instance != null)
                    SaveFile.instance.SaveGame();

                SceneManager.LoadScene("Level Select");
            }
        }
    }

    private void Awake()
    {
        instance = this;
        // find the level
        for (int i = 0; i < lm.levelInfo.Count; i++)
        {
            if (lm.levelInfo[i].WhatDay == lm.DaySelected)
            {
                level = lm.levelInfo[i];
            }
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void ChangeScore(int changeamt)
    {
        // if it is negative, record it
        if (changeamt < 0)
            ScorePenalty -= changeamt;

        score += changeamt;
        UpdateScore();
    }

    void UpdateScore()
    {
        text.text = score.ToString();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScore();
    }

    public void StopOrders()
    {
        os.StopOrders();
    }

    public void EndDay()
    {
        playercontrols.DisablePlayerControls();
        SetResult();
        GameStop = true;
        DisplayDetails = true;
        DisplayStars = true;
        EndOfDayAnimation = true;
        EndOfDayBackground.SetActive(true);
    }

    void SetSquid()
    {
        Textbox.SetActive(true);
        switch (GradeObtained)
        {
            case 'S':
            case 'A':
                Textbox.GetComponentInChildren<TextMeshProUGUI>().text = HappyDialogue;
                Squid.GetComponent<Image>().sprite = HappySquid;
                break;

            case 'B':
            case 'C':
                Textbox.GetComponentInChildren<TextMeshProUGUI>().text = NeutralDialogue;
                Squid.GetComponent<Image>().sprite = NeutralSquid;
                break;

            default:
                Textbox.GetComponentInChildren<TextMeshProUGUI>().text = AngryDialogue;
                Squid.GetComponent<Image>().sprite = AngrySquid;
                break;
        }
    }

    void UpdateGrade(int thescore)
    {
        GradeObtained = level.GetGrade(thescore);

        switch (GradeObtained)
        {
            case 'S':
                Grade.GetComponent<Image>().sprite = Srank;
                break;
            case 'A':
                Grade.GetComponent<Image>().sprite = Arank;
                break;
            case 'B':
                Grade.GetComponent<Image>().sprite = Brank;
                break;
            case 'C':
                Grade.GetComponent<Image>().sprite = Crank;
                break;
            default:
                Grade.GetComponent<Image>().sprite = Frank;
                break;

        }
    }

    void SetResult()
    {
        // Setting the total completed order for the day
        ClearedText.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "COMPLETED x " + os.GetSuccessfulOrders();
        // Setting then total score obtaining from completed orders for the day
        ClearedText.GetComponentsInChildren<TextMeshProUGUI>()[1].text = (score + ScorePenalty).ToString();
        // Setting the total not completed order for the day
        FailedText.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "FAILED x " + os.GetFailedOrders();
        // Setting the total penalty score obtained from not completed orders for the day
        FailedText.GetComponentsInChildren<TextMeshProUGUI>()[1].text = ScorePenalty.ToString();
        // Setting the total score obtained
        TotalText.GetComponent<TextMeshProUGUI>().text = score.ToString();

        // setting the receipt print to null
        for (int i = 0; i < resultDisplay.Count; i++)
        {
            resultDisplay[i].SetActive(false);
        }
    }

    public Level GetLevelReference()
    {
        return level;
    }
}
