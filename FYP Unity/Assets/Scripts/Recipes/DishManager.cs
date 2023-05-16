using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishManager : MonoBehaviour
{
    public static DishManager instance;

    public enum dishes
    {
        SUS_DISH, //0
        POTATO_SALAD, //1
        APOLO_DANGO, //2
        SUSHI, //3
        REE_GORENG, //4
        VEGETERIAN_SALAD, //5
        RAINBOW_OVERDOSE, //6
        CRYING_EGG_FRIED_RICE, //7
        PASGA_LA_VIDA, //8
        UNHAPPY_MEAL, //9
    }

    public enum Difficulty
    {
        EASY,
        NORMAL,
        HARD,
        EXTREME
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
