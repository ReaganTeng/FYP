using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    public enum Items
    {
        MUSH, //0
        POTATO_CHOPPED, // 1
        POTATO_MASHED, //2
        POTATO_PREP, //3
        RICE_CHOPPED, //4
        RICE_MASHED, //5
        RICE_PREP, //6
        EGG_CHOPPED, //7
        EGG_MASHED, //8
        EGG_PREP, //9
        SALMON_CHOPPED, //10
        SALMON_MASHED, //11
        SALMON_PREP, //12
        CHICKEN_CHOPPED, //13
        CHICKEN_MASHED, //14
        CHICKEN_PREP, //15
        COW_CHOPPED, //16
        COW_MASHED, //17
        COW_PREP, //18
        CARROT_CHOPPED, //19
        CARROT_MASHED, //20
        CARROT_PREP, //21
        APPLE_CHOPPED, //22
        APPLE_MASHED, //23
        APPLE_PREP, //24
        SPANICH_CHOPPED, //25
        SPANICH_MASHED, //26
        SPANICH_PREP, //27
        FLOUR, //28
        SEAWEED, //29
        TOTAL, //30
    }

    public Items GetItemType(int itemID)
    {
        return (Items)itemID;
    }

    public int GetItemID(Items item)
    {
        return (int)item;
    }

    private void Awake()
    {
        instance = this;
    }
}
