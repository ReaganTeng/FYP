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
        HYPER_FOCUSED_COOKING,
        CALM_MIND,
        STURDY_ARM,
        BETTER_STAMINA,
        THICK_SKIN,

        RUSH_OF_PERFECTION,
        RATION,
        JUST_DIE_ALREADY,
        DINNER_RUSH



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
            
            case ShopItemType.HYPER_FOCUSED_COOKING:
                pp.reduce_buffactive_requirement();
                break;
            case ShopItemType.CALM_MIND:
                pp.reduce_fervorspeed();
                break;
            case ShopItemType.STURDY_ARM:
                pp.increase_number_of_charges();
                break;
            case ShopItemType.BETTER_STAMINA:
                pp.decrease_heavyattackrecovery();
                break;
            case ShopItemType.THICK_SKIN:
                pp.increase_thick_skin();
                break;
            case ShopItemType.RUSH_OF_PERFECTION:
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
            case ShopItemType.RATION:
                pp.increase_rations();
                break;
        }
    }

    
}
