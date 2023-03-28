using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefinedItemManager : MonoBehaviour
{
    public static RefinedItemManager instance;

    public enum RItems
    {
        MUSHY, //0
        MASHED_POTATO_CUP, //1
        MAYONNAISE, //2
        SALAD_BOWL, //3
        BOILED_POTATO, //4
    }

    public RItems GetItemType(int itemID)
    {
        return (RItems)itemID;
    }

    public int GetItemID(RItems item)
    {
        return (int)item;
    }

    private void Awake()
    {
        instance = this;
    }
}