using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockHallway : MonoBehaviour
{
    [SerializeField] LevelManager lm;

    private void Start()
    {
        int CurrentDay = gameObject.GetComponent<EndOfDay>().GetCurrentDay();

        if (CurrentDay < lm.levelInfo.Count)
        {
            for (int i = 0; i < lm.levelInfo.Count; i++)
            {
                if (i == CurrentDay - 1)
                {
                    for (int index = 0; index < lm.levelInfo[i].HallwaysUnlock.Count; index++)
                    {
                        LevelHallway.instance.OpenHallway(lm.levelInfo[i].HallwaysUnlock[index]);
                    }
                }
            }
        }
    }
}
