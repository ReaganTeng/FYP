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
    [SerializeField] int amt_of_rations;
    public void increase_rations()
    {
        amt_of_rations += 2;
    }
    public int return_rations()
    {
        return amt_of_rations;
    }
    //


    //JUST DIE ALREADY
    //starting value = 0
    [SerializeField] float fervor_requirement;
    public void set_fervor_requirement()
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
    public float return_fervor_requirement()
    {
        return fervor_requirement;
    }
    //


    //DINNER RUSH
    //starting value 0
    [SerializeField] float burst_time;
    public void set_burst_time()
    {
        burst_time += 5;
    }
    public float return_burst_time()
    {
        return burst_time;
    }
    //



    //RUSH OF PERFECTION
    //starting value 0
    [SerializeField] int charge_reward;

    private int mixer_Reduction = 0;
    private int better_cooker = 0;
    private int longer_order = 0;
    private float perfect_dish_boost = 0;
    private int frenzy_mode_Max_stack = 0;


    public void increase_charge_reward()
    {
        charge_reward += 1;
    }
    public int return_charge_reward()
    {
        return charge_reward;
    }
    //

    //THICK SKIN
    //starting value 1
    [SerializeField] float fervorloss_padding;
    public void increase_thick_skin()
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
    public void decrease_heavyattackrecovery()
    {
        heavyattackspeed_reduction += 0.15f;
    }
    public float return_heavyattackrecovery()
    {
        return heavyattackspeed_reduction;
    }
    //

    //STURDY ARM
    [SerializeField] int number_of_charges;
    public void increase_number_of_charges()
    {
        number_of_charges += 1;
    }
    public int return_number_of_charges()
    {
        return number_of_charges;
    }
    //

    //CALM MIND
    [SerializeField] float fervorspeedreduction;
    public void reduce_fervorspeed()
    {
        fervorspeedreduction -= .25f;
    }
    public float return_fervorspeed()
    {
        return fervorspeedreduction;
    }
    //

    //HYPER FOCUSED COOKING
    [SerializeField] float buffactive_reduction;
    public void reduce_buffactive_requirement()
    {
        buffactive_reduction += 20;
    }
    public float return_buffactive_requirement()
    {
        return buffactive_reduction;
    }
    //



    public void IncreaseMixerReduction()
    {
        mixer_Reduction += 5;
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
        CosmicCredibility = 0;
        TotalObtainedCredibility = 0;
    }

    public void RefundCredibility()
    {
        CosmicCredibility = TotalObtainedCredibility;
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
        buffactive_reduction = 30;
        number_of_charges = 0;

        //starting value 1
        heavyattackspeed_reduction = 1;

        //starting value 1
        fervorloss_padding = 1;

        charge_reward = 0;

        mixer_Reduction = 0;
        better_cooker = 0; 
        longer_order = 0;
        perfect_dish_boost = 0;
        frenzy_mode_Max_stack = 0;
    }
}