using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayShopItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ItemName;
    [SerializeField] Image image;
    [SerializeField] GameObject iconEmpty;
    [SerializeField] Sprite upgradeUnlock;
    [SerializeField] Sprite upgradeLock;
    [SerializeField] Sprite UpgradeBackground;
    [SerializeField] Sprite UpgradeBackground_Glow;
    [SerializeField] Sprite UpgradeBackground_Gray;
    List<Image> UpgradeIcon = new List<Image>();
    ShopItem shopItem;
    bool IsActive;

    private void Awake()
    {
        Image[] temparray = iconEmpty.GetComponentsInChildren<Image>();

        for (int i = 0; i < temparray.Length; i++)
        {
            UpgradeIcon.Add(temparray[i]);
        }
        IsActive = true;
    }

    public void UpdateShopItemDisplay(ShopItem item)
    {
        IsActive = true;
        gameObject.GetComponent<Image>().sprite = UpgradeBackground;
        shopItem = item;
        ItemName.text = shopItem.Name;
        image.sprite = shopItem.image;

        for (int i = 0; i < UpgradeIcon.Count; i++)
        {
            if (i + 1 <= shopItem.GetCurrentLevel())
            {
                UpgradeIcon[i].sprite = upgradeUnlock;
            }
            else
                UpgradeIcon[i].sprite = upgradeLock;
        }
        gameObject.GetComponent<Button>().interactable = true;
    }

    public void DisableSlot()
    {
        gameObject.GetComponent<Image>().sprite = UpgradeBackground_Gray;
        Transform[] Array = gameObject.GetComponentsInChildren<Transform>();

        for (int i = 1; i < Array.Length; i++)
        {
            Array[i].gameObject.SetActive(false);
        }
        IsActive = false;
        gameObject.GetComponent<Button>().interactable = false;
    }

    public void HightlightOn()
    {
        gameObject.GetComponent<Image>().sprite = UpgradeBackground_Glow;
    }

    public void HightlightOff()
    {
        gameObject.GetComponent<Image>().sprite = UpgradeBackground;
    }

    public bool GetIsActive()
    {
        return IsActive;
    }
}
