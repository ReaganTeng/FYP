using System.Collections;
using System.Collections.Generic;

using System.Linq;

using UnityEngine;

using UnityEngine.UI;


public class CraftimgManager : MonoBehaviour
{
    private Item_Reagan currentItem;
    public Image customCursor;

    public List<Item_Reagan> itemList;


    public string[] recipes;

    public List<string[]> recipeList;
    public Item_Reagan[] recipeResults;

    public Slots resultslot;

    public Slots[] craftingSlots;

    public Slots plateslot;

    string[] tempList;


    void Start()
    {

       recipeList = new List<string[]>()
        {
            new string[] { "2", "3" },
            new string[]{  "1", "null" },
            new string[] { "1", "null" },
                        new string[] { "4", "3" }

        };

        tempList = new string[2];

    }



    void Update()
    {
        if (currentItem != null)
        {
            Debug.Log("CURRENT ITEM " + currentItem);
        }

            if (Input.GetMouseButtonUp(0)
            )
        {
            if(currentItem != null)
            {
                /*customCursor.gameObject.SetActive(false);
                Slots nearestSlot = null;
                float shortestDistance = float.MaxValue;

                foreach(Slots slot in craftingSlots)
                {
                    float dist = Vector2.Distance(Input.mousePosition, slot.transform.position);

                    if(dist < shortestDistance)
                    {
                        shortestDistance = dist;
                        nearestSlot = slot;
                    }
                }
                nearestSlot.gameObject.SetActive(true);
                nearestSlot.GetComponent<Image>().sprite = currentItem.GetComponent<Image>().sprite;
                nearestSlot.item = currentItem;
                itemList[nearestSlot.index] = currentItem;*/


                customCursor.gameObject.SetActive(false);
                float dist = Vector2.Distance(Input.mousePosition, plateslot.transform.position);
                if (dist < 100)
                {
                    for (int i = 0; i < craftingSlots.Length;)
                    {
                        //if slot is empty
                        if (craftingSlots[i].item == null)
                        {
                            //Debug.Log("DIST: " + dist);

                            craftingSlots[i].gameObject.SetActive(true);
                            craftingSlots[i].GetComponent<Image>().sprite = currentItem.GetComponent<Image>().sprite;
                            craftingSlots[i].item = currentItem;
                            itemList[i] = currentItem;



                            ///SORTING FOR CRAFTING
                            //check the list of recipies
                            for (int x = 0; x < recipeList.Count;)
                            {
                                /*Console.WriteLine("Check for ingredient: " 
                                + recipeList.ElementAt(x)[z]);*/
                                int z;
                                int number_of_correct_ingredients = 0;

                                //check the ingredients inside the recipies
                                for (z = 0; z < recipeList[x].Length; z++)
                                {
                                    //check all the ingredients inputted in the input
                                    for (int w = 0; w < craftingSlots.Length; w++)
                                    {
                                        if (craftingSlots[w].item == null)
                                        {
                                            tempList[z] = "null";
                                        }

                                        else
                                        {
                                            //check if any of the ingredients inside the recipes
                                            //correspond to the same id as the input
                                            if (craftingSlots[w].item.itemName ==
                                            (recipeList[x])[z])
                                            {
                                                tempList[z] = craftingSlots[w].item.itemName;
                                                number_of_correct_ingredients += 1;
                                            }
                                        }
                                    }
                                }
                                if (number_of_correct_ingredients >= 2)
                                {
                                    //break from x loop
                                    break;
                                }
                                else
                                {
                                    //MOVE TO NEXT RECIPE LIST, IF NOT ALL OF THE 
                                    //INGREDIENTS MATCHED
                                    x++;
                                }
                            }
                            ///




                            break;
                        }
                        else
                        {
                            i++;
                        }
                    }
                    
                }

                


                currentItem = null;
            }
            CheckCreatedRecipes();
        }
    }

    public void CheckCreatedRecipes()
    {
        resultslot.gameObject.SetActive(false);
        resultslot.item = null;

        string currentRecipeString = "";

        //foreach(Item item in itemList)
        //{
        //    if(item != null)
        //    {
        //        currentRecipeString += item.itemName;
        //    }
        //    else
        //    {
        //        currentRecipeString += "null";
        //    }
        //}




        //tempList[z]
        //foreach (Item item in itemList)
        //{
        //    if (item != null)
        //    {
                currentRecipeString += tempList[0] + tempList[1];
        //    }
        //    else
        //    {
        //        currentRecipeString += "null";
        //    }
        //}

        for (int i = 0; i < recipes.Length; i++)
        {
            if(recipes[i] == currentRecipeString)
            {
                resultslot.gameObject.SetActive(true);
                resultslot.GetComponent<Image>().sprite
                    = recipeResults[i].GetComponent<Image>().sprite;
                resultslot.item = recipeResults[i];
            }
        }




       




    }


    public void OnClickSlot(Slots slot)
    {
        slot.item = null;
        itemList[slot.index] = null;
        slot.gameObject.SetActive(false);
        CheckCreatedRecipes();
    }

    public void OnClickResultsSlot(Slots slot)
    {
        slot.item = null;
        itemList[slot.index] = null;
        slot.gameObject.SetActive(false);

        for (int i = 0; i < craftingSlots.Length; i++)
        {
            craftingSlots[i].item = null;
            craftingSlots[i].gameObject.SetActive(false);
        }




        






    }

    public void OnMouseDownItem(Item_Reagan item)
    {
        if(currentItem == null)
        {
            currentItem = item;
            //the cursor will carry the item clicked
            customCursor.gameObject.SetActive(true);
            customCursor.sprite = currentItem.GetComponent<Image>().sprite;
        }
    }
}
