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
        APPLE_MIX, //2
        STARCHY_RICE, //3
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
