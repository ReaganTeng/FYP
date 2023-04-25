using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "playerProgress", menuName = "PlayerProgress")]
public class PlayerProgress : ScriptableObject
{
    public int MaxInventorySlots;
    [SerializeField] int CosmicCredibility;
    [SerializeField] int TotalObtainedCredibility;


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