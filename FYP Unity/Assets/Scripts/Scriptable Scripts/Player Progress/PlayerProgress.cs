using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "playerProgress", menuName = "PlayerProgress")]
public class PlayerProgress : ScriptableObject
{
    [SerializeField] int BaseInventorySlots = 5;
    [SerializeField] int CosmicCredibility;
    [SerializeField] int TotalObtainedCredibility;
    private int CurrentInventorySlots;

    //RATION
    //starting value 0
    [SerializeField] int enemy_killed_requirement;
    public void update_ration()
    {
        if (enemy_killed_requirement == 0)
        {
            enemy_killed_requirement = 10;
        }
        else
        {
            enemy_killed_requirement -= 2;
        }
    }
    public int return_ration()
    {
        return enemy_killed_requirement;
    }
    //


    //JUST DIE ALREADY
    //starting value = 0
    [SerializeField] float fervor_requirement;
    public void update_justdiealready()
    {
        if(fervor_requirement == 0)
        {
            fervor_requirement = 70;
        }
        else
        {
            fervor_requirement -= 10;
        }
    }
    public float return_justdiealready()
    {
        return fervor_requirement;
    }
    //



    

    //DINNER RUSH
    //starting value 0
    [SerializeField] float burst_time;
    public void update_dinner_rush()
    {
        burst_time += 5;
    }
    public float return_dinner_rush()
    {
        return burst_time;
    }
    //



    private int mixer_Reduction = 0;
    private int better_cooker = 0;
    private int longer_order = 0;
    private float perfect_dish_boost = 0;
    private int frenzy_mode_Max_stack = 0;


    //RUSH OF PERFECTION
    //starting value 0
    [SerializeField] int charge_reward;
    public void update_rush_of_perfection()
    {
        charge_reward += 1;
    }
    public int return_rush_of_perfection()
    {
        return charge_reward;
    }
    //

    //THICK SKIN
    //starting value 1
    [SerializeField] float fervorloss_padding;
    public void update_thick_skin()
    {
        fervorloss_padding -= 0.15f;
    }
    public float return_thick_skin()
    {
        return fervorloss_padding;
    }
    //


    //BETTER STAMINA
    //starting value 1
    [SerializeField] float heavyattackspeed_reduction;
    public void update_better_stamina()
    {
        heavyattackspeed_reduction += 0.20f;
    }
    public float return_better_stamina()
    {
        return heavyattackspeed_reduction;
    }
    //



    //STURDY ARM
    //starting value 0
    [SerializeField] int number_of_charges;
    public void update_sturdy_arm()
    {
        number_of_charges += 1;
    }
    public int return_sturdy_arm()
    {
        return number_of_charges;
    }
    //

    //CALM MIND
    //starting value 1
    [SerializeField] float fervorspeedreduction;
    public void update_calmmind()
    {
        fervorspeedreduction -= .18f;
    }
    public float return_calmmind()
    {
        return fervorspeedreduction;
    }
    //

    //HYPER FOCUSED COOKING
    //starting value 30
    [SerializeField] float buffactive_reduction;
    public void update_hyper_focused_cooking()
    {
        buffactive_reduction += 13;
    }
    public float return_hyper_focused_cooking()
    {
        return buffactive_reduction;
    }
    //



    public void IncreaseMixerReduction()
    {
        mixer_Reduction += 3;
    }
    public int GetMixerReduction()
    {
        return mixer_Reduction;
    }
    
    public void IncreaseBetterCooker()
    {
        better_cooker += 1;
    }
    public int GetBetterCooker()
    {
        return better_cooker;
    }

    public void IncreaseLongerOrderTime()
    {
        longer_order += 10;
    }
    public int GetLongerOrderTime()
    {
        return longer_order;
    }

    public void IncreasePerfectDishBoost()
    {
        perfect_dish_boost += 0.2f;
    }
    public float GetPerfectDishBoost()
    {
        return perfect_dish_boost;
    }

    public void IncreaseFrenzyModeMaxStack()
    {
        frenzy_mode_Max_stack += 1;
    }
    public int GetFrenzyModeMaxStack()
    {
        return frenzy_mode_Max_stack;
    }

    public void ResetInventory()
    {
        CurrentInventorySlots = BaseInventorySlots;
    }

    // Increase inventory slot by one
    public void IncreaseInventorySize()
    {
        if (CurrentInventorySlots < 10)
            CurrentInventorySlots += 1;
    }

    public int GetInventorySize()
    {
        return CurrentInventorySlots;
    }


    public void AddCredibility(int amt)
    {
        CosmicCredibility += amt;
        TotalObtainedCredibility += amt;
    }

    public void DecreaseCredibility(int amt)
    {
        CosmicCredibility -= amt;
    }    

    public void ResetCredibility()
    {
        CosmicCredibility = 2;
        TotalObtainedCredibility = 0;
    }

    public void RefundCredibility()
    {
        CosmicCredibility = TotalObtainedCredibility + 2;
    }

    public int GetCurrentCC()
    {
        return CosmicCredibility;
    }

    public int GetMaxCC()
    {
        return TotalObtainedCredibility;
    }

    public void ResetPlayer()
    {
        ResetCredibility();
        ResetShopStats();
    }

    public void ResetShopPlayer()
    {
        RefundCredibility();
        ResetShopStats();
    }

    void ResetShopStats()
    {
        ResetInventory();

        //starting value 30
        buffactive_reduction = 30;

        number_of_charges = 0;


        //starting value 1
        fervorspeedreduction = 1;

        //starting value 1
        heavyattackspeed_reduction = 1;

        //starting value 1
        fervorloss_padding = 1;

        charge_reward = 0;

        //starting value 0
        enemy_killed_requirement = 0;
        //starting value = 0
        fervor_requirement = 0;
        //starting value 0
        burst_time = 0;
        

        mixer_Reduction = 0;
        better_cooker = 0; 
        longer_order = 0;
        perfect_dish_boost = 0;
        frenzy_mode_Max_stack = 0;
    }
}