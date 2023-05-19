using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "playerProgress", menuName = "PlayerProgress")]
public class PlayerProgress : ScriptableObject
{
    [SerializeField] int MaxInventorySlots = 5;
    [SerializeField] int CosmicCredibility;
    [SerializeField] int TotalObtainedCredibility;
    private int CurrentInventorySlots;

    [SerializeField] float buffactive_reduction;
    [SerializeField] float fervorspeedreduction;
    [SerializeField] float heavyattackspeed;
    [SerializeField] int number_of_charges;

    //starting value 1
    [SerializeField] float heavyattackspeed_reduction;

    //starting value 1
    [SerializeField] float fervorloss_padding;

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



    public void increase_fevor_padding()
    {
        fervorloss_padding -= 0.15f;
    }
    public float return_fevor_padding()
    {
        return fervorloss_padding;
    }

    public void decrease_heavyattackrecovery()
    {
        heavyattackspeed_reduction += 0.15f;
    }
    public float return_heavyattackrecovery()
    {
        return heavyattackspeed_reduction;
    }

    public void increase_number_of_charges()
    {
        number_of_charges += 1;
    }

    public int return_number_of_charges()
    {
        return number_of_charges;
    }

    public void increase_heavyattackspeed()
    {
        heavyattackspeed += 3;
    }
    public float return_heavyattackspeed()
    {
        return heavyattackspeed;
    }

    public void reduce_fervorspeed()
    {
        fervorspeedreduction -= .25f;
    }
    public float return_fervorspeed()
    {
        return fervorspeedreduction;
    }


    public void reduce_buffactive_requirement()
    {
        buffactive_reduction += 20;
    }
    public float return_buffactive_requirement()
    {
        return buffactive_reduction;
    }


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
        CurrentInventorySlots = MaxInventorySlots;
    }

    // Increase inventory slot by one
    public void IncreaseInventorySize()
    {
        if (MaxInventorySlots < 10)
            MaxInventorySlots += 1;
    }

    public int GetInventorySize()
    {
        CurrentInventorySlots = MaxInventorySlots;
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