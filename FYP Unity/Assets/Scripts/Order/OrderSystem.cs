using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class OrderSystem : MonoBehaviour
{
    [SerializeField] PlayerProgress pp;
    [SerializeField] LevelManager lm;
    [SerializeField] InventoryImageControl inventory;
    [SerializeField] GameObject OrderPrefab;
    float TimeBeforeFirstOrder;
    float OrderIntervalTiming;
    int MaxOrderAtOnce;
    [SerializeField] EndOfDay eod;
    [SerializeField] OrderRecipeDisplay ord;
    float beforefirstordertimer;
    float orderintervaltimer;
    bool StartOrdering;
    bool IncurPenalty;
    bool icDelay;
    bool StopComingOrders;
    private int SuccessfulOrders;
    private int FailedOrders;
    private int ObtainedStars;
    [SerializeField] float DefaultWaitingTime = 120;
    private float WaitingTime;

    [SerializeField] float NoStarDishMultipler = 0.25f;
    [SerializeField] float OneStarDishMultipler = 0.5f;
    [SerializeField] float TwoStarDishMultipler = 0.75f;
    [SerializeField] float ThreeStarDishMultipler = 1.0f;
    [SerializeField] float FourStarDishMultipler = 1.25f;
    [SerializeField] float FiveStarDishMultipler = 2.0f;

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
        // put lm in here
        for (int i = 0; i < lm.levelInfo.Count; i++)
        {
            if (i == lm.DaySelected - 1)
            {
                TimeBeforeFirstOrder = lm.levelInfo[i].TimeBeforeFirstOrder;
                OrderIntervalTiming = lm.levelInfo[i].IntervalBetweenOrders;
                MaxOrderAtOnce = lm.levelInfo[i].MaxOrders;
                break;
            }
        }

        beforefirstordertimer = TimeBeforeFirstOrder;
        orderintervaltimer = 0;
        StartOrdering = false;
        IncurPenalty = false;
        icDelay = false;
        StopComingOrders = false;
        SuccessfulOrders = 0;
        FailedOrders = 0;
        DefaultWaitingTime += pp.GetLongerOrderTime();
        SetWaitingTime();
        FiveStarDishMultipler += pp.GetPerfectDishBoost();
    }

    private void Update()
    {
        // during game startup, do not run the update
        if (FreezeGame.instance.startUpfreeze || Tutorial.instance.InTutorial)
            return;

        if (!StartOrdering)
        {
            beforefirstordertimer -= Time.deltaTime;

            if (beforefirstordertimer <= 0)
            {
                StartOrdering = true;
            }
        }

        else if (orderList.Count < MaxOrderAtOnce && !StopComingOrders)
        {
            orderintervaltimer -= Time.deltaTime;

            if (orderintervaltimer <= 0)
            {
                // Create an order list
                CreateAnOrder();
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

    void AssignRecipe(GameObject order, int index = -1)
    {
        OrderPanel orderPanel = order.GetComponent<OrderPanel>();
        Recipes.recipes theorder = OrderManager.instance.GetSelectedOrderFromCurrentDay(index);
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
            int TimeToExpire = 999999999;
            int index = -1;
            // Check to see which items is gonna expire, remove that one first
            for (int i = 0; i < orderList.Count; i++)
            {
                int TimeTillExpire = orderList[i].GetComponent<OrderPanel>().GetTimeTillExpire();
                // if it is closer to 0 and it is the right order, save the index
                if (inventory.GetSelectedFoodID(FoodManager.FoodType.DISH) == orderList[i].GetComponent<OrderPanel>().GetDishID())
                {
                    if (TimeToExpire > TimeTillExpire)
                    {
                        TimeToExpire = TimeTillExpire;
                        index = i;
                    }
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
                        Score *= NoStarDishMultipler;
                        break;
                    case 1:
                        Score *= OneStarDishMultipler;
                        break;
                    case 2:
                        Score *= TwoStarDishMultipler;
                        break;
                    case 3:
                        Score *= ThreeStarDishMultipler;
                        break;
                    case 4:
                        Score *= FourStarDishMultipler;
                        break;
                    case 5:
                        Score *= FiveStarDishMultipler;
                        // Activate frenzy
                        FrenzyMode.instance.ActivateFrenzyMode();

                        if (FrenzyMode.instance.GetCanGoFrenzy())
                        {
                            Score *= (1 + FrenzyMode.instance.GetFrenzyStack());
                        }
                        break;
                }

                // Store the stars obtained
                ObtainedStars += starsobtained;

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
                // Count towards a successful serve
                SuccessfulOrders++;
                // Sort the list and update the order UI accordingly
                CheckOrderList();
                // Check to see if can end day
                CheckIfCanEndDay();
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
        // Count towards a failed order
        FailedOrders++;
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
                CheckIfCanEndDay();
                break;
            }
        }
        ord.SetToDisplay(orderList);
    }

    public void StopOrders()
    {
        StopComingOrders = true;
        CheckIfCanEndDay();
    }

    void CheckIfCanEndDay()
    {
        if (StopComingOrders & orderList.Count == 0)
        {
            if (lm.DaySelected != 8)
                eod.EndDay();
            else
            {
                // check to see if player meet the requirement
                if (eod.GetScore() >= eod.GetLevelReference().CReq)
                {
                    // 10 is if u meet req
                    lm.DaySelected = 10;
                }
                else
                {
                    // 9 is if u meet req
                    lm.DaySelected = 9;
                }
                SceneManager.LoadScene("VNScene");
            }    
        }
    }

    public int GetSuccessfulOrders()
    {
        return SuccessfulOrders;
    }

    public int GetFailedOrders()
    {
        return FailedOrders;
    }

    public float GetStarRating()
    {
        int TotalOrders = SuccessfulOrders + FailedOrders;
        if (TotalOrders != 0)
            return ObtainedStars / TotalOrders;
        else
            return 5;
    }

    public void CreateAnOrder(int indexIfAny = -1)
    {
        // Create an order list
        GameObject temp = Instantiate(OrderPrefab);
        AssignRecipe(temp, indexIfAny);
        temp.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        orderList.Add(temp);
        CheckOrderList();
        ord.SetToDisplay(orderList);
        orderintervaltimer = OrderIntervalTiming;
    }

    public void SetWaitingTime(int i = 0)
    {
        if (i == 0)
        {
            WaitingTime = DefaultWaitingTime;
        }
        else
        {
            WaitingTime = i;
        }
    }

    public int GetOrderCount()
    {
        return orderList.Count;
    }
}
