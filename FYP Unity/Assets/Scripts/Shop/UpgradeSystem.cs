using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    [SerializeField] PlayerProgress pp;
    public enum ShopItemType
    {
        NONE,
        INVENTORY_UPGRADE,


        LIGHT_ATTACK_SPEED_BUFF,
        HEAVY_ATTACK_SPEED_BUFF

    }

    public static UpgradeSystem instance;

    void Start()
    {
        instance = this;
    }

    public void Upgrade(ShopItem item)
    {
        switch (item.GetShopItemType())
        {
            case ShopItemType.INVENTORY_UPGRADE:
                pp.IncreaseInventorySize();
                break;            
        }
    }
}
