using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] FoodManager.FoodType foodtype;
    private int foodtypeid;
    private bool perfectdish = false;
    private int amtOfStars = 0;

    private void Start()
    {
        SetType();
    }

    public void SetValues(Food food)
    {
        this.foodtype = food.foodtype;
        this.foodtypeid = food.foodtypeid;
        this.perfectdish = food.perfectdish;
        this.amtOfStars = food.amtOfStars;
    }

    public void SetValues(FoodManager.FoodType whatType, int FoodTypeID)
    {
        this.foodtype = whatType;
        this.foodtypeid = FoodTypeID;
    }

    public FoodManager.FoodType GetFoodType()
    {
        return foodtype;
    }

    public int GetFoodID()
    {
        SetType();
        return foodtypeid;
    }

    // to fix id not being called when reference to
    public void SetType()
    {
        foodtypeid = FoodManager.instance.GetFoodID(foodtype);
    }

    public void SetPerfect(bool num)
    {
        perfectdish = num;
    }

    public void SetAmountOfStars(int amt)
    {
        amtOfStars = amt;
    }

    public bool GetIsPerfect()
    {
        return perfectdish;
    }

    public int GetAmtOfStars()
    {
        return amtOfStars;
    }
}
