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
        HEALTH_UP,
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

            case ShopItemType.HEALTH_UP:
                pp.PlayermaxHealth += 10;
                break;
        }
    }
}
