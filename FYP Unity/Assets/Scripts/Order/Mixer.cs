using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mixer : MonoBehaviour
{
    public enum MixerType
    {
        REFINER,
        COOKER,
    }

    [SerializeField] MixerType mixerType;
    [SerializeField] GameObject mixingBG;
    [SerializeField] GameObject ingredient1;
    [SerializeField] GameObject ingredient2;
    [SerializeField] GameObject cookingBG;
    [SerializeField] Image timer;
    [SerializeField] GameObject doneBG;
    [SerializeField] int MixingTime;
    float mixingtimer;
    bool IsMixing;
    GameObject RecipeResultObject = null;
    bool CanPutIntoMixer;
    bool MixerDone;
    bool QTEDone;

    // to check stuff
    bool QTEActive = true;
    bool MixerActive = true;

    // List that contains the ingredients inputted into the mixer
    List<GameObject> mixercontent = new List<GameObject>();

    private void Start()
    {
        ResetMixer();
        CanPutIntoMixer = true;
        MixerDone = false;
        QTEDone = false;
    }

    public void InteractWithMixer()
    {
        if (!MixerActive)
            return;

        // If u can toss ingredients into the mixer, allow player to toss it inside
        if (CanPutIntoMixer)
        {
            AddIntoMixer();
        }

        // check to see if the mixer is done, if it is done, grab it
        if (MixerDone)
        {
            GetDishFromMixer();
        }
    }

    void AddIntoMixer()
    {
        InventoryImageControl inventory = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InventoryImageControl>();

        // if there is already 2 ingredients inside the mixer, do not allow player to put anymore ingredient
        // instead, mix them the next time they interact with it
        if (mixercontent.Count == 2)
            StartMixing();

        // if u can put, put it in
        else
        {
            // check to see what player has according to ther type of mixer
            switch (mixerType)
            {
                case MixerType.REFINER:
                    // make sure that the player is holding an ingredient and it isnt a mush
                    if (inventory.GetSelectedFoodID(FoodManager.FoodType.INGREDIENT) != -1
                        && FoodManager.instance.GetItemID(inventory.GetSelectedGameObject()) != (int)ItemManager.Items.MUSH)
                    {
                        mixercontent.Add(inventory.GetSelectedGameObject());
                        inventory.RemoveSelected();
                        RenderMixingMenu();
                    }
                    break;
                case MixerType.COOKER:
                    // make sure that the player is holding an ingredient
                    if (inventory.GetSelectedFoodID(FoodManager.FoodType.REFINED_INGREDIENT) != -1
                        && FoodManager.instance.GetItemID(inventory.GetSelectedGameObject()) != (int)RefinedItemManager.RItems.MUSHY)
                    {
                        mixercontent.Add(inventory.GetSelectedGameObject());
                        inventory.RemoveSelected();
                        RenderMixingMenu();
                    }
                    break;
            }
        }
    }

    public void StartMixing()
    {
        if (mixercontent[0] != null)
            Destroy(mixercontent[0]);
        if (mixercontent[1] != null)
            Destroy(mixercontent[1]);

        RecipeResultObject = CheckWhatIsOutput();
        // if player has a buff on itself, increase star value of dish by 1
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().buffactive())
        {
            RecipeResultObject.GetComponent<Food>().SetAmountOfStars(RecipeResultObject.GetComponent<Food>().GetAmtOfStars() + 1);
        }

        //InventoryImageControl inventory = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InventoryImageControl>();
        //GameObject temp = inventory.GetSelectedGameObject();
        Debug.Log("Dish result: " + RecipeResultObject);
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
            ingredient1.GetComponent<Image>().sprite = FoodManager.instance.GetImage(mixercontent[0]);
        }

        if (mixercontent.Count >= 2)
        {
            ingredient2.SetActive(true);
            ingredient2.GetComponent<Image>().sprite = FoodManager.instance.GetImage(mixercontent[1]);
        }
    }

    void Mixing()
    {
        timer.fillAmount = mixingtimer / MixingTime;
    }

    void DoneMixing()
    {
        mixingBG.SetActive(false);
        ingredient1.SetActive(false);
        ingredient2.SetActive(false);
        cookingBG.SetActive(false);
        doneBG.SetActive(true);
        timer.fillAmount = 1;
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

    public void GetDishFromMixer()
    {
        InventoryImageControl inv = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InventoryImageControl>();

        GameObject tempPlayerRef = GameObject.FindGameObjectWithTag("Inventory");

        // Check to see what type of mixer is it, if it is refiner, just give the ingredient, if it is cooker, go qte
        bool Successful = false;
        switch (mixerType)
        {
            case MixerType.REFINER:
                // Check to see player inventory is not full
                if (!Inventory.instance.InventoryFull)
                {
                    inv.AddItem(RecipeResultObject);
                    Successful = true;
                }

                break;
            case MixerType.COOKER:
                // Check to see player inventory is not full
                if (!Inventory.instance.InventoryFull)
                {
                    if (!QTEDone && QTEActive && FoodManager.instance.GetItemID(RecipeResultObject) != 0)
                    {
                        QTE.instance.StartQTE(gameObject, RecipeResultObject);
                        QTEDone = true;
                    }

                    else
                    {
                        inv.AddItem(RecipeResultObject);
                        Successful = true;
                        QTEDone = false;
                    }
                }
                break;
        }

        if (Successful)
        {
            // reset it
            RecipeResultObject.transform.SetParent(tempPlayerRef.transform);
            RecipeResultObject = null;
            ResetMixerEntirely();
        }
    }

    GameObject CheckWhatIsOutput()
    {
        GameObject temp = Recipes.instance.GetRecipeResult(mixercontent[0], mixercontent[1], gameObject);

        return temp;
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

    public MixerType GetMixerType()
    {
        return mixerType;
    }

    public void SetQTEActive(bool QTEtoBeActiveOrNot)
    {
        QTEActive = QTEtoBeActiveOrNot;
    }

    public void SetIsActive(bool isActive)
    {
        MixerActive = isActive;
    }
    public void AdjustMixingTime(int mixingTime)
    {
        MixingTime = mixingTime;
    }
    public int GetMixingTime()
    {
        return MixingTime;
    }
}