using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipes : MonoBehaviour
{
    public static Recipes instance;
    [System.Serializable]
    public class recipes
    {
        public Item ingredient1;
        public Item ingredient2;
        public Dish Result;
    }

    // The list that stores all the possible recipes in the game
    [SerializeField] List<recipes> recipeList;

    public recipes GetRecipe(string dishname)
    {
        for (int i = 0; i < recipeList.Count; i++)
        {
            if (dishname == recipeList[i].Result.GetDishName())
            {
                return recipeList[i];
            }
        }
        return null;
    }

    public recipes GetRecipe(int index)
    {
        if (index < recipeList.Count)
            return recipeList[index];

        // return mush if number inputted is invalid
        return null;
    }

    public int GetRecipeListCount()
    {
        return recipeList.Count;
    }

    private void Awake()
    {
        instance = this;
    }

    public int GetRecipeResult(int ingredient1ID, int ingredient2ID)
    {
        // run thru the list of recipes to find the dish that the 2 ingredients make
        for (int i = 0; i < recipeList.Count; i++)
        {
            // find the first ingredient that matches any 2 ingredient requires for the dish
            if (ingredient1ID == recipeList[i].ingredient1.GetItemID() || ingredient2ID == recipeList[i].ingredient2.GetItemID())
            {
                // if it is one of the ingredient, sum the id tgt and see if the sum number match, if it match, its that recipe
                if (ingredient1ID + ingredient2ID == recipeList[i].ingredient1.GetItemID() + recipeList[i].ingredient2.GetItemID())
                {
                    return i;
                }
            }
        }

        // if no valid recipe is found, return -1 which is sus dish
        return -1;
    }
}