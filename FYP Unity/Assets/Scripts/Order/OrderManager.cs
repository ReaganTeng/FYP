using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager instance;
    [System.Serializable]
    public class DayOrder
    {
        // contains the list of dishes that will appear that day
        public List<Dish> theDish = new List<Dish>();
        // Time till order expire, default is 100 if set to 0
        public float WaitingTime = 0;
        // Penalty for not serving a dish
        public int penalty = 10;
        // if this is check, the dish set will only appear on that day, if this is uncheck, u can set how many random orders to set
        public bool Fixed = true;
        //Set the amt of dishes for the day, will be ignore if Fixed is true
        public int AmtOfDishes;
    }

    // Get the reference to the level manager which stores the list of levels.
    [SerializeField] LevelManager lm;

    // The preset day list from the level itself
    DayOrder presetday;

    List<Recipes.recipes> currentdayrecipesList = new List<Recipes.recipes>();

    [SerializeField] EndOfDay eod;

    // Stores the current day
    private int currentday;

    private void Awake()
    {
        instance = this;
        currentday = eod.GetCurrentDay();
        // set the presetday to what the level customization is, if the level itself exist
        if (currentday > 0 && currentday < lm.levelInfo.Count)
            presetday = lm.levelInfo[currentday - 1].CustomizeOrderForThisDay;

    }

    private void Start()
    {
        // set the orders that can come out
        SetOrdersForTheDay();
    }

    public int GetCurrentDay()
    {
        return currentday;
    }

    // Increase current day by 1
    public void DayPassed()
    {
        currentdayrecipesList.Clear();
        currentday++;
        SetOrdersForTheDay();
    }

    public void SetOrdersForTheDay()
    {
        bool PresetDay = false;
        // Check to see if there is a level customization set for this day
        if (presetday != null)
        {
            // Check to see if there are dishes that appear in that level
            if (presetday.theDish.Count > 0)
            {
                PresetDay = true;
            }
        }

        // if there is a presetday, use it
        if (PresetDay)
        {
            // Use whatever that is supplied into the parameter
            for (int i = 0; i < presetday.theDish.Count; i++)
            {
                currentdayrecipesList.Add(Recipes.instance.GetDishRecipe(presetday.theDish[i].GetDishName()));
            }

            // If the day is fixed, do not need to care about the rest, if it is not fixed, fill in random recipes if needed
            if (!presetday.Fixed)
            {
                // check to see what extra dishes needs to be fulfill if any
                if (presetday.AmtOfDishes > currentdayrecipesList.Count)
                {
                    int AmtOfRecipesToAdd = presetday.AmtOfDishes - currentdayrecipesList.Count;
                    GenerateRandomRecipe(AmtOfRecipesToAdd);
                }
            }

        }
        // if it is not a presetday, use a random list of recipes
        else
        {
            int AmtOfDishToAppear = Random.Range(1, 3); // Generate between 1 or 2 recipes for the day

            GenerateRandomRecipe(AmtOfDishToAppear);
        }
    }

    void GenerateRandomRecipe(int GenerateHowMany)
    {
        for (int i = 0; i < GenerateHowMany; i++)
        {
            // Get the amount of possible recipes
            int PossibleRecipes = Recipes.instance.GetDishRecipeListCount();

            bool PassCheck = false;

            int recipeindex = 0;
            // Do not allow duplicate recipes to appear
            while (!PassCheck)
            {
                recipeindex = Random.Range(1, PossibleRecipes);
                PassCheck = true;
                for (int x = 0; x < currentdayrecipesList.Count; x++)
                {
                    // if the recipe alr exist, do not add, and check again
                    if (Recipes.instance.GetDishRecipe(recipeindex) == currentdayrecipesList[x])
                        PassCheck = false;
                }
            }
            currentdayrecipesList.Add(Recipes.instance.GetDishRecipe(recipeindex));
        }
    }

    public Recipes.recipes GetRandomOrderFromCurrentDay()
    {
        int index = Random.Range(0, currentdayrecipesList.Count);

        return currentdayrecipesList[index];
    }

    public Recipes.recipes GetSelectedOrderFromCurrentDay(int index)
    {
        if (index < currentdayrecipesList.Count && index >= 0)
        {
            return currentdayrecipesList[index];
        }

        return GetRandomOrderFromCurrentDay();
    }

    public float GetCurrentDayWaitingTime()
    {
        return presetday.WaitingTime;
    }

    public int GetPenalty()
    {
        return presetday.penalty;
    }
}