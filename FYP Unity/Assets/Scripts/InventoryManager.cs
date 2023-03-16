using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour
{
    private Item_Reagan currentItem;
    public Image customCursor;


    public List<Item_Reagan> itemList;

    public Slots[] InventorySlots;


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            AddItem(itemList[0]);

        }
        else
        if (Input.GetKeyDown(KeyCode.R))
        {
            AddItem(itemList[1]);

        }
        else
        if (Input.GetKeyDown(KeyCode.T))
        {
            AddItem(itemList[2]);

        }
        else
        if (Input.GetKeyDown(KeyCode.Y))
        {
            AddItem(itemList[3]);

        }
    }

    public void OnClickSlot(Slots slot)
    {
        slot.item = null;
        slot.gameObject.SetActive(false);
    }

    //public void OnMouseDownItem(Item item)
    //{
    //    if(currentItem == null)
    //    {
    //        currentItem = item;
    //        //the cursor will carry the item clicked
    //        customCursor.gameObject.SetActive(true);
    //        customCursor.sprite = currentItem.GetComponent<Image>().sprite;
    //    }
    //}


    public void AddItem(Item_Reagan item)
    {
        for (int i = 0; i < InventorySlots.Length;)
        {
            //if slot is empty
            if (InventorySlots[i].item == null)
            {
                InventorySlots[i].gameObject.SetActive(true);
                InventorySlots[i].GetComponent<Image>().sprite = item.GetComponent<Image>().sprite;
                InventorySlots[i].item = item;
                //itemList[i] = item;
                break;
            }
            else
            {
                i++;
            }
        }
    }
}
