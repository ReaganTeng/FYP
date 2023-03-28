using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager instance;
    [System.Serializable]
    public class DayOrder
    {
        public List<string> dishName = new List<string>();
        public int whatday;
        // Waiting time
        public float WaitingTime;
        // Penalty for not serving a dish
        public int penalty;
        // if this is check, the dish set will only appear on that day, if this is uncheck, u can set how many random orders to set
        public bool Fixed;
        //Set the amt of dishes for the day, will be ignore if Fixed is true
        public int AmtOfDishes;
    }

    // A list that store any pre-determined orders that has to come out for the day;
    [SerializeField]
    List<DayOrder> presetdaylist;

    List<Recipes.recipes> currentdayrecipesList = new List<Recipes.recipes>();

    // Stores the current day
    [SerializeField] int currentday;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
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
        int index = 0;
        // Check to see if there is any preset values for that day
        for (int i = 0; i < presetdaylist.Count; i++)
        {
            if (currentday == presetdaylist[i].whatday)
            {
                // set the index to that pos in the list
                index = i;
                PresetDay = true;
                break;
            }
        }

        // if there is a presetday, use it
        if (PresetDay)
        {
            for (int i = 0; i < presetdaylist[index].dishName.Count; i++)
            {
                Recipes.recipes temprecipe = Recipes.instance.GetDishRecipe(presetdaylist[index].dishName[i]);
                currentdayrecipesList.Add(temprecipe);
            }

            // If the day is fixed, do not need to care about the rest, if it is not fixed, fill in random recipes if needed
            if (!presetdaylist[index].Fixed)
            {
                // check to see what extra dishes needs to be fulfill if any
                if (presetdaylist[index].AmtOfDishes > currentdayrecipesList.Count)
                {
                    int AmtOfRecipesToAdd = presetdaylist[index].AmtOfDishes - currentdayrecipesList.Count;
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

    public float GetCurrentDayWaitingTime()
    {
        for (int i = 0; i < presetdaylist.Count; i++)
        {
            if (currentday == presetdaylist[i].whatday)
            {
                return presetdaylist[i].WaitingTime;
            }
        }
        return 0;
    }

    public int GetPenalty()
    {
        for (int i = 0; i < presetdaylist.Count; i++)
        {
            if (currentday == presetdaylist[i].whatday)
            {
                return presetdaylist[i].penalty;
            }
        }
        // by default -10 for penalty
        return 10;
    }
}
