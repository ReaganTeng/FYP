using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
    public string LevelName;
    public string LevelDescription;
    public int HighScore;
    public char HighestGrade;
    public int SReq;
    public int AReq;
    public int BReq;
    public int CReq;
    public int CredibilityLeftToObtain;
    public bool Locked;
    public int CCReq;
    private int MaxCredibility = 4;
    public List<LevelHallway.Hallway> HallwaysUnlock;
    public OrderManager.DayOrder CustomizeOrderForThisDay;
    public List<SpawnerManager.SPAWNERTYPE> WhatSpawnerActive;
    public TutorialLevel isThereTutorial;
    // For the orders
    public int MaxOrders = 4;
    public float TimeBeforeFirstOrder = 10;
    public float IntervalBetweenOrders = 30;
    public float DayDuration = 300;

    enum Grades
    {
        FGRADE,
        CGRADE,
        BGRADE,
        AGRADE,
        SGRADE,
    }

    // Check to see the score that the player obtain and return the Grade that player obtained
    public char GetGrade(int scoreObtained)
    {
        // if the scoreobtained is above S, return S
        if (scoreObtained >= SReq)
            return 'S';

        // if the scoreobtained is above A, return A
        else if (scoreObtained >= AReq)
            return 'A';

        // if the scoreobtained is above B, return B
        else if (scoreObtained >= BReq)
            return 'B';

        // if the scoreobtained is above C, return C
        else if (scoreObtained >= CReq)
            return 'C';

        // if the scoreobtained is below C, return F
        else
            return 'F';
    }

    // 0 = F, 1 = C, 2 = B, 3 = A, 4 = S
    public int GetGradeReq(int gradeType)
    {
        switch (gradeType)
        {
            case 1:
                return CReq;
            case 2:
                return BReq;
            case 3:
                return AReq;
            case 4:
            case 5:
                return SReq;
            default:
                return 0;
        }
    }

    public int GetCredibility(char GradeObtained)
    {
        // Temp value of credit
        int CreditObtained = 0;
        // Check to see if the grade obtained is better than
        int HighestGradeValue = GetGradeValue(HighestGrade);
        int CurrentGradeValue = GetGradeValue(GradeObtained);

        // If player obtained a higher grade than what they have
        if (CurrentGradeValue > HighestGradeValue)
        {
            // Get the cc from the level
            int creditleftinLevel = CredibilityLeftToObtain - CurrentGradeValue;

            // if the credibility to obtain exceed what it has, return what it has at the moment.
            if (creditleftinLevel < 0)
            {
                CreditObtained = CredibilityLeftToObtain;
            }
            // if it does not exceed, return that instead
            else
            {
                CreditObtained = CurrentGradeValue;
            }
        }

        // return that amount back to the player
        return CreditObtained;
    }

    public void RemoveCredibilityFromLevel(int amt)
    {
        CredibilityLeftToObtain -= amt;
    }

    public int GetMaxCredibility()
    {
        return MaxCredibility;
    }

    int GetGradeValue(char grades)
    {
        switch (grades)
        {
            case 'F':
                return (int)Grades.FGRADE;
            case 'C':
                return (int)Grades.CGRADE;
            case 'B':
                return (int)Grades.BGRADE;
            case 'A':
                return (int)Grades.AGRADE;
            case 'S':
                return (int)Grades.SGRADE;
            default:
                return -1;
        }
    }

    public string GetCreditText()
    {
        string theText = "";

        if (CredibilityLeftToObtain == 0)
        {
            theText += "MAXED";
        }
        else
        {
            theText += (MaxCredibility - CredibilityLeftToObtain).ToString() + "/" + MaxCredibility.ToString();
        }

        theText += " CC OBTAINED";

        return theText;
    }

    public void ResetLevel(bool LockLevel)
    {
        HighestGrade = 'N';
        HighScore = 0;
        CredibilityLeftToObtain = MaxCredibility;
        if (LockLevel)
            Locked = true;
    }
}
