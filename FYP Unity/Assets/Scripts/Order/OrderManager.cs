using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager instance;
    [System.Serializable]
    public class DayOrder
    {
        public List<DifficultyPriority> dishList = new List<DifficultyPriority>(); // contains the list of dishes that will appear that day
        public int penalty = 10; // Penalty for not serving a dish
        public int EasyDishChance = 0; // Set the chance for easy dish to appear
        public int NormalDishChance = 0; // Set the chance for normal dish to appear
        public int HardDishChance = 0; // Set the chance for hard dish to appear
        public int ExtremeDishChance = 0; // Set the chance extreme easy dish to appear
    }

    [System.Serializable]
    public class DifficultyPriority
    {
        public Dish theDish; // what Dish is it
        public DishManager.Difficulty whatDifficulty; // leave blank if there is a dish input
        public bool IsPrority = false; // Set a priority to that dish/difficulty, they will always appear depending on the amt you set.
        public int HowManyPriority = 1; // Set as priority, but how many will appear?
        public int DishBeforePriority = 1; // How many dishes has to come first before this dish is a priority
        public int IntervalBetweenPriority = 1; // How many dishes has to appear after the priortized before being in priority again.
    }

    // Get the reference to the level manager which stores the list of levels.
    [SerializeField] LevelManager lm;

    // The preset day list from the level itself
    DayOrder presetday;

    List<Recipes.recipes> currentdayrecipesList = new List<Recipes.recipes>();
    List<DifficultyPriority> dishPriority = new List<DifficultyPriority>();

    [SerializeField] EndOfDay eod;

    // to check for %
    private int TotalChance = 0;
    bool HaveEasy = false;
    bool HaveNormal = false;
    bool HaveHard = false;
    bool HaveExtreme = false;
    int EasyChance = 0;
    int NormalChance = 0;
    int HardChance = 0;
    int ExtremeChance = 0;

    // Stores the reference to the day
    private Level level;

    private void Awake()
    {
        instance = this;
        level = eod.GetLevelReference();
        presetday = level.CustomizeOrderForThisDay;
    }

    private void Start()
    {
        // set the orders that can come out
        SetOrdersForTheDay();
    }

    public void SetOrdersForTheDay()
    {
        for (int i = 0; i < presetday.dishList.Count; i++)
        {
            // Check to see if it is a specific dish or a random difficulty.
            if (presetday.dishList[i].theDish != null)
            {
                currentdayrecipesList.Add(Recipes.instance.GetDishRecipe(presetday.dishList[i].theDish.GetDishName()));
            }
            else
            {
                GenerateRandomRecipe(presetday.dishList[i].whatDifficulty);
            }

            // if it is a priority dish, throw it inside a list
            if (presetday.dishList[i].IsPrority)
            {
                dishPriority.Add(CopyDifficultyPriority(presetday.dishList[i]));
            }
        }
        CheckChance();
    }

    void GenerateRandomRecipe(DishManager.Difficulty whatDifficulty)
    {
        // get the list of dish according to the difficulty of the dishes
        List<Recipes.recipes> tempList = new List<Recipes.recipes>();
        tempList = Recipes.instance.GetDishList(whatDifficulty);
        // Get a random recipe from the the difficulty, if it already exist inside the currentdaylist, regenrate again
        bool ValidRecipe = false;
        while (!ValidRecipe)
        {
            int index = Random.Range(0, tempList.Count);

            if (currentdayrecipesList.Count == 0)
            {
                currentdayrecipesList.Add(tempList[index]);
            }

            else
            {
                for (int i = 0; i < currentdayrecipesList.Count; i++)
                {
                    // if the recipe chosen does not exist in current day list yet, add it, and end the loop
                    if (tempList[index] != currentdayrecipesList[i])
                    {
                        currentdayrecipesList.Add(tempList[index]);
                        ValidRecipe = true;
                    }
                }
            }
        }

    }

    public Recipes.recipes GetRandomOrderFromCurrentDay()
    {
        Recipes.recipes theRecipe = null;
        // Check for any prioritised dish, begin wif the start first
        for (int i = 0; i < dishPriority.Count; i++)
        {
            if (dishPriority[i].DishBeforePriority == 0)
            {
                theRecipe = CompareRecipe(dishPriority[i].theDish);
                dishPriority[i].HowManyPriority -= 1;

                // if there is no more priority, remove from the list
                if (dishPriority[i].HowManyPriority == 0)
                {
                    dishPriority.RemoveAt(i);
                    i--;
                }
            }

            if (dishPriority[i].DishBeforePriority >= 0)
            {
                dishPriority[i].DishBeforePriority -= 1;
            }
        }

        if (theRecipe != null)
            return theRecipe;

        // Check for any prioritised dish, check the interval one
        for (int i = 0; i < dishPriority.Count; i++)
        {
            if (dishPriority[i].DishBeforePriority >= 0)
                continue;

            if (dishPriority[i].IntervalBetweenPriority == 0)
            {
                theRecipe = CompareRecipe(dishPriority[i].theDish);
                dishPriority[i].HowManyPriority -= 1;
            }

            else
            {
                dishPriority[i].IntervalBetweenPriority -= 1;
            }

            // if there is no more priority, remove from the list
            if (dishPriority[i].HowManyPriority == 0)
            {
                dishPriority.RemoveAt(i);
                i--;
            }
        }

        if (theRecipe != null)
            return theRecipe;

        // If nothing else, do a random generation
        int chance = Random.Range(1, TotalChance + 1);
        

        if (HaveEasy && chance <= EasyChance)
        {
            theRecipe = ReturnRandomDish(DishManager.Difficulty.EASY);
        }

        else if (HaveNormal && chance <= EasyChance + NormalChance)
        {
            theRecipe = ReturnRandomDish(DishManager.Difficulty.NORMAL);
        }

        else if (HaveHard && chance <= EasyChance + NormalChance + HardChance)
        {
            theRecipe = ReturnRandomDish(DishManager.Difficulty.HARD);
        }

        else if (HaveExtreme && chance <= EasyChance + NormalChance + HardChance + ExtremeChance)
        {
            theRecipe = ReturnRandomDish(DishManager.Difficulty.EXTREME);
        }

        return theRecipe;
    }

    private Recipes.recipes CompareRecipe(Dish dish)
    {
        for (int i = 0; i < currentdayrecipesList.Count; i++)
        {
            if (dish == currentdayrecipesList[i].Result.GetComponent<Dish>())
            {
                return currentdayrecipesList[i];
            }
        }
        return null;
    }

    private void CheckChance()
    {
        // Check to see if there is any error in terms of the checking %
        EasyChance = presetday.EasyDishChance;
        NormalChance = presetday.NormalDishChance;
        HardChance = presetday.HardDishChance;
        ExtremeChance = presetday.ExtremeDishChance;


        for (int i = 0; i < currentdayrecipesList.Count; i++)
        {
            switch (currentdayrecipesList[i].Result.GetComponent<Dish>().GetDifficulty())
            {
                case DishManager.Difficulty.EASY:
                    HaveEasy = true;
                    break;
                case DishManager.Difficulty.NORMAL:
                    HaveNormal = true;
                    break;
                case DishManager.Difficulty.HARD:
                    HaveHard = true;
                    break;
                case DishManager.Difficulty.EXTREME:
                    HaveExtreme = true;
                    break;
            }
        }

        // add up all the % of values that are avaliable
        if (HaveEasy)
            TotalChance += presetday.EasyDishChance;
        else
            EasyChance = 0;

        if (HaveNormal)
            TotalChance += presetday.NormalDishChance;
        else
            NormalChance = 0;

        if (HaveHard)
            TotalChance += presetday.HardDishChance;
        else
            HardChance = 0;

        if (HaveExtreme)
            TotalChance += presetday.ExtremeDishChance;
        else
            ExtremeChance = 0;

        // Check what difficulty dish will come
    }

    private Recipes.recipes ReturnRandomDish(DishManager.Difficulty whatDifficulty)
    {
        // Check how many of that difficult dish are there inside
        List<int> indexList = new List<int>();
        for (int i = 0; i < currentdayrecipesList.Count; i++)
        {
            if (whatDifficulty == currentdayrecipesList[i].Result.GetComponent<Dish>().GetDifficulty())
            {
                indexList.Add(i);
            }
        }

        int whichOne = Random.Range(0, indexList.Count);

        return currentdayrecipesList[indexList[whichOne]];
    }    

    public Recipes.recipes GetSelectedOrderFromCurrentDay(int index)
    {
        if (index < currentdayrecipesList.Count && index >= 0)
        {
            return currentdayrecipesList[index];
        }

        return GetRandomOrderFromCurrentDay();
    }

    public int GetPenalty()
    {
        return presetday.penalty;
    }

    DifficultyPriority CopyDifficultyPriority(DifficultyPriority theReference)
    {
        DifficultyPriority copied = new DifficultyPriority();
        copied.theDish = theReference.theDish;
        copied.whatDifficulty = theReference.whatDifficulty;
        copied.theDish = theReference.theDish;
        copied.IsPrority = theReference.IsPrority;
        copied.HowManyPriority = theReference.HowManyPriority;
        copied.DishBeforePriority = theReference.DishBeforePriority;
        copied.IntervalBetweenPriority = theReference.IntervalBetweenPriority;

        return copied;
    }
}