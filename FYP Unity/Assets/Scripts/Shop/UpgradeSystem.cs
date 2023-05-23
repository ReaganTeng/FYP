using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    [SerializeField] PlayerProgress pp;
    public enum ShopItemType
    {        
        BETTER_COOKER,
        LONGER_ORDERS,
        BETTER_PERFECT_DISH,
        PERFECT_FRENZY,

        /// GENERAL
        INVENTORY_UPGRADE,
        HYPER_FOCUSED_COOKING,
        CALM_MIND,
        THICK_SKIN,
        STURDY_ARM,
        BETTER_STAMINA,
        /// 

        /// FAST SERVING
        FASTER_COOKER,
        RATION,
        JUST_DIE_ALREADY,
        DINNER_RUSH,
        /// 

        RUSH_OF_PERFECTION,



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
            ///GENERAL SHOP ITEMS
            case ShopItemType.INVENTORY_UPGRADE:
                pp.IncreaseInventorySize();
                break;            
            case ShopItemType.HYPER_FOCUSED_COOKING:
                pp.update_hyper_focused_cooking();
                break;
            case ShopItemType.CALM_MIND:
                pp.update_calmmind();
                break;
            case ShopItemType.THICK_SKIN:
                pp.update_thick_skin();
                break;
            case ShopItemType.STURDY_ARM:
                pp.update_sturdy_arm();
                break;
            case ShopItemType.BETTER_STAMINA:
                pp.update_better_stamina();
                break;
            ///

            case ShopItemType.RUSH_OF_PERFECTION:
                pp.update_rush_of_perfection();
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
            case ShopItemType.BETTER_PERFECT_DISH:
                pp.IncreasePerfectDishBoost();
                break;
            case ShopItemType.PERFECT_FRENZY:
                pp.IncreaseFrenzyModeMaxStack();
                break;
            case ShopItemType.RATION:
                pp.update_ration();
                break;

            case ShopItemType.JUST_DIE_ALREADY:
                pp.update_justdiealready();
                break;
            case ShopItemType.DINNER_RUSH:
                pp.update_dinner_rush();
                break;
        }
    }

    
}
