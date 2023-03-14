using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] int itemID;
    private ItemManager.Items itemtype;

    private void Start()
    {
        itemtype = ItemManager.instance.GetItemType(itemID);
    }

    public ItemManager.Items GetItemType()
    {
        return itemtype;
    }

    public int GetItemID()
    {
        return itemID;
    }
}
