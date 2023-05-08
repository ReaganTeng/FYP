using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager instance;
    [SerializeField] LevelManager lm;

    private void Awake()
    {
        instance = this;
    }

    public enum SPAWNERTYPE
    {
        OOTATOO,
        RAISUU,
        KAAROOT,
        EYGOO,
        MOOMOO,
        JIKKEN,
        SAAMONO,
        AAPOLO,
        SPAANICHO,
        ALL,
    }

    [System.Serializable]
    private class spawner
    {
        public GameObject whatSpawner;
        public SPAWNERTYPE spawnerType;
    }

    [SerializeField] List<spawner> spawnerList;

    public void SetSpawner(SPAWNERTYPE whichSpawner, bool active)
    {
        bool All = false;
        if (whichSpawner == SPAWNERTYPE.ALL)
            All = true;

        for (int i = 0; i < spawnerList.Count; i++)
        {
            if (spawnerList[i].spawnerType == whichSpawner || All)
            {
                //spawnerList[i].whatSpawner.GetComponent<Spawner>().SetEnable(active);

                spawnerList[i].whatSpawner.GetComponent<Spawner>().SetEnable(true);
            }
        }
    }

    public GameObject GetSpawner(SPAWNERTYPE whichSpawner)
    {
        for (int i = 0; i < spawnerList.Count; i++)
        {
            if (spawnerList[i].spawnerType == whichSpawner)
            {
                return spawnerList[i].whatSpawner;
            }
        }
        return null;
    }

    private void Start()
    {
        if (!Tutorial.instance.InTutorial)
        {
            for (int i = 0; i < lm.levelInfo[lm.DaySelected - 1].WhatSpawnerActive.Count; i++)
            {
                SetSpawner(lm.levelInfo[lm.DaySelected - 1].WhatSpawnerActive[i], true);
            }
        }
    }
}
