using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfDay : MonoBehaviour
{
    [SerializeField] int score;

    public int GetScore()
    {
        return score;
    }

    public void ChangeScore(int changeamt)
    {
        score += changeamt;
    }
}
