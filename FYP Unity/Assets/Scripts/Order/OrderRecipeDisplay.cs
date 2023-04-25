using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderRecipeDisplay : MonoBehaviour
{
    [SerializeField] List<GameObject> orderBGDisplay;
    [SerializeField] Sprite NotSelectedPanel;
    [SerializeField] Sprite SelectedPanel;
    [SerializeField] GameObject RecipeDisplay;
    [SerializeField] Image DishResult;
    [SerializeField] Image RefinedIngredientResult1;
    [SerializeField] Image RefinedIngredientResult2;
    [SerializeField] Image Ingredient1;
    [SerializeField] Image Ingredient2;
    [SerializeField] Image Ingredient3;
    [SerializeField] Image Ingredient4;
    private int TotalOrder;

    public void SetToDisplay(List<GameObject> gameObjectList)
    {
        for (int i = 0; i < gameObjectList.Count; i++)
        {
            if (i >= orderBGDisplay.Count)
                break;

            gameObjectList[i].transform.SetParent(orderBGDisplay[i].transform);
            gameObjectList[i].transform.localScale = orderBGDisplay[i].transform.localScale;
            gameObjectList[i].transform.localPosition = Vector3.zero;
        }
        // Check to see if any are selected, if yes, do the checking
        if (CheckIfAnySelected())
        {
            ChangeSelectedAfterOrder();
        }
        TotalOrder = gameObjectList.Count;
        SetDisplayInteractable();
    }

    void SetDisplayInteractable()
    {
        for (int i = 0; i < orderBGDisplay.Count; i++)
        {
            if (orderBGDisplay[i].GetComponentInChildren<OrderPanel>() != null)
            {
                orderBGDisplay[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                orderBGDisplay[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    public void ChangeSelected(GameObject panelSelected)
    {
        // if it is null, all are not selected
        if (panelSelected == null)
        {
            for (int i = 0; i < orderBGDisplay.Count; i++)
            {
                orderBGDisplay[i].GetComponent<Image>().sprite = NotSelectedPanel;
            }
            RecipeDisplay.SetActive(false);
        }

        // if it is not null, find what call it and make it selected
        else
        {
            for (int i = 0; i < orderBGDisplay.Count; i++)
            {
                // if the gameobject is the one being clicked on, change it to selected
                if (orderBGDisplay[i] == panelSelected)
                {
                    orderBGDisplay[i].GetComponent<Image>().sprite = SelectedPanel;
                    AssignImage(orderBGDisplay[i].GetComponentInChildren<OrderPanel>());
                }
                else
                    orderBGDisplay[i].GetComponent<Image>().sprite = NotSelectedPanel;
            }
            RecipeDisplay.SetActive(true);
        }
    }

    void ChangeSelectedAfterOrder()
    {
        for (int i = 0; i < orderBGDisplay.Count; i++)
        {
            // Find the selected order panel
            if (orderBGDisplay[i].GetComponent<Image>().sprite == SelectedPanel)
            {
                // Check to see if it contains an order, if it does, reallocate the selected
                if (orderBGDisplay[i].GetComponentInChildren<OrderPanel>() != null)
                {
                    ChangeSelected(orderBGDisplay[i]);
                }
                // if it does not contain an order, push the selected to the previous order panel
                else
                {
                    // if the index is not a, not the first panel, change to the previous panel
                    if (i != 0)
                    {
                        ChangeSelected(orderBGDisplay[i - 1]);
                    }
                    // if the index is at 0, the first panel, disable the display
                    else
                    {
                        ChangeSelected(null);
                    }
                }
                break;
            }
        }
    }

    private void Start()
    {
        SetDisplayInteractable();
        RecipeDisplay.SetActive(false);
    }

    void AssignImage(OrderPanel dishInput)
    {
        // Assign the dish image to the display
        DishResult.sprite = FoodManager.instance.GetImage(dishInput.GetOrderRecipe().Result);
        // get the gameobject of refined ingredient 1 and 2
        GameObject refined1 = dishInput.GetOrderRecipe().ingredient1;
        GameObject refined2 = dishInput.GetOrderRecipe().ingredient2;
        // assign the refined ingredient image to the display
        RefinedIngredientResult1.sprite = FoodManager.instance.GetImage(refined1);
        RefinedIngredientResult2.sprite = FoodManager.instance.GetImage(refined2);
        GameObject[] tempArray = { refined1, refined2 };

        // repeat the assigning twice
        for (int i = 0; i < tempArray.Length; i++)
        {
            // get the gameobject of the ingredient 1 and 2
            Recipes.recipes refinedIngredientRecipe = Recipes.instance.GetRefinedRecipe(tempArray[i]);
            GameObject ingre1 = refinedIngredientRecipe.ingredient1;
            GameObject ingre2 = refinedIngredientRecipe.ingredient2;
            // assign the ingredient image to the display
            if (i == 0)
            {
                Ingredient1.sprite = FoodManager.instance.GetImage(ingre1);
                Ingredient2.sprite = FoodManager.instance.GetImage(ingre2);
            }
            else
            {
                Ingredient3.sprite = FoodManager.instance.GetImage(ingre1);
                Ingredient4.sprite = FoodManager.instance.GetImage(ingre2);
            }
        }
    }

    bool CheckIfAnySelected()
    {
        for (int i = 0; i < orderBGDisplay.Count; i++)
        {
            if (orderBGDisplay[i].GetComponent<Image>().sprite == SelectedPanel)
            {
                return true;
            }
        }
        return false;
    }
}