using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    [SerializeField] PlayerProgress pp;
    [SerializeField] GameObject ShopUI;
    [SerializeField] LevelSelectManager lsm;
    [SerializeField] int ItemPerPanel;
    [SerializeField] List<GameObject> shopItemSlots;
    [SerializeField] List<ShopItem> shopItem;
    [SerializeField] GameObject itemPanel;
    [SerializeField] TextMeshProUGUI CurrencyAmt;
    [SerializeField] Sprite EnabledButton;
    [SerializeField] Sprite DisabledButton;
    [SerializeField] Sprite MaxedButton;
    [SerializeField] ShopManager sm;
    int CurrentPanel;
    ShopItem panelItem;

    private void Start()
    {
        ShopUI.SetActive(false);
        CurrentPanel = 0;
        itemPanel.SetActive(false);
        panelItem = null;
        if (sm.LoadUpgrade)
        {
            sm.AssignUpgrades();
            sm.LoadUpgrade = false;
        }
    }

    public void SelectItem(int itemNo)
    {
        int WhichItem = CurrentPanel * ItemPerPanel + itemNo - 1;
        // Display on panel
        DisplayOnPanel(shopItem[WhichItem]);

        for (int i = 0; i < ItemPerPanel; i++)
        {
            if (i == itemNo - 1)
            {
                shopItemSlots[i].GetComponent<DisplayShopItem>().HightlightOn();
            }
            else if (shopItemSlots[i].GetComponent<DisplayShopItem>().GetIsActive())
            {
                shopItemSlots[i].GetComponent<DisplayShopItem>().HightlightOff();
            }
        }
    }

    public void OpenShop()
    {
        ShopUI.SetActive(true);
        UpdateShopUI();
    }

    public void CloseShop()
    {
        ShopUI.SetActive(false);
        lsm.UpdateUI();
        CurrentPanel = 0;
        itemPanel.SetActive(false);
        panelItem = null;
    }

    void UpdateShopUI(bool DisableGlow = true)
    {
        for (int i = 0; i < ItemPerPanel; i++)
        {
            DisplayShopItem dsi = shopItemSlots[i].GetComponent<DisplayShopItem>();
            if (i + (CurrentPanel * ItemPerPanel) < shopItem.Count)
            {
                dsi.UpdateShopItemDisplay(shopItem[CurrentPanel * ItemPerPanel + i], DisableGlow);
            }
            else
            {
                dsi.DisableSlot();
            }
        }
        CurrencyAmt.text = pp.GetCurrentCC().ToString();
    }

    public void SwitchPanel(int panelNo)
    {
        CurrentPanel = panelNo;
        UpdateShopUI();
    }

    int CalculateCost(ShopItem item)
    {
        int Cost = item.BaseCost + (item.CostIncrement * item.GetCurrentLevel());
        return Cost;
    }

    void DisplayOnPanel(ShopItem item)
    {
        itemPanel.SetActive(true);
        TextMeshProUGUI[] temp = itemPanel.GetComponentsInChildren<TextMeshProUGUI>();

        // Item Name
        temp[0].text = item.Name;
        // Item Description
        temp[1].text = item.Description;
        // Cost of item
        temp[2].text = CalculateCost(item).ToString();

        itemPanel.GetComponentInChildren<Image>().sprite = item.image;

        // Enable/Disable the button depending on whetehr player has enough CC to afford it
        // If player has max lvl on that item, lock it and display maxed
        if (item.GetCurrentLevel() >= item.UpgradeLevels)
        {
            itemPanel.GetComponentsInChildren<Image>()[1].sprite = MaxedButton;
            itemPanel.GetComponentInChildren<Button>().interactable = false;
            temp[2].text = "-";
        }
        // Enable if player has enough cc and it is not maxed level
        else if (pp.GetCurrentCC() >= CalculateCost(item))
        {
            itemPanel.GetComponentsInChildren<Image>()[1].sprite = EnabledButton;
            itemPanel.GetComponentInChildren<Button>().interactable = true;
        }
        else
        {
            itemPanel.GetComponentsInChildren<Image>()[1].sprite = DisabledButton;
            itemPanel.GetComponentInChildren<Button>().interactable = false;
        }

        panelItem = item;
    }

    public void PurchaseItem()
    {
        // deduct the cost and increase the level by 1
        for(int i = 0; i < shopItem.Count; i++)
        {
            if (panelItem == shopItem[i])
            {
                pp.DecreaseCredibility(CalculateCost(shopItem[i]));
                shopItem[i].SetLevel(1);
                UpgradeSystem.instance.Upgrade(shopItem[i]);
                UpdateShopUI(false);
                DisplayOnPanel(shopItem[i]);
                // break the loop
                i = shopItem.Count;
            }
        }
    }

    public void ResetShopLevel()
    {
        for (int i = 0; i < shopItem.Count; i++)
        {
            shopItem[i].ResetLevel();
        }
        UpdateShopUI();
    }
}
