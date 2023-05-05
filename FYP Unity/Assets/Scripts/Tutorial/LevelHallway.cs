using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHallway : MonoBehaviour
{
    public static LevelHallway instance;

    public enum Hallway
    {
        KITCHEN_TO_OOTATOO,
        KITCHEN_TO_AAPOLO,
        KITCHEN_TO_KAAROOT,
        KITCHEN_TO_MOOMOO,
        KITCHEN_TO_SPAANICHO,
        OOTATOO_TO_SAAMONO,
        OOTATOO_TO_RAISUU,
        RAISUU_TO_EGGYO,
        EGGYO_TO_JIKKEN,
        KARAROOT_TO_JIKKEN,
        SAAMONO_TO_AAPOLO,
        AAPOLO_TO_MOOMOO,
        KAROOT_TO_SPAANICHO,
        JIKKEN_TO_SPAANICHO,
        ALL,
    }


    [System.Serializable]
    public class Hallways
    {
        public string HallwayName;
        public List<GameObject> doors;
        public Hallway HallwayType;
    }

    [SerializeField] List<Hallways> hallwayList;

    public List<Hallways> GetHallwayList()
    {
        return hallwayList;
    }

    public void OpenHallway(Hallway type)
    {
        bool OpenAll = false;

        if (type == Hallway.ALL)
        {
            OpenAll = true;
        }

        for (int index = 0; index < hallwayList.Count; index++)
        {
            if (hallwayList[index].HallwayType == type || OpenAll)
            {
                for (int i = 0; i < hallwayList[index].doors.Count; i++)
                {
                    hallwayList[index].doors[i].SetActive(false);
                }
            }
        }
    }

    public void CloseHallway(Hallway type)
    {
        bool CloseAll = false;

        if (type == Hallway.ALL)
        {
            CloseAll = true;
        }

        for (int index = 0; index < hallwayList.Count; index++)
        {
            if (hallwayList[index].HallwayType == type || CloseAll)
            {
                for (int i = 0; i < hallwayList[index].doors.Count; i++)
                {
                    hallwayList[index].doors[i].SetActive(true);
                }
            }
        }
    }

    private void Awake()
    {
        CloseHallway(Hallway.ALL);
        //OpenHallway(Hallway.ALL);

        instance = this;
    }
}
