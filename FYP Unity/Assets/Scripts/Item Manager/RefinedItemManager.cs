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
        DANGO, //3
        SOOSHI, //4
        SAAMONO_OVERLOAD, //5
        CAKE_MIX, //6
        NOODLE, //7
        MSG, //8
        APOLO_SALAD, //9
        KAROOT_SALAD, //10
        FRUIT_PUNCH, //11
        MEATY_TREAT, //12
        WC_BONDALDS, //13
        SALTY_RICE, //14
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
