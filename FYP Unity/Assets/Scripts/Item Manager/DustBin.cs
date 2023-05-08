using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustBin : MonoBehaviour
{
    List<Food> dustbinException = new List<Food>();

    public void RemoveItem()
    {
        InventoryImageControl inv = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InventoryImageControl>();

        if (!CheckIfAnException(inv.GetSelectedGameObject()))
            inv.RemoveSelected(true);
    }

    // Add an exception to what can be thrown
    public void AddException(FoodManager.FoodType whatType, int foodID)
    {
        Food tempFood = new Food();
        tempFood.SetValues(whatType, foodID);
        dustbinException.Add(tempFood);
    }


    // remove an exception to that fooditem, amking it throwable
    public void RemoveException(FoodManager.FoodType whatType, int foodID)
    {
        for (int i = 0; i < dustbinException.Count; i++)
        {
            if (dustbinException[i].GetFoodType() == whatType && dustbinException[i].GetFoodID() == foodID)
            {
                dustbinException.RemoveAt(i);
            }
        }
    }

    bool CheckIfAnException(GameObject foodCheck)
    {
        Food checkFood = foodCheck.GetComponent<Food>();

        for(int i = 0; i < dustbinException.Count; i++)
        {
            if (dustbinException[i].GetFoodType() == checkFood.GetFoodType() && dustbinException[i].GetFoodID() == checkFood.GetFoodID())
            {
                return true;
            }
        }

        return false;
    }
}