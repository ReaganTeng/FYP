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
    int CurrentlySelectedIndex = -1;

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
        TotalOrder = gameObjectList.Count;
        bool NoOrders = false;

        if (CheckIfAnySelected())
        {
            NoOrders = ChangeSelected(true);
        }
        if (!NoOrders)
        {
            SetDisplayInteractable();
        }
    }

    void SetDisplayInteractable(bool voided = false)
    {
        for (int i = 0; i < orderBGDisplay.Count; i++)
        {
            Color color = orderBGDisplay[i].GetComponent<Image>().color;

            // if there is an order there, set alpha to 1, if no order, set alpha to half
            if (orderBGDisplay[i].GetComponentInChildren<OrderPanel>() != null && !voided)
            {
                color.a = 1;
            }
            else
            {
                color.a = 0.5f;
            }

            orderBGDisplay[i].GetComponent<Image>().color = color;
        }
    }

    public bool ChangeSelected(bool ChangeFromOrderFulfill = false)
    {
        // if there is no panel to select, make it not selected
        if (TotalOrder == 0)
        {
            for (int i = 0; i < orderBGDisplay.Count; i++)
            {
                orderBGDisplay[i].GetComponent<Image>().sprite = NotSelectedPanel;
            }
            SetDisplayInteractable(true);
            RecipeDisplay.SetActive(false);
            return true;
        }
        else
        {
            if (!ChangeFromOrderFulfill)
                CurrentlySelectedIndex++;

            // if it went out of bound, set it back to 0
            if (CurrentlySelectedIndex > orderBGDisplay.Count)
            {
                CurrentlySelectedIndex = 0;
            }

            // if it is at the rightmost order, and that order no longer exist, push it back
            if (CurrentlySelectedIndex >= TotalOrder)
            {
                // if it is not a switch panel input, reset it to 0 instead
                if (!ChangeFromOrderFulfill)
                    CurrentlySelectedIndex = 0;

                else
                    CurrentlySelectedIndex--;
            }

            for (int i = 0; i < orderBGDisplay.Count; i++)
            {
                if (i == CurrentlySelectedIndex)
                {
                    orderBGDisplay[i].GetComponent<Image>().sprite = SelectedPanel;
                    AssignImage(orderBGDisplay[i].GetComponentInChildren<OrderPanel>());
                    RecipeDisplay.SetActive(true);
                }
                else
                {
                    orderBGDisplay[i].GetComponent<Image>().sprite = NotSelectedPanel;
                }
            }
        }

        return false;
    }

    private void Start()
    {
        SetDisplayInteractable();
        RecipeDisplay.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeSelected();
        }
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

    public bool CheckIfAnySelected()
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