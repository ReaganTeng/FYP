using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndOfDay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] int score;

    private void Start()
    {
        text.text = "Score: 0";
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
}
