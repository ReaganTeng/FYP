using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] Inventory PlayerInventory;
    List<GameObject> InteractableInRangeList = new List<GameObject>();
    InventoryImageControl ic;
    public bool DisableControls = false;
    public bool CannotInteractWithDustbin = false;
    public bool CannotPickUpItems = false;
    int selectedScroll = 1;




    [SerializeField] PlayerAttack playerAttackScript;
    [SerializeField] PlayerProgress pp;

    private void Start()
    {
        ic = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InventoryImageControl>();
    }

    // add the interactable objects into the list
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ingredient") || other.CompareTag("Mixer") || other.CompareTag("Serve") || other.CompareTag("Barrel") || other.CompareTag("DustBin"))
        {
            if (!InteractableInRangeList.Contains(other.gameObject))
            {
                InteractableInRangeList.Add(other.gameObject);
                
            }
        }
           
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ingredient") || other.CompareTag("Mixer") || other.CompareTag("Serve") || other.CompareTag("Barrel") || other.CompareTag("DustBin"))
        {
            if (InteractableInRangeList.Contains(other.gameObject))
            {
                InteractableInRangeList.Remove(other.gameObject);
                if (other.CompareTag("Ingredient"))
                    other.GetComponent<ItemGlow>().TurnOffHighlight();
            }
        }
    }

    private void Update()
    {
        ClearList();
        if (!DisableControls)
        {
            // If Inventory is not full, then do item highlight to indicate that items can be picked
            if (!PlayerInventory.InventoryFull)
            {
                // if there is only one object, highlight that
                if (InteractableInRangeList.Count == 1)
                {
                    // switch the item color or make it glow maybe?
                    if (InteractableInRangeList[0].CompareTag("Ingredient"))
                        InteractableInRangeList[0].GetComponent<ItemGlow>().TurnOnHighlight();
                }

                // if there is more than one object, check the closest distance between those and take the nearest one
                else if (InteractableInRangeList.Count > 1)
                {
                    GameObject nearestGameObject = FindNearestGameObject();

                    for (int i = 0; i < InteractableInRangeList.Count; i++)
                    {
                        if (nearestGameObject == InteractableInRangeList[i].gameObject)
                        {
                            // make the item glow
                            if (InteractableInRangeList[i].CompareTag("Ingredient") && !CannotPickUpItems)
                                InteractableInRangeList[i].GetComponent<ItemGlow>().TurnOnHighlight();
                        }

                        // if its not the closest, dont make it glow
                        else
                        {
                            if (InteractableInRangeList[i].CompareTag("Ingredient") && !CannotPickUpItems)
                                InteractableInRangeList[i].GetComponent<ItemGlow>().TurnOffHighlight();
                        }
                    }
                }
            }

            // Do something wif the gameobject that has the shortest distance
            // Check to see if player wants to pick up an item
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameObject pickupobject;
                pickupobject = FindNearestGameObject();

                if (pickupobject != null)
                {
                    
                    if (pickupobject.CompareTag("Ingredient") && !PlayerInventory.InventoryFull && !CannotPickUpItems)
                    {
                        InteractableInRangeList.Remove(pickupobject);
                        // Add the item into ur inventory
                        ic.AddItem(pickupobject.GetComponentInParent<Food>().gameObject);
                        pickupobject.GetComponent<ItemGlow>().GetParent().transform.SetParent(GameObject.FindGameObjectWithTag("Inventory").transform);

                        //ADD BACK THE NUMBER OF CHARGES WHEN INGREDIENT IS PERFECT, AND RUSH OF PERFECTION SHOP UPGRADE
                        //IS USED
                        if(pickupobject.GetComponentInParent<Food>().GetIsPerfect()
                            && pp.return_rush_of_perfection() > 0)
                        {
                            //Debug.Log("CONGRATS YOU REGAINED CHARGES");
                            playerAttackScript.addcharge(pp.return_rush_of_perfection());
                        }
                        //

                        GameSoundManager.PlaySound("PickUpItem");
                        pickupobject.SetActive(false);
                    }

                    else if (pickupobject.CompareTag("Mixer"))
                    {
                        pickupobject.GetComponent<Mixer>().InteractWithMixer();
                    }

                    else if (pickupobject.CompareTag("Serve"))
                    {
                        pickupobject.GetComponent<Serving>().Serve();

                        //SET BURST TIME BASE ON THE DINNER RUSH SHOP UPGRADE
                        if(pp.return_dinner_rush() > 0)
                        {
                            //Debug.Log("BURST");
                            GetComponentInParent<PlayerStats>().setbursttime(pp.return_dinner_rush());
                        }
                        //

                        GameSoundManager.PlaySound("ServeDish");
                    }

                    else if (pickupobject.CompareTag("Barrel"))
                    {
                        pickupobject.GetComponent<IngredientBarrel>().GetIngredientFromBarrel();
                        //GameSoundManager.PlaySound("PickUpItem");
                    }

                    else if (pickupobject.CompareTag("DustBin") && !CannotInteractWithDustbin)
                    {
                        pickupobject.GetComponent<DustBin>().RemoveItem();
                    }
                }
            }

            // switch between selected items
            if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
            {
                // if it exceed the max due to removal of item, set it to the correct value
                if (selectedScroll > Inventory.instance.GetList().Count)
                    selectedScroll = Inventory.instance.GetList().Count;

                //selectedScroll -= (int)Input.mouseScrollDelta.y;

                float scrollwhat = Input.GetAxisRaw("Mouse ScrollWheel");
                if (scrollwhat > 0)
                    selectedScroll -= 1;
                else if (scrollwhat < 0)
                    selectedScroll += 1;

                // if the inventory is not empty
                if (Inventory.instance.GetList().Count > 0)
                {
                    // if scroll out of bound, below 1, warp to last one
                    if (selectedScroll <= 0)
                    {
                        selectedScroll = Inventory.instance.GetList().Count;
                    }
                    // if scroll out of bound, above max item amt, warp to the first one
                    if (selectedScroll > Inventory.instance.GetList().Count)
                    {
                        selectedScroll = 1;
                    }

                    ic.ChangeSelectedHotBar(selectedScroll - 1);
                }
                else
                {
                    selectedScroll = 1;
                }
            }
        }
    }

    GameObject FindNearestGameObject()
    {
        // If there is a nearest gameobject, find it
        if (InteractableInRangeList.Count > 0)
        {
            float shortestDistance = 100;
            GameObject nearestobject = null;
            for (int i = 0; i < InteractableInRangeList.Count; i++)
            {
                Vector3 Ingredientpos = new Vector3(InteractableInRangeList[i].transform.position.x, 0, InteractableInRangeList[i].transform.position.z);
                Vector3 Playerpos = new Vector3(transform.position.x, 0, transform.position.z);
                float Distance = Vector3.Distance(Playerpos, Ingredientpos);

                if (Distance < shortestDistance)
                {
                    nearestobject = InteractableInRangeList[i].gameObject;
                    shortestDistance = Distance;
                }
            }
            return nearestobject;
        }
        // if there isnt a gameobject, simply return null
        else
            return null;
    }

    void ClearList()
    {
        for (int i = 0; i < InteractableInRangeList.Count; i++)
        {
            if (InteractableInRangeList[i] == null)
            {
                InteractableInRangeList.RemoveAt(i);
                break;
            }
        }
    }
}