using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "playerProgress", menuName = "PlayerProgress")]
public class PlayerProgress : ScriptableObject
{
    public int MaxInventorySlots;
    public int PlayermaxHealth;
    [SerializeField] int CosmicCredibility;
    [SerializeField] int TotalObtainedCredibility;

    [SerializeField] float buffactive_reduction;


    [SerializeField] float fervorspeedreduction;

    [SerializeField] float heavyattackspeed;


    public void increase_heavyattackspeed()
    {
        heavyattackspeed += 1;
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

    public void ResetInventory()
    {
        MaxInventorySlots = 5;
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

    public int GetCurrentCC()
    {
        return CosmicCredibility;
    }

    public int GetMaxCC()
    {
        return TotalObtainedCredibility;
    }



}