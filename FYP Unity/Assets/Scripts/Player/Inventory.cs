using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] int MaxInventorySize;
    List<Item> InventoryList = new List<Item>();
    public bool InventoryFull = false;

    public bool AddToInventory(Item ItemToAdd)
    {
        // Check to see if there is slots to put item into inventory
        if (InventoryList.Count < MaxInventorySize)
        {
            InventoryList.Add(ItemToAdd);
            IsInventoryFull();
            // link the UI to here, and add the item into the UI

            return true;
        }

        return false;
    }

    public void RemoveFromInventory(Item ItemToRemove)
    {
        InventoryList.Remove(ItemToRemove);
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
}
