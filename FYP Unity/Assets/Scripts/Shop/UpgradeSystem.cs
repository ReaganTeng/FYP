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
        FERVOR_REDUCTION,
        CHARGE_INCREASE,
        HEAVYATTACK_RECOVERY,
        FERVORLOSS_PADDING,
        CHARGE_REWARD,
        FASTER_COOKER,
        BETTER_COOKER,
        LONGER_ORDERS,
        BETTER_PERFECT,
        PERFECT_FRENZY,
    }

    public static UpgradeSystem instance;

    void Awake()
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
            
            case ShopItemType.BUFF_MODE_AMOUNT_REQUIREMENT:
                pp.reduce_buffactive_requirement();
                break;
            case ShopItemType.FERVOR_REDUCTION:
                pp.reduce_fervorspeed();
                break;
            case ShopItemType.HEAVY_ATTACK_BUFF:
                pp.increase_heavyattackspeed();
                break;
            case ShopItemType.CHARGE_INCREASE:
                pp.increase_number_of_charges();
                break;
            case ShopItemType.HEAVYATTACK_RECOVERY:
                pp.decrease_heavyattackrecovery();
                break;
            case ShopItemType.FERVORLOSS_PADDING:
                pp.increase_fevor_padding();
                break;
            case ShopItemType.CHARGE_REWARD:
                pp.increase_charge_reward();
                break;
            case ShopItemType.FASTER_COOKER:
                pp.IncreaseMixerReduction();
                break;
            case ShopItemType.BETTER_COOKER:
                pp.IncreaseBetterCooker();
                break;
            case ShopItemType.LONGER_ORDERS:
                pp.IncreaseLongerOrderTime();
                break;
            case ShopItemType.BETTER_PERFECT:
                pp.IncreasePerfectDishBoost();
                break;
            case ShopItemType.PERFECT_FRENZY:
                pp.IncreaseFrenzyModeMaxStack();
                break;
        }
    }

    
}
