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
}
