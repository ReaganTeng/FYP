using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    [SerializeField] DishManager.dishes dishtype;
    [SerializeField] string dishName;
    [SerializeField] Sprite image;
    [SerializeField] int BaseScore;
    private int dishID;

    private void Start()
    {
        dishID = DishManager.instance.GetItemID(dishtype);
    }

    public void SetValues(Dish dish)
    {
        this.dishtype = dish.dishtype;
        this.dishName = dish.dishName;
        this.image = dish.image;
        this.BaseScore = dish.BaseScore;
        this.dishID = dish.dishID;
    }

    public DishManager.dishes GetItemType()
    {
        return dishtype;
    }

    public int GetItemID()
    {
        dishID = DishManager.instance.GetItemID(dishtype);
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

    public int GetScore()
    {
        return BaseScore;
    }
}
