using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public class inventory
    {
        public GameObject food;
        public bool IsPerfect;
        public bool IsDish;
        public int Stars;
    }

    public static Inventory instance;
    public PlayerProgress playerProgress;
    private int MaxInventorySize;


    List<inventory> InventoryList = new List<inventory>();
    public bool InventoryFull = false;

    public bool AddToInventory(GameObject ItemToAdd)
    {
        // Check to see if there is slots to put item into inventory
        if (InventoryList.Count < MaxInventorySize)
        {
            inventory tempinv = new inventory();
            tempinv.food = ItemToAdd;
            tempinv.IsPerfect = false;
            tempinv.Stars = 0;

            // check to see if the item added is an ingredient or a dish
            if (ItemToAdd.GetComponent<Food>().GetFoodType() == FoodManager.FoodType.DISH)
            {
                tempinv.IsDish = true;
                // Assign the amount of stars into the item
                tempinv.Stars = ItemToAdd.GetComponent<Food>().GetAmtOfStars();
            }
            else
            {
                tempinv.IsDish = false;
                // check to see if it is a perfect ingredient
                if (ItemToAdd.GetComponent<Food>().GetIsPerfect())
                {
                    tempinv.IsPerfect = true;
                }
            }
            InventoryList.Add(tempinv);
            IsInventoryFull();
            return true;
        }

        return false;
    }

    public void RemoveFromInventory(int Itempos, bool DeleteReference)
    {
        GameObject tempobj = InventoryList[Itempos].food;

        InventoryList.RemoveAt(Itempos);

        if (DeleteReference)
        {
            if (tempobj != null)
            {
                Destroy(tempobj);
            }
        }

        IsInventoryFull();
    }

    public void AddMaxInventorySize()
    {
        MaxInventorySize += 1;
    }

    void IsInventoryFull()
    {
        if (InventoryList.Count >= MaxInventorySize)
            InventoryFull = true;
        else
            InventoryFull = false;
    }

    public int GetMaxInventorySize()
    {
        return MaxInventorySize;
    }

    public List<inventory> GetList()
    {
        return InventoryList;
    }

    private void Awake()
    {
        instance = this;
        MaxInventorySize = playerProgress.GetInventorySize();
    }
}