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

    int PlayerAttack;


    [SerializeField] int numberConsecutiveHits;
    [SerializeField] int ConsecutiveHit_Stage1;
    [SerializeField] int ConsecutiveHit_Stage2;
    [SerializeField] int ConsecutiveHit_Stage3;
    [SerializeField] int ConsecutiveHit_Stage4;
    [SerializeField] int ConsecutiveHit_Stage5;

    [SerializeField] GameObject RankSprite;
    [SerializeField] Sprite S_Rank;
    [SerializeField] Sprite A_Rank;
    [SerializeField] Sprite B_Rank;
    [SerializeField] Sprite C_Rank;
    [SerializeField] Sprite F_Rank;

    [SerializeField] Slider fervorBar;
    [SerializeField] float fervorLevel;
    float fervor2Add;
    float fervorMaxLevel;
    [SerializeField] float combo_timer;

    //If fervorlevel >= 70, this is true, else false
    bool buff_active;

    [SerializeField] TextMeshProUGUI combo_timer_text;
    [SerializeField] TextMeshProUGUI combo_text;

    float hurt_period;




    public void Start()
    {
        PlayerAttack = 2;
        fervorMaxLevel = 100;
        fervor2Add = 0;
        buff_active = false;

        if (fervorBar != null)
        {
            fervorBar.maxValue = fervorMaxLevel;
        }

        fervorLevel = 0;
        numberConsecutiveHits = 0;
        hurt_period = 0;
    }



    public void setAttack(int atk)
    {
        PlayerAttack = atk;
    }

    public int getAttack(int additionalAtk = 0)
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

        //Debug.Log("Fervor: " + fervorLevel);
    }


    public void Update()
    {

      
        if (GetComponentInChildren<Animator>().GetBool("Hurt") == true)
        {
            //Debug.Log("Animation name "
            //    + GetComponentInChildren<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name);
            hurt_period += Time.deltaTime;

            if(hurt_period >= GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length)
            {
                GetComponentInChildren<Animator>().SetBool("Hurt", false);
            }
        }
        else
        {
            hurt_period = 0.0f;
        }
        if (combo_timer_text != null)
        {
            combo_timer_text.SetText(((int)combo_timer).ToString());
        }


        if (fervorBar != null)
        {
            fervorBar.value = fervorLevel;
        }
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


        if (combo_text != null)
        {
            combo_text.text = numberConsecutiveHits.ToString();
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


        if (numberConsecutiveHits <= 0)
        {
            RankSprite.GetComponent<Image>().enabled = false;
        }
        else
        {
            RankSprite.GetComponent<Image>().enabled = true;
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
        //[SerializeField] Image S_Rank;
        //[SerializeField] Image A_Rank;
        //[SerializeField] Image B_Rank;
        //[SerializeField] Image C_Rank;
        //[SerializeField] Image F_Rank;

        if (numberConsecutiveHits >= consecutive_stage_min
            && numberConsecutiveHits < consecutive_stage_max)
        {
            fervor2Add = fervor_2add;
        }
    }


    

    public void decidecombotimer(float consecutive_stage_min, float consecutive_stage_max, float comb_timer, Sprite rank_sprite)
    {
        //[SerializeField] Image S_Rank;
        //[SerializeField] Image A_Rank;
        //[SerializeField] Image B_Rank;
        //[SerializeField] Image C_Rank;
        //[SerializeField] Image F_Rank;

        if (numberConsecutiveHits >= consecutive_stage_min
            && numberConsecutiveHits < consecutive_stage_max)
        {

            combo_timer = comb_timer;
            RankSprite.GetComponent<Image>().sprite = rank_sprite;
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
            15.9f, F_Rank);
        //between 10 and 15
        decidecombotimer(ConsecutiveHit_Stage2,
            ConsecutiveHit_Stage3,
            12.9f, C_Rank);
        //between 15 and 20
        decidecombotimer(ConsecutiveHit_Stage3,
            ConsecutiveHit_Stage4,
            10.9f, B_Rank);
        //between 20 and 25
        decidecombotimer(ConsecutiveHit_Stage4,
            ConsecutiveHit_Stage5,
            7.9f, A_Rank);
        //more than 25
        decidecombotimer(ConsecutiveHit_Stage5,
            ConsecutiveHit_Stage5 + 999,
            3.9f, S_Rank);
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
    public int GetPlayerAttack()
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
