using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryImageControl : MonoBehaviour
{
    [SerializeField] GameObject InventorySlot1;
    [SerializeField] GameObject InventorySlot2;
    [SerializeField] GameObject InventorySlot3;
    [SerializeField] GameObject InventorySlot4;
    [SerializeField] GameObject InventorySlot5;

    [SerializeField] GameObject IngredientSlot1;
    [SerializeField] GameObject IngredientSlot2;
    [SerializeField] GameObject IngredientSlot3;
    [SerializeField] GameObject IngredientSlot4;
    [SerializeField] GameObject IngredientSlot5;

    [SerializeField] Sprite NotSelectedHotBar;
    [SerializeField] Sprite SelectedHotBar;
    [SerializeField] Sprite ChoppedApple;
    [SerializeField] Sprite MashedApple;
    [SerializeField] Sprite PrepApple;

    List<item> inventorySlots = new List<item>();
    Inventory inv;

    public class item
    {
        public GameObject inventorySlot;
        public GameObject ingredientSlot;
        public bool Selected;
        public int itemid;
    }

    private void Start()
    {
        GameObject[] inventoryArray = { InventorySlot1, InventorySlot2, InventorySlot3, InventorySlot4, InventorySlot5 };
        GameObject[] ingredientArray = { IngredientSlot1, IngredientSlot2, IngredientSlot3, IngredientSlot4, IngredientSlot5 };
        inv = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        for (int i = 0; i < inventoryArray.Length; i++)
        {
            item tempItem = new item();
            tempItem.inventorySlot = inventoryArray[i];
            tempItem.inventorySlot.SetActive(false);
            tempItem.ingredientSlot = ingredientArray[i];
            tempItem.ingredientSlot.SetActive(false);
            tempItem.Selected = false;
            tempItem.itemid = -1;
            inventorySlots.Add(tempItem);

        }
        ModifyItemSlots(inv.GetMaxInventorySize());
    }


    public void AddItem(Item item)
    {
        int itemid = item.GetItemID();
        UpdateImage(itemid);
    }

    void AddImage(GameObject image, int ItemID)
    {
        switch ((ItemManager.Items)ItemID)
        {
            case ItemManager.Items.POTATO_CHOPPED:
                image.GetComponent<Image>().sprite = ChoppedApple;
                break;

            case ItemManager.Items.POTATO_MASHED:
                image.GetComponent<Image>().sprite = MashedApple;
                break;

            case ItemManager.Items.POTATO_PREP:
                image.GetComponent<Image>().sprite = PrepApple;
                break;
        }
    }

    public void ModifyItemSlots(int openslots)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (i < openslots)
                inventorySlots[i].inventorySlot.SetActive(true);
            else
                inventorySlots[i].inventorySlot.SetActive(false);
        }
    }

    void UpdateImage(int itemid)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            // if it is not active, add it into the inventory
            if (!inventorySlots[i].ingredientSlot.activeSelf)
            {
                inventorySlots[i].ingredientSlot.SetActive(true);
                inventorySlots[i].itemid = itemid;
                AddImage(inventorySlots[i].ingredientSlot, itemid);

                if (GetUsedInventorySlot() == 1)
                {
                    //inventorySlots[i].inventorySlot;
                    ChangeSelectedHotBar(i);
                    inventorySlots[i].Selected = true;
                }
                break;
            }
        }
    }

    int GetUsedInventorySlot()
    {
        int slotused = 0;

        for (int i = 0; i < inv.GetMaxInventorySize(); i++)
        {
            // if it is active reduce slot by 1
            if (inventorySlots[i].ingredientSlot.activeSelf)
                slotused++;
        }
        return slotused;
    }

    public void ChangeSelectedHotBar(int pos)
    {
        // check if its valid, if it is not, do not switch
        if (pos + 1 <= GetUsedInventorySlot())
        {
            for (int i = 0; i < inv.GetMaxInventorySize(); i++)
            {
                // change previous selected back to not selected sprite
                if (inventorySlots[i].Selected)
                {
                    inventorySlots[i].inventorySlot.GetComponent<Image>().sprite = NotSelectedHotBar;
                    inventorySlots[i].Selected = false;
                }
            }

            inventorySlots[pos].inventorySlot.GetComponent<Image>().sprite = SelectedHotBar;
            inventorySlots[pos].Selected = true;
            Debug.Log(inventorySlots[pos].itemid);
        }
    }

    public void RemoveSelected()
    {
        for (int i = 0; i < inv.GetMaxInventorySize(); i++)
        {
            // change previous selected back to not selected sprite
            if (inventorySlots[i].Selected)
            {
                SortList(i);
                break;
            }
        }
    }

    public void SortList(int pos)
    {
        for (int i = pos; i < inv.GetMaxInventorySize(); i++)
        {
            if (i + 1 < GetUsedInventorySlot())
            {
                inventorySlots[i].ingredientSlot.GetComponent<Image>().sprite = inventorySlots[i + 1].ingredientSlot.GetComponent<Image>().sprite;
                inventorySlots[i].itemid = inventorySlots[i + 1].itemid;
            }
            else
            {
                ChangeSelectedHotBar(i - 1);
                inventorySlots[i].ingredientSlot.SetActive(false);
                inventorySlots[i].itemid = -1;
            }
        }
    }
}
