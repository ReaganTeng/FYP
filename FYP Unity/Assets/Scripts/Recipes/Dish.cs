using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    [SerializeField] int dishID;
    [SerializeField] string dishName;
    [SerializeField] Sprite image;
    [SerializeField] int BaseScore;
    private DishManager.dishes dishtype;

    private void Start()
    {
        dishtype = DishManager.instance.GetItemType(dishID);
    }

    public DishManager.dishes GetItemType()
    {
        return dishtype;
    }

    public int GetItemID()
    {
        return dishID;
    }

    public string GetDishName()
    {
        return dishName;
    }

    public Sprite GetImage()
    {
        return image;
    }
}
