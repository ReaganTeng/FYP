using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] PlayerProgress pp;
    [SerializeField] LevelManager lm;
    [SerializeField] TextMeshProUGUI Currency;
    [SerializeField] TextMeshProUGUI Reputation;
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }


    void CheckIfCanUnlock()
    {
        for (int i = 0; i < lm.levelInfo.Count; i++)
        {
            // check to see if any lvl are locked, if they are locked check to see if it can be unlocked
            if (lm.levelInfo[i].Locked)
            {
                if (pp.GetMaxCC() >= lm.levelInfo[i].CCReq)
                {
                    lm.levelInfo[i].Locked = false;
                }
            }
        }
    }

    public void UpdateUI()
    {
        CheckIfCanUnlock();
        Currency.text = pp.GetCurrentCC().ToString();
        Reputation.text = pp.GetMaxCC().ToString();
    }
}
