using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTE : MonoBehaviour
{
    public static QTE instance;
    [SerializeField] GameObject TheSlider;
    [SerializeField] float speed;
    [SerializeField] float OffSet;
    [SerializeField] GameObject GreatBG;
    [SerializeField] GameObject PerfectBG;
    Slider slider;
    float GreatStart;
    [SerializeField] float GreatRange;
    float PerfectStart;
    [SerializeField] float PerfectRange;
    float UnitPer;

    private float needletravel;
    private bool StopNeedle = false;

    GameObject Cooker;
    GameObject TheDish;
    PlayerMovement playercontrols;

    [SerializeField] Sprite GoodHit;
    [SerializeField] Sprite GreatHit;
    [SerializeField] Sprite PerfectHit;

    [SerializeField] GameObject TheText;
    [SerializeField] float QTEResultMaxTime;
    float QTEResulttimer;
    bool DisplayResult;

    private void Start()
    {
        TheSlider.SetActive(false);
        instance = this;
        slider = TheSlider.GetComponent<Slider>();
        GreatStart = 0;
        PerfectStart = 0;
        UnitPer = ((slider.gameObject.GetComponent<RectTransform>().rect.width - OffSet * 2)/slider.maxValue);
        SetArea();
        playercontrols = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        QTEResulttimer = QTEResultMaxTime;
        DisplayResult = false;
        TheText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (TheSlider.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DisplayResult = true;
                StopNeedle = true;
            }

            if (!StopNeedle)
                slider.value += needletravel * Time.deltaTime;


            if (slider.value == slider.minValue)
            {
                needletravel = speed;
            }

            else if (slider.value == slider.maxValue)
            {
                needletravel = -speed;
            }

            if (DisplayResult)
            {
                if (QTEResulttimer == QTEResultMaxTime)
                {
                    CheckHitWhatArea();
                }

                QTEResulttimer -= Time.deltaTime;

                if (QTEResulttimer <= 0)
                {
                    EndQTE();
                    DisplayResult = false;
                    QTEResulttimer = QTEResultMaxTime;
                    TheText.SetActive(false);
                }
            }
        }
    }

    public void StartQTE(GameObject thecooker, GameObject thedish)
    {
        StopNeedle = false;
        playercontrols.DisablePlayerControls();
        Cooker = thecooker;
        TheDish = thedish;
        SetArea();
        TheSlider.SetActive(true);
    }

    void EndQTE()
    {
        Cooker.GetComponent<Mixer>().GetDishFromMixer();
        TheSlider.SetActive(false);
        playercontrols.EnablePlayerControls();
    }

    public void IncreaseGreatZone(float IncreaseBy)
    {
        GreatRange += IncreaseBy;
    }

    public void IncreasePerfectZone(float IncreaseBy)
    {
        PerfectRange += IncreaseBy;
    }

    // set the area for the great and perfect bar to appear in
    void SetArea()
    {
        GreatStart = Random.Range(5, 96 - GreatRange);

        GreatBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(GreatStart * UnitPer, 0);
        GreatBG.GetComponent<RectTransform>().sizeDelta = new Vector2(GreatRange * UnitPer, 100);

        PerfectStart = GreatStart + GreatRange / 2 - PerfectRange / 2;

        PerfectBG.GetComponent<RectTransform>().anchoredPosition = new Vector2(PerfectStart * UnitPer, 0);
        PerfectBG.GetComponent<RectTransform>().sizeDelta = new Vector2(PerfectRange * UnitPer, 100);
    }

    void CheckHitWhatArea()
    {
        float GreatEnd = GreatStart + GreatRange;
        float PerfectEnd = PerfectStart + PerfectRange;

        // if it is in range of perfect, do something
        if (slider.value > PerfectStart && slider.value < PerfectEnd)
        {
            TheDish.GetComponent<Food>().SetAmountOfStars(TheDish.GetComponent<Food>().GetAmtOfStars() + 2);
            TheText.GetComponent<Image>().sprite = PerfectHit;
            Debug.Log("Perfect Hit!");
        }

        // else if it is in range of great, do something
        else if (slider.value > GreatStart && slider.value < GreatEnd)
        {
            TheDish.GetComponent<Food>().SetAmountOfStars(TheDish.GetComponent<Food>().GetAmtOfStars() + 1);
            TheText.GetComponent<Image>().sprite = GreatHit;
            Debug.Log("Great Hit!");
        }

        // else do something
        else
        {
            TheText.GetComponent<Image>().sprite = GoodHit;
            Debug.Log("You No Skillz!");
        }

        TheText.SetActive(true);
    }
}
