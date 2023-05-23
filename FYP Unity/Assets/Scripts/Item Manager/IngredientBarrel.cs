using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientBarrel : MonoBehaviour
{
    [SerializeField] GameObject ingredient;
    [SerializeField] Image ingredientDisplay;
    [SerializeField] GameObject emptyPrefab;
    [SerializeField] IngredientBarrelManager.BarrelTypes barrelType;
    private bool barrelActive = true;
    

    private void Start()
    {
        ingredientDisplay.sprite = FoodManager.instance.GetImage(ingredient);
    }

    public void GetIngredientFromBarrel()
    {
        if (!barrelActive)
            return;

        InventoryImageControl inv = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InventoryImageControl>();

        // if inventory is not full, add the ingredient to the inventory
        if (!Inventory.instance.InventoryFull)
        {
            GameObject PlayerInv = GameObject.FindGameObjectWithTag("Inventory");
            GameObject resultFood = Instantiate(emptyPrefab, PlayerInv.transform);

            // create a copy of that item as an object reference
            resultFood.GetComponent<Food>().SetValues(ingredient.GetComponent<Food>());
            resultFood.GetComponent<Food>().SetPerfect(true);
            Destroy(resultFood.GetComponent<Dish>());
            Destroy(resultFood.GetComponent<RefinedItem>());
            resultFood.AddComponent<Item>();
            resultFood.GetComponent<Item>().SetValueManually(GetItemType());
            inv.AddItem(resultFood);
        }
    }

    public void SetIsActive(bool active)
    {
        barrelActive = active;
    }

    public IngredientBarrelManager.BarrelTypes GetBarrelType()
    {
        return barrelType;
    }

    private ItemManager.Items GetItemType()
    {
        switch (barrelType)
        {
            case IngredientBarrelManager.BarrelTypes.FLOUR:
                return ItemManager.Items.FLOUR;
            case IngredientBarrelManager.BarrelTypes.SEAWEED:
                return ItemManager.Items.SEAWEED;
            case IngredientBarrelManager.BarrelTypes.SUGAR:
                return ItemManager.Items.SUGAR;
            case IngredientBarrelManager.BarrelTypes.SALT:
                return ItemManager.Items.SALT;
            default:
                return ItemManager.Items.FLOUR;
        }
    }
}
