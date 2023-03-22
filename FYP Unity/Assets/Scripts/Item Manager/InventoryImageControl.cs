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

    List<item> inventorySlots = new List<item>();
    Inventory inv;

    public class item
    {
        public GameObject inventorySlot;
        public GameObject ingredientSlot;
        // if the item is currently being selected
        public bool Selected;
        // id for the ingredients
        public int itemid;
        // a bool to check to see if its an ingredient or a dish
        public bool IsADish;
        // id for the dishes
        public int dishid;
        // to store the item reference itself to be used for mixer
        public Item theItem;
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
            tempItem.IsADish = false;
            tempItem.dishid = -1;
            tempItem.theItem = null;
            inventorySlots.Add(tempItem);

        }
        ModifyItemSlots(inv.GetMaxInventorySize());
    }

    // Add ingredient into inventory
    public void AddItem(Item item)
    {
        UpdateImage(item.gameObject, false);
    }

    // Overloaded function, add dish into inventory
    public void AddItem(Dish dish)
    {
        UpdateImage(dish.gameObject, true);
    }

    void AddImage(GameObject image, GameObject ItemID, bool UseDishImage)
    {
        if (UseDishImage)
        {
            image.GetComponent<Image>().sprite = ItemID.GetComponent<Dish>().GetImage();
        }

        else
        {
            image.GetComponent<Image>().sprite = ItemID.GetComponent<Item>().GetImage();
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

    void UpdateImage(GameObject item, bool IsADish)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            // if it is not active, add it into the inventory
            if (!inventorySlots[i].ingredientSlot.activeSelf)
            {
                inventorySlots[i].ingredientSlot.SetActive(true);
                // If it is a dish, add as a dish instead
                if (IsADish)
                {
                    inventorySlots[i].dishid = item.GetComponent<Dish>().GetItemID();
                    inventorySlots[i].IsADish = true;
                }
                else
                {
                    inventorySlots[i].itemid = item.GetComponent<Item>().GetItemID();
                    inventorySlots[i].IsADish = false;
                    inventorySlots[i].theItem = item.GetComponent<Item>();
                }

                AddImage(inventorySlots[i].ingredientSlot, item, IsADish);

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

            // Ignore this if the selected is negative
            if (pos != -1)
            {
                inventorySlots[pos].inventorySlot.GetComponent<Image>().sprite = SelectedHotBar;
                inventorySlots[pos].Selected = true;
                if (!inventorySlots[pos].IsADish)
                    Debug.Log("ItemID: " + inventorySlots[pos].itemid);
                else
                    Debug.Log("DishID: " + inventorySlots[pos].dishid);
            }
        }
    }

    public void RemoveSelected()
    {
        for (int i = 0; i < inv.GetMaxInventorySize(); i++)
        {
            // change previous selected back to not selected sprite
            if (inventorySlots[i].Selected)
            {
                inv.RemoveFromInventory(i);
                SortList(i);
                break;
            }
        }
    }

    public int GetSelectedInventory(bool IsItADish)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            // get the selected one
            if (inventorySlots[i].Selected)
            {
                // if it is a dish, return the dish id
                if (IsItADish)
                {
                    return inventorySlots[i].dishid;
                }
                // if it is not a dish, return ingredient id instead
                else
                {
                    return inventorySlots[i].itemid;
                }
            }
        }

        return -1;
    }

    public Item GetItem()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            // get the selected one
            if (inventorySlots[i].Selected)
            {
                return inventorySlots[i].theItem;
            }
        }

        return null;
    }

    public void SortList(int pos)
    {
        for (int i = pos; i < inv.GetMaxInventorySize(); i++)
        {
            if (i + 1 < GetUsedInventorySlot())
            {
                inventorySlots[i].ingredientSlot.GetComponent<Image>().sprite = inventorySlots[i + 1].ingredientSlot.GetComponent<Image>().sprite;
                inventorySlots[i].itemid = inventorySlots[i + 1].itemid;
                inventorySlots[i].dishid = inventorySlots[i + 1].dishid;
                inventorySlots[i].IsADish = inventorySlots[i + 1].IsADish;
                inventorySlots[i].theItem = inventorySlots[i + 1].theItem;
            }
            else
            {
                // if the selected is the last slot, change the cursor
                if (pos + 1 == GetUsedInventorySlot())
                {
                    ChangeSelectedHotBar(i - 1);
                }

                inventorySlots[i].ingredientSlot.SetActive(false);
                inventorySlots[i].itemid = -1;
                inventorySlots[i].dishid = -1;
                inventorySlots[i].IsADish = false;
                inventorySlots[i].theItem = null;
            }
        }
    }
}
