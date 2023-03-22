using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] Inventory PlayerInventory;
    List<GameObject> IngredientInRangeList = new List<GameObject>();
    InventoryImageControl ic;
    Mixer mixer;
    Serving sc;
    bool NearMixer;
    bool NearServingCounter;

    private void Start()
    {
        ic = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InventoryImageControl>();
        mixer = GameObject.FindGameObjectWithTag("Mixer").GetComponent<Mixer>();
        sc = GameObject.FindGameObjectWithTag("Serve").GetComponent<Serving>();
        NearMixer = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ingredient"))
        {
            if (!IngredientInRangeList.Contains(other.gameObject))
            {
                IngredientInRangeList.Add(other.gameObject);
            }
        }

        else if (other.CompareTag("Mixer"))
            NearMixer = true;

        else if (other.CompareTag("Serve"))
            NearServingCounter = true;
            
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ingredient"))
        {
            if (IngredientInRangeList.Contains(other.gameObject))
            {
                IngredientInRangeList.Remove(other.gameObject);
                other.GetComponent<ItemGlow>().TurnOffHighlight();
            }
        }

        else if (other.CompareTag("Mixer"))
            NearMixer = false;

        else if (other.CompareTag("Serve"))
            NearServingCounter = false;
    }

    private void Update()
    {
        bool Ecooldown = false;
        // If Inventory is not full, then do item highlight to indicate that items can be picked
        if (!PlayerInventory.InventoryFull)
        {
            // if there is only one object, highlight that
            if (IngredientInRangeList.Count == 1)
            {
                // switch the item color or make it glow maybe?
                IngredientInRangeList[0].GetComponent<ItemGlow>().TurnOnHighlight();
            }

            // if there is more than one object, check the closest distance between those and take the nearest one
            else if (IngredientInRangeList.Count > 1)
            {
                GameObject nearestGameObject = FindNearestGameObject();

                for (int i = 0; i < IngredientInRangeList.Count; i++)
                {
                    if (nearestGameObject == IngredientInRangeList[i].gameObject)
                    {
                        // make the item glow
                        IngredientInRangeList[i].GetComponent<ItemGlow>().TurnOnHighlight();
                    }

                    // if its not the closest, dont make it glow
                    else
                    {
                        IngredientInRangeList[i].GetComponent<ItemGlow>().TurnOffHighlight();
                    }
                }
            }

            // Do something wif the gameobject that has the shortest distance
            // Check to see if player wants to pick up an item
            if (Input.GetKeyDown(KeyCode.E) && IngredientInRangeList.Count > 0)
            {
                Ecooldown = true;
                GameObject pickupobject;
                if (IngredientInRangeList.Count == 1)
                {
                    pickupobject = IngredientInRangeList[0];
                    IngredientInRangeList.Remove(pickupobject);
                    Destroy(pickupobject);
                    // Add the item into ur inventory
                    PlayerInventory.AddToInventory(pickupobject);
                    ic.AddItem(pickupobject.GetComponentInParent<Item>());
                }
                else
                {
                    pickupobject = FindNearestGameObject();
                    IngredientInRangeList.Remove(pickupobject);
                    Destroy(pickupobject);
                    // Add the item into ur inventory
                    PlayerInventory.AddToInventory(pickupobject);
                    ic.AddItem(pickupobject.GetComponentInParent<Item>());
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
        // Remove selected
        if (Input.GetKeyDown(KeyCode.L))
        {
            ic.RemoveSelected();
        }

        // If you interact wif ennvironment objects and there is no ingredients around the player
        if (Input.GetKeyDown(KeyCode.E) && !Ecooldown && IngredientInRangeList.Count == 0)
        {
            if (NearMixer)
            {
                if (mixer.GetCanPutIntoMixer())
                {
                    // if can add to mixer, remove the selected object
                    if (mixer.AddIntoMixer())
                    {
                        ic.RemoveSelected();
                    }

                    else if (mixer.GetMixerAmount() == 2)
                    {
                        mixer.StartMixing();
                    }
                }

                // if mixer is done, and player inventory is not full
                else if (mixer.GetIsMixerDone())
                {
                    if (!PlayerInventory.InventoryFull)
                    {
                        Dish thedish = mixer.GetDishFromMixer();
                        PlayerInventory.AddToInventory(thedish.gameObject);
                        ic.AddItem(thedish);
                    }
                }
            }

            else if (NearServingCounter)
            {
                sc.Serve();
            }
        }

    }

    GameObject FindNearestGameObject()
    {
        // If there is a nearest gameobject, find it
        if (IngredientInRangeList.Count > 0)
        {
            float shortestDistance = 100;
            GameObject nearestobject = null;
            for (int i = 0; i < IngredientInRangeList.Count; i++)
            {
                Vector3 Ingredientpos = new Vector3(IngredientInRangeList[i].transform.position.x, 0, IngredientInRangeList[i].transform.position.z);
                Vector3 Playerpos = new Vector3(transform.position.x, 0, transform.position.z);
                float Distance = Vector3.Distance(Playerpos, Ingredientpos);

                if (Distance < shortestDistance)
                {
                    nearestobject = IngredientInRangeList[i].gameObject;
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
