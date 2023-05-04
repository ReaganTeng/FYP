using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientBarrel : MonoBehaviour
{
    [SerializeField] GameObject ingredient;
    [SerializeField] Image ingredientDisplay;
    [SerializeField] GameObject emptyPrefab;

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
            GameObject PlayerInv = GameObject.FindGameObjectWithTag("Inventory");
            GameObject resultFood = Instantiate(emptyPrefab, PlayerInv.transform);

            resultFood.GetComponent<Food>().SetValues(ingredient.GetComponent<Food>());
            resultFood.GetComponent<Food>().SetPerfect(true);
            Destroy(resultFood.GetComponent<Dish>());
            Destroy(resultFood.GetComponent<RefinedItem>());
            resultFood.AddComponent<Item>();
            resultFood.GetComponent<Item>().SetValueManually(ItemManager.Items.FLOUR, ingredientDisplay.sprite);
            inv.AddItem(resultFood);
        }
    }
}
