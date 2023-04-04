using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "playerProgress", menuName = "PlayerProgress")]
public class PlayerProgress : ScriptableObject
{
    public int PlayerMaxHealth;
    public int MaxInventorySlots;
    public int CosmicCredibility;
}