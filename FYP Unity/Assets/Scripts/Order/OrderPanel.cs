using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderPanel : MonoBehaviour
{
    [SerializeField] Image Ingredient1;
    [SerializeField] Image Ingredient2;
    [SerializeField] Image ResultDish;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float OrderTimer;
    [SerializeField] int score;
    [SerializeField] Recipes.recipes OrderRecipe;

    public void SetOrder(Sprite ingre1, Sprite ingre2, Sprite dishresult, Recipes.recipes therecipe)
    {
        Ingredient1.sprite = ingre1;
        Ingredient2.sprite = ingre2;
        ResultDish.sprite = dishresult;
        OrderRecipe = therecipe;
    }

    public void SetOrder(Sprite ingre1, Sprite ingre2, Sprite dishresult, Recipes.recipes therecipe, float waitingtime)
    {
        Ingredient1.sprite = ingre1;
        Ingredient2.sprite = ingre2;
        ResultDish.sprite = dishresult;
        OrderRecipe = therecipe;
        OrderTimer = waitingtime;
    }

    public int GetDishID()
    {
        return OrderRecipe.Result.GetItemID();
    }

    public int GetTimeTillExpire()
    {
        return (int)OrderTimer;
    }

    public int GetScore()
    {
        return score;
    }

    private void Update()
    {
        OrderTimer -= Time.deltaTime;
        text.text = "Time Left: " + (int)OrderTimer;

        // Destroy itself if timer hits 0;
        if (OrderTimer <= 0.0f)
        {
            Destroy(gameObject);
            IncurPenalty();
            //maybe call the list to delete that part?
        }
    }

    void IncurPenalty()
    {
        OrderSystem orderSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<OrderSystem>();
    }

    public void Served()
    {
        Destroy(gameObject);
    }
}
