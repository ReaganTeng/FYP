using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryImageControl : MonoBehaviour
{
    [SerializeField] Sprite NotSelectedHotbar;
    [SerializeField] Sprite SelectedHotbar;
    [SerializeField] Sprite LockedHotbar;
    [SerializeField] GameObject inventoryEmpty;
    [SerializeField] Sprite SliverStar;
    [SerializeField] Sprite GoldStar;
    [SerializeField] Sprite IridiumStar;
    [SerializeField] GameObject tempfood;
    [SerializeField] bool IsPerfect;
    [SerializeField] int amtofStars;

    // Stores the Currently selected inventory slot
    int SelectedSlot;

    // stores the reference of inventory slots image
    List<GameObject> inventorySlots = new List<GameObject>();

    private void Start()
    {
        SelectedSlot = -1;
        Transform[] inventory = inventoryEmpty.GetComponentsInChildren<Transform>();

        foreach(var gm in inventory)
        {
            gm.gameObject.SetActive(false);
            inventorySlots.Add(gm.gameObject);
        }

        //for (int i = 0; i < 4; i++)
        //{
        //    GameObject pt = tempfood;
        //    pt.GetComponent<Food>().SetPerfect(IsPerfect);
        //    pt.GetComponent<Food>().SetAmountOfStars(amtofStars);
        //    AddItem(pt);
        //}

        // Set active
        inventorySlots[0].SetActive(true);

        SetInventoryDisplay();
    }

    // Add ingredient into inventory
    public void AddItem(GameObject item)
    {
        Inventory.instance.AddToInventory(item);
        UpdateImage();
    }

    void AddImage(GameObject image, GameObject item)
    {
        Sprite theimage = FoodManager.instance.GetImage(item);
        image.GetComponent<Image>().sprite = theimage;
    }

    // overloaded function for star image
    void AddImage(GameObject image, int stars)
    {
        switch (stars)
        {
            case 1:
            case 2:
                image.GetComponent<Image>().sprite = SliverStar;
                break;
            case 3:
            case 4:
                image.GetComponent<Image>().sprite = GoldStar;
                break;
            case 5:
                image.GetComponent<Image>().sprite = IridiumStar;
                break;
            default:
                image.GetComponent<Image>().sprite = SliverStar;
                break;
        }
    }

    // 0 is not selected, 1 is selected, 2 is locked
    void AddHotbarImage(GameObject image, int type)
    {
        switch (type)
        {
            case 1:
                image.GetComponent<Image>().sprite = NotSelectedHotbar;
                break;
            case 2:
                image.GetComponent<Image>().sprite = SelectedHotbar;
                break;
            case 3:
                image.GetComponent<Image>().sprite = LockedHotbar;
                break;
            default:
                image.GetComponent<Image>().sprite = LockedHotbar;
                break;
        }
    }

    // modify the inventory ui to lock or unlock slots
    public void SetInventoryDisplay()
    {
        int index = 0;
        for (int i = 1; i < inventorySlots.Count; i += 9)
        {
            if (index < Inventory.instance.GetMaxInventorySize())
            {
                index++;
                AddHotbarImage(inventorySlots[i], 1);
            }
            else
            {
                AddHotbarImage(inventorySlots[i], 0);
            }

            inventorySlots[i].SetActive(true);
        }
    }

    // 1 = slots bg, 2 = Display for food, 3 = quality of ingredients, 4 = parent of star quality for dish, 5 to 9 = the stars;
    void UpdateImage()
    {
        ResetImage();
        for (int i = 0; i < Inventory.instance.GetList().Count; i++)
        {
            // Set the dish to render and assign the image
            inventorySlots[2 + (i * 9)].SetActive(true);
            AddImage(inventorySlots[2 + (i * 9)], Inventory.instance.GetList()[i].food);

            // Assign quality if its ingredients
            if (!Inventory.instance.GetList()[i].IsDish)
            {
                if (Inventory.instance.GetList()[i].IsPerfect)
                    inventorySlots[3 + (i * 9)].SetActive(true);
            }
            // Assign amount of stars if it is a dish
            else
            {
                // Set the parent to true
                inventorySlots[4 + (i * 9)].SetActive(true);
                for (int x = 0; x < 5; x++)
                {
                    if (x < Inventory.instance.GetList()[i].Stars)
                    {
                        inventorySlots[5 + x + (i * 9)].SetActive(true);
                        AddImage(inventorySlots[5 + x + (i * 9)], Inventory.instance.GetList()[i].Stars);
                    }
                    else
                        inventorySlots[5 + x + (i * 9)].SetActive(false);
                }
            }

            if (Inventory.instance.GetList().Count == 1)
                ChangeSelectedHotBar(0);
        }
    }

    void ResetImage()
    {
        for (int i = 1; i < inventorySlots.Count; i++)
        {
            if ((i - 1) % 9 == 0)
                inventorySlots[i].SetActive(true);
            else
                inventorySlots[i].SetActive(false);
        }
    }

    public void ChangeSelectedHotBar(int pos)
    {
        // check if its valid, if it is not, do not switch
        if (pos + 1 <= Inventory.instance.GetList().Count)
        {
            SelectedSlot = pos;
            for (int i = 0; i < Inventory.instance.GetMaxInventorySize(); i++)
            {
                if (i == pos)
                    AddHotbarImage(inventorySlots[1 + (9 * i)], 2);
                else
                    AddHotbarImage(inventorySlots[1 + (9 * i)], 1);
            }
        }
    }

    public void RemoveSelected(bool DeleteReference = false)
    {
        // check to see if the remove is valid
        if (SelectedSlot <= Inventory.instance.GetList().Count && SelectedSlot != -1)
        {
            Inventory.instance.RemoveFromInventory(SelectedSlot, DeleteReference);
            UpdateImage();
            // if the item is deleted as its last slot, move it, if cannot move, none are selected
            if (SelectedSlot >= Inventory.instance.GetList().Count)
            {
                SelectedSlot -= 1;
                ChangeSelectedHotBar(SelectedSlot);
            }
        }
    }

    public int GetSelectedFoodID(FoodManager.FoodType ft)
    {
        if (Inventory.instance.GetList().Count > 0)
        {
            // check to see if the current sleected ingredient matches the parameter, if it does not match, return -1
            if (Inventory.instance.GetList()[SelectedSlot].food.GetComponent<Food>().GetFoodType() == ft)
            {
                return FoodManager.instance.GetItemID(Inventory.instance.GetList()[SelectedSlot].food);
            }
        }

        return -1;
    }

    public GameObject GetSelectedGameObject()
    {
        if (Inventory.instance.GetList().Count > 0)
        {
            return Inventory.instance.GetList()[SelectedSlot].food;
        }
        return null;
    }

    public int GetSelectedStarAmount()
    {
        if (Inventory.instance.GetList()[SelectedSlot].IsDish)
        {
            return Inventory.instance.GetList()[SelectedSlot].Stars;
        }

        return 0;
    }
}
