using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishManager : MonoBehaviour
{
    public static DishManager instance;

    public enum dishes
    {
        SUS_DISH, //0
        MASH_POTATO, //1
        APOLO_CAKE,
    }

    public dishes GetItemType(int itemID)
    {
        return (dishes)itemID;
    }

    public int GetItemID(dishes dish)
    {
        return (int)dish;
    }


    void Awake()
    {
        instance = this;
    }
}
