using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public static FoodManager instance;

    public enum FoodType
    {
        INGREDIENT, //0
        REFINED_INGREDIENT, //1
        DISH //2
    }

    public FoodType GetFoodType(int foodID)
    {
        return (FoodType)foodID;
    }

    public int GetFoodID(FoodType food)
    {
        return (int)food;
    }

    private void Awake()
    {
        instance = this;
    }

    public int GetItemID(GameObject food)
    {
        // Get the type of the gameobject

        FoodType ftype = food.GetComponent<Food>().GetFoodType();
        int foodid = -1;

        // call their corresponding id and get it
        switch (ftype)
        {
            case FoodType.INGREDIENT:
                foodid =  food.GetComponent<Item>().GetItemID();
                break;

            case FoodType.REFINED_INGREDIENT:
                foodid = food.GetComponent<RefinedItem>().GetItemID();
                break;

            case FoodType.DISH:
                foodid = food.GetComponent<Dish>().GetItemID();
                break;
        }

        return foodid;
    }

    public Sprite GetImage(GameObject food)
    {
        FoodType ftype = food.GetComponent<Food>().GetFoodType();
        Sprite foodimage = null;

        // call their corresponding id and get it
        switch (ftype)
        {
            case FoodType.INGREDIENT:
                foodimage = food.GetComponent<Item>().GetImage();
                break;

            case FoodType.REFINED_INGREDIENT:
                foodimage = food.GetComponent<RefinedItem>().GetImage();
                break;

            case FoodType.DISH:
                foodimage = food.GetComponent<Dish>().GetImage();
                break;
        }

        return foodimage;
    }

    public string GetDishName(GameObject food)
    {
        FoodType ftype = food.GetComponent<Food>().GetFoodType();
        string DishName = "";

        // if the foodtype is a dish, get the dish name
        if (ftype == FoodType.DISH)
        {
            DishName = food.GetComponent<Dish>().GetDishName();
        }

        return DishName;
    }
}
