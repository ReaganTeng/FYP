using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] int itemID;
    [SerializeField] Sprite image;
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

    public Sprite GetImage()
    {
        return image;
    }
}
