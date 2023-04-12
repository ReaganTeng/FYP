using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class EnemyScript : MonoBehaviour
{
    [SerializeField] public float EnemyHealth;
    public float AttackDamage;
    [SerializeField] GameObject prepPrefab;
    [SerializeField] GameObject choppedPefab;
    [SerializeField] GameObject smashedPrefab;
    [SerializeField] GameObject Mush;


    float Iframemaxtime = 0.1f;
    float Iframetimer = 0.1f;
    bool Iframe = false;
    bool FirstAttack = false;
    int AttackByWhatWeapon = 0;
    bool AttackByOtherWeapon = false;

    [SerializeField] Slider healthbar;
     GameObject player;

    //public GameObject[] zones;
    //public int zone_no;
    float timer;
    [SerializeField] float cooldown_period;
    [SerializeField] float abouttoattack_period;

    float transitionFromHurtTimer;

    int attack_type;

    [SerializeField] GameObject attackhitbox;

    bool updating;
    int zoneno;
    GameObject[] zone;
    GameObject hitbox;

    public enum Phases
    {
        ABOUT_TO_ATTACK,
        COOLDOWN,
        ATTACK_TYPE_1,
        ATTACK_TYPE_2,
        
        TOTAL
    }


    [SerializeField] public Phases phase;

    Transform spawnerparent;
    public void setparent(Transform parentSpawner)
    {
        spawnerparent = parentSpawner;
    }

    public Transform getparent()
    {
        return spawnerparent;
    }

    void Awake()
    {
        updating = false;
        zoneno = 0;
        zone = GameObject.FindGameObjectsWithTag("Zone");
        BoundaryCheck();
        hitbox = GameObject.FindGameObjectWithTag("Attack");

        transitionFromHurtTimer = 0;
        //phase = Phases.PHASE_3;

        healthbar.maxValue = EnemyHealth;
        healthbar.minValue = 0;
        player = GameObject.FindGameObjectWithTag("Player");


        GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
        GetComponentInChildren<Animator>().SetBool("attacked", false);

        //phase = Phases.ATTACK_TYPE_1;
        timer = 0.0f;
    }

    void Start()
    {
        updating = false;

        zoneno = 0;
        zone = GameObject.FindGameObjectsWithTag("Zone");
        BoundaryCheck();
        hitbox = GameObject.FindGameObjectWithTag("Attack");

        transitionFromHurtTimer = 0;
        //phase = Phases.PHASE_3;

        healthbar.maxValue = EnemyHealth;
        healthbar.minValue = 0;
        player = GameObject.FindGameObjectWithTag("Player");

        GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
        GetComponentInChildren<Animator>().SetBool("attacked", false);


        //phase = Phases.ATTACK_TYPE_1;
        timer = 0.0f;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Floor"))
            return;

        // if the object calling this is not the base enemy, ignore
        if (!gameObject.CompareTag("Enemy"))
            return;

        // If its from player attack
        if (other.CompareTag("Attack") && !Iframe
            && GetComponent<BoxCollider>().enabled == true
            && player.GetComponentInChildren<PlayerAttack>().getHitbox() == true)
        {
            EnemyHealth -= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().GetPlayerAttack();

            //play attacked animation
            GetComponentInChildren<Animator>().SetBool("attacked", true);
            //

            player.GetComponent<PlayerStats>().addConsecutiveHit();
            player.GetComponent<PlayerStats>().resetCombo_timer();

            GetComponent<Rigidbody>().AddForce(
               (GetComponent<Transform>().position - other.GetComponentInParent<Transform>().position).normalized * 100.0f,
               ForceMode.Impulse
               );

            Iframe = true;

            if (!FirstAttack)
            {
                FirstAttack = true;
                AttackByWhatWeapon = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerAttack>().GetWeaponType();
            }

            else if (FirstAttack)
            {
                // if the weapon the enemy was attack by is not the same
                if (AttackByWhatWeapon != GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerAttack>().GetWeaponType())
                {
                    AttackByOtherWeapon = true;
                }
            }


            if (phase != Phases.COOLDOWN)
            {
                phase = Phases.COOLDOWN;
            }

            Debug.Log("Enemy Health Left: " + EnemyHealth);

            // Precise Kill
            /*if (EnemyHealth == 0)
            {
                Debug.Log("Precise Kill!");
                EnemyDie(true);
            }

            else if (EnemyHealth < 0)
            {
                Debug.Log("Killed!");
                EnemyDie(false);
            }*/
        }
    }

    private void Update()
    {
        //hitbox = GameObject.FindGameObjectWithTag("Attack");


        //if (player.GetComponentInChildren<PlayerAttack>().getHitbox() == true)
        //{
        //    Debug.Log("HITBOX TRUE");
        //}

        player = GameObject.FindGameObjectWithTag("Player");

        GetComponentInChildren<Animator>().SetFloat("health", EnemyHealth);
       
        if (GetComponentInChildren<Animator>().GetBool("attacked") == true)
        {
            //Debug.Log("OH IM ATTACKED");
            if (EnemyHealth > 0)
            {
                GetComponentInChildren<Animator>().speed = 3;
                transitionFromHurtTimer += 1.0f * Time.deltaTime;

                if(transitionFromHurtTimer >= 1.0f)
                {
                    GetComponentInChildren<Animator>().SetBool("attacked", false);
                }
            }
        }
        else
        {
            GetComponentInChildren<Animator>().speed = 1.5f;
            transitionFromHurtTimer = 0.0f;
        }

        


        //Debug.Log("ANIMATOR REGISTER " + GetComponentInChildren<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name);



        healthbar.value = EnemyHealth;

        if (Iframe)
        {
            if (Iframetimer > 0)
                Iframetimer -= Time.deltaTime;
            if (Iframetimer <= 0)
            {
                Iframetimer = Iframemaxtime;
                Iframe = false;
            }
        }

        zone = GameObject.FindGameObjectsWithTag("Zone");
        BoundaryCheck();

        if (zoneno == player.GetComponent<PlayerZoneCheck>().getZoneno())
        {
            updating = true;
        }
        else
        {
            updating = false;
        }

        if (updating == false)
        {
            phase = Phases.ABOUT_TO_ATTACK;
        }



        switch (phase)
            {
                case Phases.ABOUT_TO_ATTACK:
                    {

                    if (updating)
                    {
                        abouttoattackUpdate();
                    }
                    else
                    {
                        //if (GetComponent<ChaserScript>() != null)
                        //{
                        //    GetComponent<ChaserScript>().DestroyBeams();
                        //}

                        // Debug.Log("CURRENT POSITION " + GetComponentInParent<Transform>().name);
                        GetComponent<BoxCollider>().enabled = true;
                        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);


                        if (GetComponent<NavMeshAgent>().enabled == true)
                        {
                            GetComponent<NavMeshAgent>().speed = 5.0f;
                            GetComponent<NavMeshAgent>().SetDestination(getparent().position);
                        }

                        GetComponentInChildren<Canvas>().transform.localPosition = new Vector3(0, 0, 0);
                        GetComponentInChildren<SpriteRenderer>().transform.localPosition = new Vector3(0, 0.66f, 0);
                        GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
                    }
                        /*timer += 1.0f * Time.deltaTime;
                        GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                        GetComponent<NavMeshAgent>().speed = 0.0f;


                        attackhitbox.GetComponent<BoxCollider>().enabled = false;
                        GetComponent<BoxCollider>().enabled = true;


                        if (timer >= abouttoattackend)
                        {
                            timer = 0.0f;
                            attack_type = Random.Range(1, 3);


                            if (attack_type == 1)
                            {
                                phase = Phases.ATTACK_TYPE_1;
                            }
                            else
                            {
                                phase = Phases.ATTACK_TYPE_2;
                            }
                        }*/
                        break;
                    }
                case Phases.COOLDOWN:
                    {
                    //Debug.Log("COOLDOWN");
                    attackhitbox.GetComponent<BoxCollider>().enabled = false;


                    cooldownUpdate();
                    

                        break;
                    }
            }
        
        
        

    }


    public void BoundaryCheck()
    {
        for (int i = 0; i < zone.Length; i++)
        {
            if (transform.position.x < zone[i].GetComponent<Transform>().position.x + (zone[i].GetComponent<Transform>().localScale.x / 2)
             && transform.position.x > zone[i].GetComponent<Transform>().position.x - (zone[i].GetComponent<Transform>().localScale.x / 2)
             && transform.position.z > zone[i].GetComponent<Transform>().position.z - (zone[i].GetComponent<Transform>().localScale.z / 2)
            && transform.position.z < zone[i].GetComponent<Transform>().position.z + (zone[i].GetComponent<Transform>().localScale.z / 2)
             )

            {
                zoneno = zone[i].GetComponent<WhatZone>().zone_number;
                break;
            }
            else
            {
                zoneno = 0;
            }
        }
    }

    public int getzoneno()
    {
        return zoneno;
    }

    public bool getupdating()
    {
        return updating;
    }
    public void abouttoattackUpdate()
    {
        GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
        timer += Time.deltaTime;
        GetComponent<NavMeshAgent>().speed = 0.0f;
        attackhitbox.GetComponent<BoxCollider>().enabled = false;
        GetComponent<BoxCollider>().enabled = true;

        //Debug.Log("ABOUT TO ATTACK IN " + (int)timer);


        if (timer >= abouttoattack_period)
        {
            timer = 0.0f;
            attack_type = Random.Range(1, 3);

            if (attack_type == 1)
            {
                phase = Phases.ATTACK_TYPE_1;
            }
            else
            {
                phase = Phases.ATTACK_TYPE_2;
            }
        }

    }

    public void cooldownUpdate()
    {
        GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);

        timer += 1.0f * Time.deltaTime;
        GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
        GetComponent<NavMeshAgent>().speed = 0.0f;

        attackhitbox.GetComponent<BoxCollider>().enabled = false;
        GetComponent<BoxCollider>().enabled = true;

        if (timer >= cooldown_period)
        {
            timer = 0.0f;
            phase = Phases.ABOUT_TO_ATTACK;
        }
    }

    public void settimer(float time)
    {
        timer = time;
    }

    public void addtimer(float time)
    {
        timer += time;
    }

    

    public float gettimer()
    {
        return timer;
    }

    public float getcooldownend()
    {
        return cooldown_period;
    }

    void EnemyDie(bool ExactKill)
    {
        // if the enemy was attack by another weapon before drop mush
        if (AttackByOtherWeapon)
        {
            Instantiate(Mush, gameObject.transform.position, Quaternion.identity);
        }
        else
        {
            GameObject whichobject = null;

            switch (AttackByWhatWeapon)
            {
                // 0 is spatula, 1 is knife, 2 is rolling pin
                case 0:
                    whichobject = prepPrefab;
                    break;
                case 1:
                    whichobject = choppedPefab;
                    break;
                case 2:
                    whichobject = smashedPrefab;
                    break;
            }
            GameObject tempobj = Instantiate(whichobject, gameObject.transform.position, Quaternion.identity);

            // if it is exact kill
            if (ExactKill)
            {
                tempobj.GetComponent<Food>().SetPerfect(true);
            }
        }

        /*float myTime = GetComponentInChildren<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length
            * GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
         //if (myTime >= 0.9f * GetComponentInChildren<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length)
         Debug.Log("ANIMATOR REGISTER " + GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime);*/

        //if (GetComponentInChildren<Animator>().GetComponent<DieFinish>().returnDead() == true)
        //{
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        Debug.Log("DESTROYED");
        Destroy(gameObject);
        //}
    }

    public void Death()
    {
        //attackhitbox.SetActive(false);
        attackhitbox.GetComponent<BoxCollider>().enabled = false;

        //Destroy(attackhitbox);

        if (EnemyHealth == 0)
        {
            Debug.Log("Precise Kill!");
            EnemyDie(true);
        }
        else if (EnemyHealth < 0)
        {
            Debug.Log("Killed!");
            EnemyDie(false);
        }
    }
}
