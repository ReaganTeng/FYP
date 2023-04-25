using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientBarrel : MonoBehaviour
{
    [SerializeField] GameObject ingredient;
    [SerializeField] Image ingredientDisplay;

    private void Start()
    {
        ingredientDisplay.sprite = FoodManager.instance.GetImage(ingredient);
    }

    public void GetIngredientFromBarrel()
    {
        InventoryImageControl inv = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InventoryImageControl>();

        // if inventory is not full, add the ingredient to the inventory
        if (!Inventory.instance.InventoryFull)
        {
            inv.AddItem(ingredient);
        }
    }
}
