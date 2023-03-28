using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderSystem : MonoBehaviour
{
    [SerializeField] InventoryImageControl inventory;
    [SerializeField] Transform OrderZone;
    [SerializeField] GameObject OrderPrefab;
    [SerializeField] float TimeBeforeFirstOrder;
    [SerializeField] float OrderIntervalTiming;
    [SerializeField] int MaxOrderAtOnce;
    [SerializeField] EndOfDay eod;
    float beforefirstordertimer;
    float orderintervaltimer;
    bool StartOrdering;
    bool IncurPenalty;
    bool icDelay;

    List<GameObject> orderList = new List<GameObject>();

    public void ModifyTimeBeforeFirstOrder(float modifiedtiming)
    {
        TimeBeforeFirstOrder = modifiedtiming;
    }

    public void ModifyOrderIntervalTiming(float modifiedtiming)
    {
        OrderIntervalTiming = modifiedtiming;
    }

    public void ModifyMaximumOrderAtOnce(int maxorder)
    {
        MaxOrderAtOnce = maxorder;
    }

    private void Start()
    {
        beforefirstordertimer = TimeBeforeFirstOrder;
        orderintervaltimer = 0;
        StartOrdering = false;
        IncurPenalty = false;
        icDelay = false;
    }

    private void Update()
    {
        if (!StartOrdering)
        {
            beforefirstordertimer -= Time.deltaTime;

            if (beforefirstordertimer <= 0)
            {
                StartOrdering = true;
            }
        }

        else if (orderList.Count < MaxOrderAtOnce)
        {
            orderintervaltimer -= Time.deltaTime;

            if (orderintervaltimer <= 0)
            {
                // Create an order list
                GameObject temp = Instantiate(OrderPrefab);
                AssignRecipe(temp);
                temp.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                temp.transform.SetParent(OrderZone);
                orderList.Add(temp);
                orderintervaltimer = OrderIntervalTiming;
            }
        }

        if (IncurPenalty)
        {
            if (icDelay)
            {
                icDelay = false;
                IncurPenalty = false;
                CheckOrderList();
            }
            icDelay = true;
        }
    }

    void AssignRecipe(GameObject order)
    {
        OrderPanel orderPanel = order.GetComponent<OrderPanel>();
        float WaitingTime = OrderManager.instance.GetCurrentDayWaitingTime();
        Recipes.recipes theorder = OrderManager.instance.GetRandomOrderFromCurrentDay();
        if (WaitingTime == 0)
            orderPanel.SetOrder(FoodManager.instance.GetImage(theorder.ingredient1), FoodManager.instance.GetImage(theorder.ingredient2), FoodManager.instance.GetImage(theorder.Result), theorder);
        else
            orderPanel.SetOrder(FoodManager.instance.GetImage(theorder.ingredient1), FoodManager.instance.GetImage(theorder.ingredient2), FoodManager.instance.GetImage(theorder.Result), theorder, WaitingTime);
    }

    public void Serving()
    {
        // If the dish that they are holding can be served
        bool CanBeServed = false;

        for (int i = 0; i < orderList.Count; i++)
        {
            // check to see if the dish u holding can be served
            if (inventory.GetSelectedFoodID(FoodManager.FoodType.DISH) == orderList[i].GetComponent<OrderPanel>().GetDishID())
            {
                CanBeServed = true;
                break;
            }
        }

        if (CanBeServed)
        {
            int TimeToExpire = 999;
            int index = -1;
            // Check to see which items is gonna expire, remove that one first
            for (int i = 0; i < orderList.Count; i++)
            {
                int TimeTillExpire = orderList[i].GetComponent<OrderPanel>().GetTimeTillExpire();
                // if it is closer to 0, save the index
                if (TimeToExpire > TimeTillExpire)
                {
                    TimeToExpire = TimeTillExpire;
                    index = i;
                }
            }
            
            // just a safety measure
            if (index != -1)
            {
                OrderPanel TheOrder = orderList[index].GetComponent<OrderPanel>();

                // Get score base on stars obtained
                int starsobtained = inventory.GetSelectedStarAmount();
                float Score = TheOrder.GetScore();
                switch (starsobtained)
                {
                    case 0:
                        Score *= 0.25f;
                        break;
                    case 1:
                        Score *= 0.5f;
                        break;
                    case 2:
                        Score *= 0.75f;
                        break;
                    case 3:
                        Score *= 1.0f;
                        break;
                    case 4:
                        Score *= 1.25f;
                        break;
                    case 5:
                        Score *= 2.0f;
                        break;
                }

                // award score to player
                eod.ChangeScore((int)Score);
                // remove the order
                TheOrder.Served();
                // remove from the list
                orderList.RemoveAt(index);
                // Remove from scene first
                Destroy(inventory.GetSelectedGameObject());
                // Remove it from player inventory
                inventory.RemoveSelected();
            }
        }
        else
        {
            Debug.Log("Nani u serving? bakaaaaaaa!");
        }
    }

    public void PlayerIncurPenalty()
    {
        int penaltyby = GameObject.FindGameObjectWithTag("GameManager").GetComponent<OrderManager>().GetPenalty();
        IncurPenalty = true;
        eod.ChangeScore(-penaltyby);
        CheckOrderList();
    }

    // clear out any deleted order panel from the list
    void CheckOrderList()
    {
        // if that reference is null, it means that object no longer exist, and it shld remove that object from the list
        for (int i = 0; i < orderList.Count; i++)
        {
            if (orderList[i] == null)
            {
                orderList.Remove(orderList[i]);
                break;
            }
        }
    }
}