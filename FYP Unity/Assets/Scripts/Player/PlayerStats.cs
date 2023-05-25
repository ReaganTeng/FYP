using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public PlayerProgress playerProgress;

    //WON'T BE USED FOR NOW
    //[SerializeField] float PlayerHealth;
    //
    
    // Set the attack for the player
    [SerializeField] int NormalAttackDamage;
    [SerializeField] int HeavyAttackDamage;
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


    [SerializeField] Sprite non_fervor_mode;
    [SerializeField] Sprite fervor_mode;
    [SerializeField] Image fillimage;



    [SerializeField] Slider fervorBar;
    float fervorLevel;
    float fervor2Add;
    float fervorMaxLevel;
    float combo_timer;

    //If fervorlevel >= 70, this is true, else false
    bool buff_active;

    [SerializeField] TextMeshProUGUI combo_timer_text;
    [SerializeField] TextMeshProUGUI combo_text;

    [SerializeField] TextMeshProUGUI HitText;


    float hurt_period;

        // Enabling/Disabling Fervor bar (for tutorial purposes. DO NOT REMOVE)
    bool fervorBarActive = true;

    float alpha_change;

   

    [SerializeField] PlayerProgress pp;


    [SerializeField] GameObject particles;

    bool instant_kill_mode;

    //DINNER RUSH UPGRADE
    bool burstmode;
    float bursttime;
    //
    public void Start()
    {

        instant_kill_mode = false;

        burstmode = false;
        bursttime = 0.0f;

        setAttack(false);
        fervorMaxLevel = 100;
        fervor2Add = 0;
        buff_active = false;

        alpha_change = 0.0f;

        if (fervorBar != null)
        {
            fervorBar.maxValue = fervorMaxLevel;
            fervorBar.minValue = 0;
        }

        fervorLevel = 0;
        numberConsecutiveHits = 0;

        hurt_period = 0;
        //RankSprite.SetActive(false);
    }


    public PlayerProgress getpp()
    {
        return pp;
    }

    public void setAttack(bool Isheavy)
    {
        // if it is heavy, set it to heavy attack dmg
        if (Isheavy)
            PlayerAttack = HeavyAttackDamage;
        // if it is not heavy, set it to normal attack dmg
        else
            PlayerAttack = NormalAttackDamage;
    }

    //public void ChangeHealth(float Healthchange)
    //{
    //    PlayerHealth += Healthchange;
    //    Debug.Log("Player Health: " + PlayerHealth);

    //    if (PlayerHealth <= 0)
    //    {
    //        Debug.Log("Imagine dying to ingredients!!");
    //    }
    //}

    public void ChangeFervor(float Fervorchange)
    {
        if (fervorBarActive)
        {
            if (fervorLevel > 0)
            {
                fervorLevel += Fervorchange;

                if (fervorLevel < 0)
                {
                    fervorLevel = 0;
                }
            }
        }
        Debug.Log("Fervor: " + fervorLevel + "REDUCED BY " + Fervorchange);
    }

    //DINNER RUSH UPGRADE
    public void setbursttime(float bt)
    {
        bursttime = bt;
    }
    public bool getburstmode()
    {
        return burstmode;
    }
    //

    //FOR JUST DIE ALREADY UPGRADE
    public bool getinstantkillmode()
    {
        return instant_kill_mode;
    }
    public void reducefervor()
    {
        fervorLevel -= pp.return_justdiealready();
    }
    //



    public void Update()
    {
        //DINNER RUSH UPGRADE
        if (bursttime > 0)
        {
            bursttime -= Time.deltaTime;
            burstmode = true;
        }
        else
        {
            burstmode = false;
        }
        //

        //GO INSTANT KILL MODE WHEN FERVOR REQUIREMENT IS MORE THAN THE FERVOR REQUIREMENT TO GO THERE
        //BASE ON JUST_DIE_ALREADY UPGRADE
        if(fervorLevel > pp.return_justdiealready()
            && pp.return_justdiealready() > 0)
        {
            instant_kill_mode = true;
        }
        else
        {
            instant_kill_mode = false;
        }
        //


        //when player is playing hurt animation
        if (GetComponentInChildren<Animator>().GetBool("Hurt"))
        {
            //Debug.Log("OUCH");
            hurt_period += Time.deltaTime;
            particles.SetActive(true);
            if(hurt_period >= GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length)

            {
                particles.SetActive(false);
                GetComponentInChildren<Animator>().SetBool("Hurt", false);
            }
        }
        else
        {
            particles.SetActive(false);
            hurt_period = 0.0f;
        }

        //Debug.Log("BUFF " + pp.return_hyper_focused_cooking());
        //TEMPORARY, FOR TESTING PURPOSES
        //if (Input.GetMouseButtonDown(0))
        //{
        //    for (int i = 0; i < 5; i++)
        //    {
        //        addConsecutiveHit();
        //        resetCombo_timer();
        //        //Debug.Log("CONSECUTIVE HITS");
        //    }
        //}

        if (fervorBarActive)
        {
            if (combo_timer_text != null)
            {
                combo_timer_text.enabled = true;
                combo_timer_text.SetText(((int)combo_timer).ToString());
            }


            if (fervorBar != null)
            {
                fervorBar.enabled = true;
                fervorBar.value = fervorLevel;
            }
            if (fervorLevel > 0
                )
            {
                //REDUCE THE FERVOR, AND CAN CHANGE ITS SPEED BASE OF CALM MIND SHOP UPGRADE
                fervorLevel -= pp.return_calmmind() *  2.0f * Time.deltaTime;
            }


            if(fervorLevel < 0)
            {
                fervorLevel = 0;
            }

            if (combo_timer <= 0
                && numberConsecutiveHits >= 5
                && !Input.GetMouseButtonDown(0)
                 && !Input.GetMouseButtonDown(1))
            {
                Debug.Log("RESET");
                ResetConsecutiveHit();
                numberConsecutiveHits = 0;
            }
            

            if (combo_timer > 0)
            {
                combo_timer -= Time.deltaTime;
                alpha_change += /*.4f */ /* numberConsecutiveHits*/ /** */Time.deltaTime;            
            }
            else
            {
                numberConsecutiveHits = 0;
                combo_timer = 0;
                alpha_change = 0;
                
            }

            //change the alpha of ranksprite
            var alphaval = RankSprite.GetComponent<Image>().color;
            if (alpha_change > 0)
            {
                alphaval.a = 1.0f / alpha_change;
                RankSprite.GetComponent<Image>().color = alphaval;
            }
            else
            {
                alphaval.a = 0;
                RankSprite.GetComponent<Image>().color = alphaval;
            }

            if (combo_timer < 0)
            {
                combo_timer = 0;
            }

            if (combo_text != null)
            {
                if (numberConsecutiveHits > 0)
                {
                    combo_text.enabled = true;
                    combo_text.text = numberConsecutiveHits.ToString();
                }
                else
                {
                    combo_text.enabled = false;

                }
            }

           
            if (HitText != null)
            {
                if (numberConsecutiveHits > 0)
                {
                    HitText.enabled = true;
                    RankSprite.GetComponent<Image>().enabled = true;
                }
                else
                {
                    HitText.enabled = false;
                    RankSprite.GetComponent<Image>().enabled = false;
                }
            }


            //IF FERVOR REACHES FERVOR REQUIREMENT TO GO BUFF MODE
            //CAN BE REDUCED BY HYPER FOCUSED COOKING SHOP UPGRADE
            if (fervorLevel >= fervorMaxLevel - pp.return_hyper_focused_cooking())
            {
                fillimage.sprite
                    = fervor_mode;
                buff_active = true;
            }
            else
            {
                fillimage.sprite
                    = non_fervor_mode;

                buff_active = false;
            }
            //

            
        }
        else
        {

            if (HitText != null)
            {
                HitText.enabled = false;
                RankSprite.GetComponent<Image>().enabled = false;
            }

            if (combo_timer_text != null)
            {
                combo_timer_text.enabled = false;
            }

            if (fervorBar != null)
            {
                fervorBar.enabled = false;
            }

            if (combo_text != null)
            {
                combo_text.enabled = false;
            }

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


        
    }

    public void decidefervor2add(float consecutive_stage_min, float consecutive_stage_max, float fervor_2add)
    {
       
        if (numberConsecutiveHits >= consecutive_stage_min
            && numberConsecutiveHits < consecutive_stage_max)
        {
            fervor2Add = fervor_2add;
        }
    }



    public void decidecombotimer(float consecutive_stage_min, float consecutive_stage_max, float comb_timer, Sprite rank_sprite)
    {
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
        if (fervorBarActive)
        {
            //between 0 and 5
            decidecombotimer(-999,
                   ConsecutiveHit_Stage1,
                   15.9f, F_Rank);

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
    }



   
    public void ResetConsecutiveHit()
    {
        if (fervorBarActive)
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

            //numberConsecutiveHits = 0;
            //combo_timer = 0;
        }

        //Debug.Log("CON " + numberConsecutiveHits);
    }
    

    public void resetval()
    {
        numberConsecutiveHits = 0;
        combo_timer = 0;
    }

    public void addConsecutiveHit()
    {
        if (fervorBarActive)
        {
            alpha_change = 0;

            numberConsecutiveHits += 1;
        }
        //Debug.Log("CON " + numberConsecutiveHits);

    }
    public int GetPlayerAttack()
    {
        return PlayerAttack;
    }


    

    public void SetIfFervorActive(bool fervorActive)
    {
        fervorBarActive = fervorActive;
    }
}
