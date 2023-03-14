using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    public enum Items
    {
        POTATO_CHOPPED,
        POTATO_MASHED,
        POTATO_PREP,
        RICE_CHOPPED,
        RICE_MASHED,
        RICE_PREP,
        EGG_CHOPPED,
        EGG_MASHED,
        EGG_PREP,
        SALMON_CHOPPED,
        SALMON_MASHED,
        SALMON_PREP,
        CHICKEN_CHOPPED,
        CHICKEN_MASHED,
        CHICKEN_PREP,
        COW_CHOPPED,
        COW_MASHED,
        COW_PREP,
        CARROT_CHOPPED,
        CARROT_MASHED,
        CARROT_COOKED,
        APPLE_CHOPPED,
        APPLE_MASHED,
        APPLE_COOKED,
        SPANICH_CHOPPED,
        SPANICH_MASHED,
        SPANICH_PREP,
        TOTAL,
    }

    public Items GetItemType(int itemID)
    {
        return (Items)itemID;
    }

    public int GetItemID(Items item)
    {
        return (int)item;
    }

    private void Start()
    {
        instance = this;
    }
}
