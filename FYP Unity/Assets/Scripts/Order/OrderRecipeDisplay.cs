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
    [SerializeField] Image DishBG;
    [SerializeField] Image RefinedIngredientBG1;
    [SerializeField] Image RefinedIngredientBG2;
    [SerializeField] Image Ingredient1BG;
    [SerializeField] Image Ingredient2BG;
    [SerializeField] Image Ingredient3BG;
    [SerializeField] Image Ingredient4BG;
    private Image DishResult;
    private Image RefinedIngredientResult1;
    private Image RefinedIngredientResult2;
    private Image Ingredient1;
    private Image Ingredient2;
    private Image Ingredient3;
    private Image Ingredient4;
    List<Image> displayList = new List<Image>();
    [SerializeField] Color rolling_pin;
    [SerializeField] Color knife;
    [SerializeField] Color spatula;
    [SerializeField] Color crate;
    List<Image> IngredientList = new List<Image>();

    Color defaultColor = new Color(255, 255, 255, 255);
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
            SetOrderDisplayActive();
        }
    }

    void SetOrderDisplayActive(bool voided = false)
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
            SetOrderDisplayActive(true);
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
        SetOrderDisplayActive();
        RecipeDisplay.SetActive(false);
        DishResult = DishBG.gameObject.GetComponentsInChildren<Image>()[1];
        RefinedIngredientResult1 = RefinedIngredientBG1.gameObject.GetComponentsInChildren<Image>()[1];
        RefinedIngredientResult2 = RefinedIngredientBG2.gameObject.GetComponentsInChildren<Image>()[1];
        Ingredient1 = Ingredient1BG.gameObject.GetComponentsInChildren<Image>()[1];
        Ingredient2 = Ingredient2BG.gameObject.GetComponentsInChildren<Image>()[1];
        Ingredient3 = Ingredient3BG.gameObject.GetComponentsInChildren<Image>()[1];
        Ingredient4 = Ingredient4BG.gameObject.GetComponentsInChildren<Image>()[1];

        displayList.Add(DishBG.gameObject.GetComponentsInChildren<Image>()[2]);
        displayList.Add(RefinedIngredientBG1.gameObject.GetComponentsInChildren<Image>()[2]);
        displayList.Add(RefinedIngredientBG2.gameObject.GetComponentsInChildren<Image>()[2]);
        displayList.Add(Ingredient1BG.gameObject.GetComponentsInChildren<Image>()[2]);
        displayList.Add(Ingredient2BG.gameObject.GetComponentsInChildren<Image>()[2]);
        displayList.Add(Ingredient3BG.gameObject.GetComponentsInChildren<Image>()[2]);
        displayList.Add(Ingredient4BG.gameObject.GetComponentsInChildren<Image>()[2]);

        IngredientList.Add(Ingredient1);
        IngredientList.Add(Ingredient2);
        IngredientList.Add(Ingredient3);
        IngredientList.Add(Ingredient4);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeSelected();
            SetTick(FoodManager.FoodType.DISH, false);
        }
        CheckWhatIsFulfilled();
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
                ChangeToWeaponType(Ingredient1BG, ingre1);
                Ingredient2.sprite = FoodManager.instance.GetImage(ingre2);
                ChangeToWeaponType(Ingredient2BG, ingre2);
            }
            else
            {
                Ingredient3.sprite = FoodManager.instance.GetImage(ingre1);
                ChangeToWeaponType(Ingredient3BG, ingre1);
                Ingredient4.sprite = FoodManager.instance.GetImage(ingre2);
                ChangeToWeaponType(Ingredient4BG, ingre2);
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

    public void ChangeToWeaponType(Image theImage, GameObject theReference)
    {
        switch (theReference.GetComponent<Item>().GetItemVariant())
        {
            case ItemManager.ItemVariant.CHOPPED:
                {
                    theImage.color = knife;
                    break;
                }
            case ItemManager.ItemVariant.MASHED:
                {
                    theImage.color = rolling_pin;
                    break;
                }
            case ItemManager.ItemVariant.PREP:
                {
                    theImage.color = spatula;
                    break;
                }
            case ItemManager.ItemVariant.DRAWER:
                {
                    theImage.color = crate;
                    break;
                }
        }
    }

    void CheckWhatIsFulfilled()
    {
        SetTick(FoodManager.FoodType.DISH, false);
        bool HasDish = false;
        List<int> RefinedIndex = new List<int>();
        List<int> NormalIndex = new List<int>();
        for (int i = 0; i < Inventory.instance.GetList().Count; i++)
        {
            GameObject foodobj = Inventory.instance.GetList()[i].food;

            // if it contain a dish
            if (foodobj.GetComponent<Food>().GetFoodType() == FoodManager.FoodType.DISH && foodobj.GetComponent<Dish>().GetImage() == DishResult.sprite)
            {
                SetTick(FoodManager.FoodType.DISH, true);
                HasDish = true;
                break;
            }

            // if it contain a refined ingredieny
            if (foodobj.GetComponent<Food>().GetFoodType() == FoodManager.FoodType.REFINED_INGREDIENT)
            {
                RefinedIndex.Add(i);
            }

            // if it contain a normal ingredient
            else if (foodobj.GetComponent<Food>().GetFoodType() == FoodManager.FoodType.INGREDIENT)
            {
                NormalIndex.Add(i);
            }
        }

        if (HasDish)
            return;

        bool case1 = false;
        bool case2 = false;
        bool NoRepeat = false;
        // after the loop, check for the refined first
        for (int i = 0; i < RefinedIndex.Count; i++)
        {
            if (FoodManager.instance.GetImage(Inventory.instance.GetList()[RefinedIndex[i]].food) == RefinedIngredientResult1.sprite && !NoRepeat)
            {
                SetTick(FoodManager.FoodType.REFINED_INGREDIENT, true, 1);
                case1 = true;
                NoRepeat = true;
            }

            else if (FoodManager.instance.GetImage(Inventory.instance.GetList()[RefinedIndex[i]].food) == RefinedIngredientResult2.sprite)
            {
                SetTick(FoodManager.FoodType.REFINED_INGREDIENT, true, 2);
                case2 = true;
            }
        }

        // if both refined are found, no reason to check ingredient
        if (case1 && case2)
                return;

        List<bool> IngredientDupe = new List<bool>();
        for (int b = 0; b < 4; b++)
        {
            IngredientDupe.Add(false);
        }
        // check for ingredient
        for (int i = 0; i < NormalIndex.Count; i++)
        {
            for (int index = 0; index < IngredientList.Count; index++)
            {
                if (FoodManager.instance.GetImage(Inventory.instance.GetList()[NormalIndex[i]].food) == IngredientList[index].sprite && !IngredientDupe[index])
                {
                    SetTick(FoodManager.FoodType.INGREDIENT, true, 0, index + 1);
                    IngredientDupe[index] = true;
                    break;
                }
            }
        }
    }

    void SetTick(FoodManager.FoodType whatType, bool TickOrNot, int riCount = 0, int iCount = 0)
    {
        // if it is a dish, set all to ticked
        if (whatType == FoodManager.FoodType.DISH)
        {
            for (int i = 0; i < displayList.Count; i++)
            {
                displayList[i].gameObject.SetActive(TickOrNot);
            }
        }
        // 0 is dish, 1-2 is refined, 3-6 is ingredient
        
        // if it is a refined ingredient, check to see how many needed to be ticked
        else if (whatType == FoodManager.FoodType.REFINED_INGREDIENT)
        {
            if (riCount == 2)
            {
                displayList[2].gameObject.SetActive(TickOrNot);
                displayList[5].gameObject.SetActive(TickOrNot);
                displayList[6].gameObject.SetActive(TickOrNot);
            }
            if (riCount == 1)
            {
                displayList[1].gameObject.SetActive(TickOrNot);
                displayList[3].gameObject.SetActive(TickOrNot);
                displayList[4].gameObject.SetActive(TickOrNot);
            }
        }

        // if it is a ingredient, check to see how many needed to be ticked

        else if (whatType == FoodManager.FoodType.INGREDIENT)
        {
            displayList[iCount + 2].gameObject.SetActive(TickOrNot);
        }
    }
}