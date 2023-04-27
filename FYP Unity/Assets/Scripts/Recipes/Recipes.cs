using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipes : MonoBehaviour
{
    public static Recipes instance;
    [System.Serializable]
    public class recipes
    {
        public GameObject ingredient1;
        public GameObject ingredient2;
        public GameObject Result;
    }

    // The list that stores all the possible recipes in the game
    [SerializeField] GameObject Mushy;
    [SerializeField] GameObject Sus_Dish;
    [SerializeField] List<recipes> RefinedIngredientrecipeList;
    [SerializeField] List<recipes> dishrecipeList;
    [SerializeField] GameObject emptyPrefab;

    public recipes GetDishRecipe(string dishname)
    {
        for (int i = 0; i < dishrecipeList.Count; i++)
        {
            if (dishname == dishrecipeList[i].Result.GetComponent<Dish>().GetDishName())
            {
                return dishrecipeList[i];
            }
        }
        return null;
    }

    public recipes GetDishRecipe(int index)
    {
        if (index < dishrecipeList.Count)
            return dishrecipeList[index];

        // return mush if number inputted is invalid
        return null;
    }

    public recipes GetRefinedRecipe(GameObject refinedInput)
    {
        for (int i = 0; i < RefinedIngredientrecipeList.Count; i++)
        {
            if (refinedInput == RefinedIngredientrecipeList[i].Result)
            {
                return RefinedIngredientrecipeList[i];
            }
        }

        // return null if it finds nothing
        return null;
    }

    public int GetDishRecipeListCount()
    {
        return dishrecipeList.Count;
    }

    private void Awake()
    {
        instance = this;
    }

    public GameObject GetRecipeResult(GameObject ingredient1, GameObject ingredient2)
    {
        List<recipes> rp = CheckFoodType(ingredient1);
        int ingredient1id = FoodManager.instance.GetItemID(ingredient1);
        int ingredient2id = FoodManager.instance.GetItemID(ingredient2);

        // run thru the list of recipes to find the dish that the 2 ingredients make
        for (int i = 0; i < rp.Count; i++)
        {
            // find the first ingredient that matches any 2 ingredient requires for the dish
            if (ingredient1id == FoodManager.instance.GetItemID(rp[i].ingredient1) || ingredient1id == FoodManager.instance.GetItemID(rp[i].ingredient2))
            {
                // if it is one of the ingredient, sum the id tgt and see if the sum number match, if it match, its that recipe
                if (ingredient1id + ingredient2id == FoodManager.instance.GetItemID(rp[i].ingredient1) + FoodManager.instance.GetItemID(rp[i].ingredient2))
                {
                    GameObject PlayerInv = GameObject.FindGameObjectWithTag("Inventory");
                    GameObject resultFood = Instantiate(emptyPrefab, PlayerInv.transform);

                    resultFood.GetComponent<Food>().SetValues(rp[i].Result.GetComponent<Food>());

                    switch (rp[i].Result.GetComponent<Food>().GetFoodType())
                    {
                        case FoodManager.FoodType.REFINED_INGREDIENT:
                            resultFood.GetComponent<RefinedItem>().SetValues(rp[i].Result.GetComponent<RefinedItem>());
                            Destroy(resultFood.GetComponent<Dish>());
                            break;
                        case FoodManager.FoodType.DISH:
                            resultFood.GetComponent<Dish>().SetValues(rp[i].Result.GetComponent<Dish>());
                            Destroy(resultFood.GetComponent<RefinedItem>());
                            break;
                    }

                    SetQuality(resultFood, ingredient1, ingredient2);
                    return resultFood;
                }
            }
        }

        // if no valid recipe is found, return mushy or sus dish
        if (ingredient1.GetComponent<Food>().GetFoodType() == FoodManager.FoodType.INGREDIENT)
        {
            return Mushy;
        }
        else
        {
            return Sus_Dish;
        }
    }

    List<recipes> CheckFoodType(GameObject tocheck)
    {
        FoodManager.FoodType ft = tocheck.GetComponent<Food>().GetFoodType();
        int foodidtype = FoodManager.instance.GetFoodID(ft);

        switch (foodidtype)
        {
            // if food type is a raw ingredient, use the RIlist
            case 0:
                return RefinedIngredientrecipeList;

            // if food type is a refined ingredient, use the DishList
            case 1:
                return dishrecipeList;
        }
        return null;
    }

    // function to set the quality or the rating of the ingredient/dishes
    GameObject SetQuality(GameObject toEdit, GameObject ingre1, GameObject ingre2)
    {
        FoodManager.FoodType foodtype = toEdit.GetComponent<Food>().GetFoodType();

        // depending on the foodtype, it will set certain values accordingly
        switch (foodtype)
        {
            case FoodManager.FoodType.REFINED_INGREDIENT:
                // if it is Perfect for both raw ingredients, the refined ingredient is perfect
                if (ingre1.GetComponent<Food>().GetIsPerfect() && ingre2.GetComponent<Food>().GetIsPerfect())
                {
                    toEdit.GetComponent<Food>().SetPerfect(true);
                }
                else
                {
                    toEdit.GetComponent<Food>().SetPerfect(false);
                }

                break;
            case FoodManager.FoodType.DISH:
                // if any of the ingredient is perfect, add a star to them.
                toEdit.GetComponent<Food>().SetAmountOfStars(0);

                if (ingre1.GetComponent<Food>().GetIsPerfect())
                    toEdit.GetComponent<Food>().SetAmountOfStars(toEdit.GetComponent<Food>().GetAmtOfStars() + 1);

                if (ingre2.GetComponent<Food>().GetIsPerfect())
                    toEdit.GetComponent<Food>().SetAmountOfStars(toEdit.GetComponent<Food>().GetAmtOfStars() + 1);

                break;
        }

        return toEdit;
    }
}