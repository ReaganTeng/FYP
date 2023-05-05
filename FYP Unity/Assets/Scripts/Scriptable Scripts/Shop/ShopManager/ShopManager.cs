using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ShopManager", menuName = "ShopManager")]
public class ShopManager : ScriptableObject
{
    public List<ShopItem> shopList;

    public void ResetShopLevel()
    {
        for (int i = 0; i < shopList.Count; i++)
        {
            shopList[i].ResetLevel();
        }
    }
}