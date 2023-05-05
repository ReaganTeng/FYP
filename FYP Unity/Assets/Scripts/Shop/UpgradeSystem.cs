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
        HEAVY_ATTACK_BUFF,
        BUFF_MODE_AMOUNT_REQUIREMENT,
        FERVOR_REDUCTION
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
                pp.MaxInventorySlots += 1;
                break;
            
            case ShopItemType.BUFF_MODE_AMOUNT_REQUIREMENT:
                pp.reduce_buffactive_requirement();
                break;
            case ShopItemType.FERVOR_REDUCTION:
                pp.reduce_fervorspeed();
                break;
            case ShopItemType.HEAVY_ATTACK_BUFF:
                pp.increase_heavyattackspeed();
                break;
        }
    }
}
