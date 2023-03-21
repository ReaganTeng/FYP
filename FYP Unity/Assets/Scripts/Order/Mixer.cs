using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mixer : MonoBehaviour
{
    [SerializeField] GameObject mixingBG;
    [SerializeField] GameObject ingredient1;
    [SerializeField] GameObject ingredient2;
    [SerializeField] GameObject cookingBG;
    [SerializeField] Text text;
    [SerializeField] GameObject doneBG;
    [SerializeField] int MixingTime;
    [SerializeField] Dish SusDish;
    float mixingtimer;
    bool IsMixing;
    int RecipeResultID = 0;
    bool CanPutIntoMixer;
    bool MixerDone;

    // List that contains the ingredients inputted into the mixer
    List<Item> mixercontent = new List<Item>();

    private void Start()
    {
        ResetMixer();
        CanPutIntoMixer = true;
        MixerDone = false;
    }

    public bool AddIntoMixer()
    {
        InventoryImageControl inventory = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InventoryImageControl>();

        // make sure player has something selected and the mixer contains less than 2 ingredient inside
        if (inventory.GetSelectedInventory(false) != -1 && mixercontent.Count < 2)
        {
            mixercontent.Add(inventory.GetItem());
            RenderMixingMenu();
            return true;
        }
        return false;
    }

    public int GetMixerAmount()
    {
        return mixercontent.Count;
    }

    public void StartMixing()
    {
        RecipeResultID = Recipes.instance.GetRecipeResult(mixercontent[0].GetItemID(), mixercontent[1].GetItemID());
        Debug.Log("DishID result: " + RecipeResultID);
        ResetMixer();
        Mixing();
        Debug.Log("Mixing in process, please stand by..");
        IsMixing = true;
        // Set the timer background to true
        cookingBG.SetActive(true);
        CanPutIntoMixer = false;
    }

    void ResetMixer()
    {
        mixercontent.Clear();
        mixingBG.SetActive(false);
        ingredient1.SetActive(false);
        ingredient2.SetActive(false);
        cookingBG.SetActive(false);
        doneBG.SetActive(false);
        mixingtimer = MixingTime;
        IsMixing = false;
    }

    void RenderMixingMenu()
    {
        mixingBG.SetActive(true);
        ingredient1.SetActive(false);
        ingredient2.SetActive(false);
        cookingBG.SetActive(false);
        doneBG.SetActive(false);

        if (mixercontent.Count >= 1)
        {
            ingredient1.SetActive(true);
            ingredient1.GetComponent<Image>().sprite = mixercontent[0].GetImage();
        }

        if (mixercontent.Count >= 2)
        {
            ingredient2.SetActive(true);
            ingredient2.GetComponent<Image>().sprite = mixercontent[1].GetImage();
        }
    }

    void Mixing()
    {
        text.text = ((int)mixingtimer).ToString();
    }

    void DoneMixing()
    {
        mixingBG.SetActive(false);
        ingredient1.SetActive(false);
        ingredient2.SetActive(false);
        cookingBG.SetActive(false);
        doneBG.SetActive(true);
    }

    public void ResetMixerEntirely()
    {
        mixercontent.Clear();
        mixingBG.SetActive(false);
        ingredient1.SetActive(false);
        ingredient2.SetActive(false);
        cookingBG.SetActive(false);
        doneBG.SetActive(false);
        mixingtimer = MixingTime;
        IsMixing = false;
        CanPutIntoMixer = true;
        MixerDone = false;
    }

    public Dish GetDishFromMixer()
    {
        Dish dish;
        if (RecipeResultID >= 0)
        {
            dish = Recipes.instance.GetRecipe(RecipeResultID).Result;
        }

        else
        {
            dish = SusDish;
        }

        // reset it
        RecipeResultID = 0;
        ResetMixerEntirely();
        return dish;
    }

    public bool GetCanPutIntoMixer()
    {
        return CanPutIntoMixer;
    }

    public bool GetIsMixerDone()
    {
        return MixerDone;
    }

    private void Update()
    {
        if (IsMixing)
        {
            mixingtimer -= Time.deltaTime;
            if (mixingtimer > 0)
                Mixing();

            else if (mixingtimer <= 0)
            {
                ResetMixer();
                DoneMixing();
                IsMixing = false;
                MixerDone = true;
            }
        }
    }
}
