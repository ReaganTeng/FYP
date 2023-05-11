using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderPanel : MonoBehaviour
{
    [SerializeField] Image ResultDish;
    [SerializeField] float OrderTimer;
    [SerializeField] Slider Timer;
    Recipes.recipes OrderRecipe;
    int score;
    float InitialTimer;

    public void SetOrder(Sprite ingre1, Sprite ingre2, Sprite dishresult, Recipes.recipes therecipe)
    {
        ResultDish.sprite = dishresult;
        OrderRecipe = therecipe;
        score = therecipe.Result.GetComponent<Dish>().GetScore();
    }

    public void SetOrder(Sprite ingre1, Sprite ingre2, Sprite dishresult, Recipes.recipes therecipe, float waitingtime)
    {
        ResultDish.sprite = dishresult;
        OrderRecipe = therecipe;
        OrderTimer = waitingtime;
        score = therecipe.Result.GetComponent<Dish>().GetScore();
    }

    public int GetDishID()
    {
        return FoodManager.instance.GetItemID(OrderRecipe.Result);
    }

    public int GetTimeTillExpire()
    {
        return (int)OrderTimer;
    }

    public int GetScore()
    {
        return score;
    }

    private void Start()
    {
        InitialTimer = OrderTimer;
        Timer.value = Timer.maxValue;
    }

    private void Update()
    {
        OrderTimer -= Time.deltaTime;
        Timer.value -= Time.deltaTime / InitialTimer;

        // Destroy itself if timer hits 0;
        if (OrderTimer <= 0.0f)
        {
            Destroy(gameObject);
            IncurPenalty();
        }
    }

    void IncurPenalty()
    {
        OrderSystem orderSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<OrderSystem>();
        orderSystem.PlayerIncurPenalty();
    }

    public void Served()
    {
        Destroy(gameObject);
    }

    public Recipes.recipes GetOrderRecipe()
    {
        return OrderRecipe;
    }
}
