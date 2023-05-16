using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockHallway : MonoBehaviour
{
    [SerializeField] LevelManager lm;

    private void Start()
    {
        for (int index = 0; index < EndOfDay.instance.GetLevelReference().HallwaysUnlock.Count; index++)
        {
            LevelHallway.instance.OpenHallway(EndOfDay.instance.GetLevelReference().HallwaysUnlock[index]);
        }
    }

    public void LockHallway(LevelHallway.Hallway whichHallway)
    {
        LevelHallway.instance.CloseHallway(whichHallway);
    }

    public void OpenHallway(LevelHallway.Hallway whichHallway)
    {
        LevelHallway.instance.OpenHallway(whichHallway);
    }
}
