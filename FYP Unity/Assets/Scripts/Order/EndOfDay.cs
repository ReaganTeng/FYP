using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndOfDay : MonoBehaviour
{
    [SerializeField] OrderSystem os;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] int score;
    [SerializeField] LevelManager lm;
    private int currentday;
    PlayerMovement playercontrols;
    private char GradeObtained;
    private bool EndOfDayAnimation;

    [SerializeField] GameObject EndOfDayBackground;

    [SerializeField] GameObject Textbox;
    [SerializeField] string HappyDialogue;
    [SerializeField] string NeutralDialogue;
    [SerializeField] string AngryDialogue;

    [SerializeField] GameObject Squid;
    [SerializeField] Sprite HappySquid;
    [SerializeField] Sprite NeutralSquid;
    [SerializeField] Sprite AngrySquid;

    [SerializeField] GameObject ClearedText;
    [SerializeField] GameObject FailedText;

    [SerializeField] GameObject Grade;
    [SerializeField] Sprite Srank;
    [SerializeField] Sprite Arank;
    [SerializeField] Sprite Brank;
    [SerializeField] Sprite Crank;
    [SerializeField] Sprite Frank;

    [SerializeField] Slider gradeSlider;
    [SerializeField] float SliderDuration;
    int index;
    bool LoadGrade = false;
    float BarTimer = 0;

    private void Start()
    {
        text.text = "Score: 0";
        playercontrols = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        EndOfDayBackground.SetActive(false);
        EndOfDayAnimation = false;
        index = 0;
    }

    private void Update()
    {
        if (EndOfDayAnimation)
        {
            if (!LoadGrade)
            {
                gradeSlider.minValue = lm.levelInfo[lm.DaySelected - 1].GetGradeReq(index);
                index++;
                gradeSlider.maxValue = lm.levelInfo[lm.DaySelected - 1].GetGradeReq(index);
                SetResults((int)gradeSlider.minValue);

                if (index < 5)
                {
                    LoadGrade = true;
                    BarTimer = 0;
                }
                else
                {
                    EndOfDayAnimation = false;
                    gradeSlider.minValue -= 1;
                    gradeSlider.value = gradeSlider.maxValue;
                }
            }

            else if (LoadGrade)
            {
                if (gradeSlider.value > score)
                {
                    EndOfDayAnimation = false;
                    LoadGrade = false;
                }

                float timevalue = BarTimer / SliderDuration;
                gradeSlider.value = Mathf.Lerp(gradeSlider.minValue, gradeSlider.maxValue, timevalue);
                BarTimer += Time.deltaTime;

                if (BarTimer >= SliderDuration)
                {
                    LoadGrade = false;
                    Debug.Log("stop");
                }
            }
        }
    }

    private void Awake()
    {
        currentday = lm.DaySelected;
    }

    public int GetScore()
    {
        return score;
    }

    public void ChangeScore(int changeamt)
    {
        score += changeamt;
        UpdateScore();
    }

    void UpdateScore()
    {
        text.text = "Score: " + score.ToString();
    }

    public int GetCurrentDay()
    {
        return currentday;
    }

    public void StopOrders()
    {
        os.StopOrders();
    }

    public void EndDay()
    {
        playercontrols.DisablePlayerControls();
        EndOfDayAnimation = true;
        EndOfDayBackground.SetActive(true);
    }

    void SetResults(int thescore)
    {
        //GradeObtained = lm.levelInfo[lm.DaySelected - 1].GetGrade(score);
        GradeObtained = lm.levelInfo[lm.DaySelected - 1].GetGrade(thescore);
        ClearedText.GetComponentsInChildren<TextMeshProUGUI>()[1].text = os.GetSuccessfulOrders().ToString();
        FailedText.GetComponentsInChildren<TextMeshProUGUI>()[1].text = os.GetFailedOrders().ToString();

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
}
