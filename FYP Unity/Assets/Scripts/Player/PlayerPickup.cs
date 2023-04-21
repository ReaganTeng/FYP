using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] Inventory PlayerInventory;
    List<GameObject> InteractableInRangeList = new List<GameObject>();
    InventoryImageControl ic;
    public bool DisableControls;

    private void Start()
    {
        ic = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InventoryImageControl>();
        DisableControls = false;
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
                            if (InteractableInRangeList[i].CompareTag("Ingredient"))
                                InteractableInRangeList[i].GetComponent<ItemGlow>().TurnOnHighlight();
                        }

                        // if its not the closest, dont make it glow
                        else
                        {
                            if (InteractableInRangeList[i].CompareTag("Ingredient"))
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
                    if (pickupobject.CompareTag("Ingredient") && !PlayerInventory.InventoryFull)
                    {
                        InteractableInRangeList.Remove(pickupobject);
                        // Add the item into ur inventory
                        ic.AddItem(pickupobject.GetComponentInParent<Food>().gameObject);
                        pickupobject.SetActive(false);
                    }

                    else if (pickupobject.CompareTag("Mixer"))
                    {
                        pickupobject.GetComponent<Mixer>().InteractWithMixer();
                    }

                    else if (pickupobject.CompareTag("Serve"))
                    {
                        pickupobject.GetComponent<Serving>().Serve();
                    }

                    else if (pickupobject.CompareTag("Barrel"))
                    {
                        pickupobject.GetComponent<IngredientBarrel>().GetIngredientFromBarrel();
                    }

                    else if (pickupobject.CompareTag("DustBin"))
                    {
                        pickupobject.GetComponent<DustBin>().RemoveItem();
                    }
                }
            }

            // switch between selected items
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ic.ChangeSelectedHotBar(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ic.ChangeSelectedHotBar(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ic.ChangeSelectedHotBar(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ic.ChangeSelectedHotBar(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                ic.ChangeSelectedHotBar(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                ic.ChangeSelectedHotBar(5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                ic.ChangeSelectedHotBar(6);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                ic.ChangeSelectedHotBar(7);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                ic.ChangeSelectedHotBar(8);
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                ic.ChangeSelectedHotBar(9);
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
}
