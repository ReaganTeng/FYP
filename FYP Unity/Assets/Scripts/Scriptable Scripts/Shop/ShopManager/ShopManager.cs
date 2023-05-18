using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ShopManager", menuName = "ShopManager")]
public class ShopManager : ScriptableObject
{
    public List<ShopItem> shopList;
    public bool LoadUpgrade = false;

    public void ResetShopLevel()
    {
        for (int i = 0; i < shopList.Count; i++)
        {
            shopList[i].ResetLevel();
        }
    }

    public void AssignUpgrades()
    {
        for (int i = 0; i < shopList.Count; i++)
        {
            int AmtOfUpgrade = shopList[i].GetCurrentLevel();
            for (int index = 0; index < AmtOfUpgrade; index++)
            {
                UpgradeSystem.instance.Upgrade(shopList[i]);
            }
        }
    }
}