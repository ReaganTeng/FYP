using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] ItemManager.Items itemtype;
    [SerializeField] ItemManager.ItemVariant itemVariant;
    [SerializeField] Sprite image;
    private int itemID;

    private void Start()
    {
        itemID = ItemManager.instance.GetItemID(itemtype);
    }

    public ItemManager.Items GetItemType()
    {
        return itemtype;
    }

    public int GetItemID()
    {
        itemID = ItemManager.instance.GetItemID(itemtype);
        return itemID;
    }

    public Sprite GetImage()
    {
        return image;
    }

    public void SetValueManually(GameObject itemReference)
    {
        itemtype = itemReference.GetComponent<Item>().GetItemType();
        image = itemReference.GetComponent<Item>().GetImage();
    }

    public ItemManager.ItemVariant GetItemVariant()
    {
        return itemVariant;
    }
}
