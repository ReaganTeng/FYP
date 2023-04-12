using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public PlayerProgress playerProgress;

    //WON'T BE USED FOR NOW
    [SerializeField] float PlayerHealth;
    //

    float PlayerAttack;


    [SerializeField] int numberConsecutiveHits;
    [SerializeField] int ConsecutiveHit_Stage1;
    [SerializeField] int ConsecutiveHit_Stage2;
    [SerializeField] int ConsecutiveHit_Stage3;
    [SerializeField] int ConsecutiveHit_Stage4;
    [SerializeField] int ConsecutiveHit_Stage5;

    [SerializeField] Slider fervorBar;
    [SerializeField] float fervorLevel;
    float fervor2Add;
    float fervorMaxLevel;
    [SerializeField] float combo_timer;

    //If fervorlevel >= 70, this is true, else false
    bool buff_active;

    [SerializeField] TextMeshProUGUI combo_timer_text;

   

    public void Awake()
    {
        PlayerAttack = 10.0f;
        

        fervorMaxLevel = 100;
        fervor2Add = 0;
        buff_active = false;
        fervorBar.maxValue = fervorMaxLevel;
        fervorLevel = 0;
        numberConsecutiveHits = 0;
    }

   
    

    public void Start()
    {
        PlayerAttack = 10.0f;
        

        //PlayerHealth = playerProgress.PlayerMaxHealth;

        fervorMaxLevel = 100;
        fervor2Add = 0;
        buff_active = false;
        fervorLevel = 0;
        numberConsecutiveHits = 0;
    }

    public void setAttack(float atk)
    {
        PlayerAttack = atk;
    }

    public float getAttack(float additionalAtk = 0)
    {
        return PlayerAttack + additionalAtk;
    }

    public void ChangeHealth(float Healthchange)
    {
        PlayerHealth += Healthchange;
        Debug.Log("Player Health: " + PlayerHealth);

        if (PlayerHealth <= 0)
        {
            Debug.Log("Imagine dying to ingredients!!");
        }
    }

    public void ChangeFervor(float Fervorchange)
    {

        if (fervorLevel > 0)
        {
            fervorLevel += Fervorchange;

            if (fervorLevel < 0)
            {
                fervorLevel = 0;
            }
        }

        Debug.Log("Fervor: " + fervorLevel);
    }


    public void Update()
    {

        if(GetComponentInChildren<Animator>().GetBool("Hurt") == true)
        {
            Debug.Log("PLAYER HURT");
        }

        combo_timer_text.SetText(((int)combo_timer).ToString());
        fervorBar.value = fervorLevel;
        if (fervorLevel > 0)
        {
            fervorLevel -= 1.0f * Time.deltaTime;
        }

        if (combo_timer <= 0
            && !Input.GetMouseButtonDown(0))
        {
            ResetConsecutiveHit();
            numberConsecutiveHits = 0;
        }
        else
        {
            combo_timer -= 1 * Time.deltaTime;
        }





        if (fervorLevel >= fervorMaxLevel - 30)
        {
            buff_active = true;
            //Debug.Log("BUFF IS ACTIVE");
        }
        else
        {
            buff_active = false;
        }



        //SCALE EFFECT FOR TEXT
        //GetComponent<Transform>().localScale += new Vector3(1 * Time.deltaTime, 1 * Time.deltaTime, 0);
        /*if (obtainTimer > 0)
        {
            if (obtainScale < 1)
            {
                obtainScale += 10 * Time.deltaTime;
            }

            obtainText.transform.localScale = new Vector3(obtainScale, obtainScale, 1);

            obtainTimer -= Time.deltaTime;

            if (obtainTimer <= 0)
            {
                obtainText.SetText("");
            }
        }*/


        //TEMPORARY, FOR TESTING PURPOSES
        //if (Input.GetMouseButtonDown(0))
        //{
        //    for (int i = 0; i < 5; i++)
        //    {
        //        addConsecutiveHit();
        //        resetCombo_timer();
        //        Debug.Log("CONSECUTIVE HITS");
        //    }
        //}
        //

    }

    public void decidefervor2add(float consecutive_stage_min, float consecutive_stage_max, float fervor_2add)
    {
        if (numberConsecutiveHits >= consecutive_stage_min
            && numberConsecutiveHits < consecutive_stage_max)
        {
            fervor2Add = fervor_2add;
        }
    }


    

    public void decidecombotimer(float consecutive_stage_min, float consecutive_stage_max, float comb_timer)
    {
        if (numberConsecutiveHits >= consecutive_stage_min
            && numberConsecutiveHits < consecutive_stage_max)
        {

            combo_timer = comb_timer;
        }
    }



    public bool buffactive()
    {
        return buff_active;
    }

    public void resetCombo_timer()
    {
        //between 5 and 10
        decidecombotimer(ConsecutiveHit_Stage1,
            ConsecutiveHit_Stage2,
            15.9f);
        //between 10 and 15
        decidecombotimer(ConsecutiveHit_Stage2,
            ConsecutiveHit_Stage3,
            12.9f);
        //between 15 and 20
        decidecombotimer(ConsecutiveHit_Stage3,
            ConsecutiveHit_Stage4,
            10.9f);
        //between 20 and 25
        decidecombotimer(ConsecutiveHit_Stage4,
            ConsecutiveHit_Stage5,
            7.9f);
        //more than 25
        decidecombotimer(ConsecutiveHit_Stage5,
            ConsecutiveHit_Stage5 + 999,
            3.9f);
    }

    public void ResetConsecutiveHit()
    {
        //between 5 and 10
        decidefervor2add(ConsecutiveHit_Stage1,
            ConsecutiveHit_Stage2,
            10);
        //between 10 and 15
        decidefervor2add(ConsecutiveHit_Stage2,
            ConsecutiveHit_Stage3,
            20);
        //between 15 and 20
        decidefervor2add(ConsecutiveHit_Stage3,
            ConsecutiveHit_Stage4,
            40);
        //between 20 and 25
        decidefervor2add(ConsecutiveHit_Stage4,
            ConsecutiveHit_Stage5,
            75);
        //more than 25
        decidefervor2add(ConsecutiveHit_Stage5,
            ConsecutiveHit_Stage5 + 999,
            100);

        if (fervor2Add > fervorLevel)
        {
            fervorLevel = fervor2Add;
            fervor2Add = 0;
        }

        numberConsecutiveHits = 0;
        combo_timer = 0;

    }
    public float getfervor2add()
    {
        return fervor2Add;
    }



    public void addConsecutiveHit()
    {
        numberConsecutiveHits += 1;
    }
    public float GetPlayerAttack()
    {
        return PlayerAttack;
    }


    private void OnTriggerStay(Collider other)
    {
        /*if (other.CompareTag("Enemy"))
        {
            Debug.Log("COLLIDED WITH ENEMY");
        }*/
    }
}
