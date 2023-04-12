using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "ShopItem")]
public class ShopItem : ScriptableObject
{
    [SerializeField] UpgradeSystem.ShopItemType Itemtype; // what item is it
    public string Name; // Name of the item
    public string Description; // Description of the item
    public int BaseCost; // The base cost of the item
    public int CostIncrement; // How much is the cost increase by per purchase
    public int UpgradeLevels; // Determine how many levels is there for the upgrade
    public Sprite image;

    private int currentLevel; // the current level for the upgrade. Starts at 0

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void SetLevel(int ChangeAmt)
    {
        currentLevel += ChangeAmt;
    }

    public void ResetLevel()
    {
        currentLevel = 0;
    }
    
    public UpgradeSystem.ShopItemType GetShopItemType()
    {
        return Itemtype;
    }
}
