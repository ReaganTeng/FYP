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
        SUSHI, //2
        STEAK, //3
        CRUNCHY_SALAD,
        FRANTIC_CHICKEN_RICE,
        TAINTED_FRIED_RICE,
        APPLE_DESSERT,
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
