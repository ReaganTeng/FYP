using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayLevelSelect : MonoBehaviour
{
    public Level level;
    [SerializeField] TextMeshPro levelname;
    [SerializeField] TextMeshPro leveldescription;
    [SerializeField] GameObject HighestScore;
    [SerializeField] GameObject levelSelect;
    [SerializeField] GameObject levelLocked;
    [SerializeField] GameObject DisplayGrade;
    [SerializeField] GameObject PlayButton;

    [SerializeField] Sprite Srank;
    [SerializeField] Sprite Arank;
    [SerializeField] Sprite Brank;
    [SerializeField] Sprite Crank;
    [SerializeField] Sprite Frank;

    // Start is called before the first frame update
    void Start()
    {
        if (level.Locked)
        {
            levelSelect.SetActive(false);
            levelLocked.SetActive(true);
            PlayButton.SetActive(false);
        }
        else
        {
            levelSelect.SetActive(true);
            levelLocked.SetActive(false);
            PlayButton.SetActive(true);
        }

        levelname.text = level.LevelName;
        leveldescription.text = level.LevelDescription;
        DisplayGradeOnUI();

        if (level.HighScore != 0)
        {
            HighestScore.GetComponent<TextMeshPro>().text = "HighScore: " + level.HighScore;
        }
        else
            HighestScore.SetActive(false);
    }

    void DisplayGradeOnUI()
    {
        switch (level.HighestGrade)
        {
            case 'S':
                DisplayGrade.GetComponent<SpriteRenderer>().sprite = Srank;
                break;
            case 'A':
                DisplayGrade.GetComponent<SpriteRenderer>().sprite = Arank;
                break;
            case 'B':
                DisplayGrade.GetComponent<SpriteRenderer>().sprite = Brank;
                break;
            case 'C':
                DisplayGrade.GetComponent<SpriteRenderer>().sprite = Crank;
                break;
            case 'F':
                DisplayGrade.GetComponent<SpriteRenderer>().sprite = Frank;
                break;
            default:
                DisplayGrade.SetActive(false);
                break;
        }
    }
}
