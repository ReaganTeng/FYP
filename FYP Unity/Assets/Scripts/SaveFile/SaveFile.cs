using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFile : MonoBehaviour
{
    public static SaveFile instance;

    // The list that store the saves for the level details
    [SerializeField] LevelManager lm;
    [SerializeField] ShopManager sm;
    [SerializeField] PlayerProgress pp;

    public void SaveGame()
    {
        // Save the details of each level select
        for (int i = 0; i < lm.levelInfo.Count; i++)
        {
            // NOTE!!! DO NOT CHANGE ANY KEY NAMING, IF U DO, RESET PLAYERPREF PLS
            string highScoreKey = "LevelHighScore" + (i + 1).ToString();
            PlayerPrefs.SetInt(highScoreKey, lm.levelInfo[i].GetHighScore());
            string highestGradeKey = "LevelHighestGrade" + (i + 1).ToString();
            PlayerPrefs.SetString(highestGradeKey, (lm.levelInfo[i].GetHighestGrade()).ToString());
        }

        // Save the details of each shop manager
        for (int i = 0; i < sm.shopList.Count; i++)
        {
            // NOTE!!! DO NOT CHANGE ANY KEY NAMING, IF U DO, RESET PLAYERPREF PLS
            string upgradeLevelKey = "ShopLevelKey" + (i + 1).ToString();
            PlayerPrefs.SetInt(upgradeLevelKey, sm.shopList[i].GetCurrentLevel());
        }

        // Save the details of the player stats
        // NOTE!!! DO NOT CHANGE ANY KEY NAMING, IF U DO, RESET PLAYERPREF PLS
        PlayerPrefs.SetInt("CC", pp.GetCurrentCC()); // the CC currency
        PlayerPrefs.SetInt("REP", pp.GetMaxCC()); // the Max CC aka the rep points
    }


    public void LoadSave()
    {
        // Note to self, allocate the cc lefttobtained later
        // Load in the details of each level select
        lm.ResetGameLevel();
        for (int i = 0; i < lm.levelInfo.Count; i++)
        {
            // NOTE!!! DO NOT CHANGE ANY KEY NAMING, IF U DO, RESET PLAYERPREF PLS
            string highScoreKey = "LevelHighScore" + (i + 1).ToString();
            lm.levelInfo[i].SetHighScore(PlayerPrefs.GetInt(highScoreKey , 0));
            string highestGradeKey = "LevelHighestGrade" + (i + 1).ToString();
            // Get what creditobtained from the level first before setting the grade
            string HighGrade = PlayerPrefs.GetString(highestGradeKey, "N");
            char HighestGrade = HighGrade[0];

            int CreditObtained = lm.levelInfo[i].GetCredibility(HighestGrade);
            lm.levelInfo[i].SetHighestGrade(HighestGrade);
            lm.levelInfo[i].RemoveCredibilityFromLevel(CreditObtained);
        }

        // Get the shop lvl
        sm.ResetShopLevel();
        for (int i = 0; i < sm.shopList.Count; i++)
        {
            // NOTE!!! DO NOT CHANGE ANY KEY NAMING, IF U DO, RESET PLAYERPREF PLS
            string upgradeLevelKey = "ShopLevelKey" + (i + 1).ToString();
            sm.shopList[i].SetLevel(PlayerPrefs.GetInt(upgradeLevelKey, 0));
        }
        // Assign the upgrades accordingly
        sm.LoadUpgrade = true;

        // Get the player stats
        // NOTE!!! DO NOT CHANGE ANY KEY NAMING, IF U DO, RESET PLAYERPREF PLS
        pp.ResetPlayer();
        int Rep = PlayerPrefs.GetInt("REP", 0);
        int CC = PlayerPrefs.GetInt("CC", 2);
        int RepAndCCDiff = Rep + 2 - CC;
        pp.AddCredibility(Rep);
        pp.DecreaseCredibility(RepAndCCDiff);
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
    }
}
