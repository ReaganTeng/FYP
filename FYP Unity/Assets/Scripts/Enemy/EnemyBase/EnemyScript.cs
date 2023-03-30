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
    [SerializeField] GameObject player;

    //public GameObject[] zones;
    //public int zone_no;
    float timer;
     float cooldownend;
    float abouttoattackend;

    int attack_type;

    public GameObject attackhitbox;

    public enum Phases
    {
        ABOUT_TO_ATTACK,
        COOLDOWN,
        ATTACK_TYPE_1,
        ATTACK_TYPE_2,
        
        TOTAL
    }


    [SerializeField] public Phases phase;

    void Start()
    {
        //phase = Phases.PHASE_3;

        healthbar.maxValue = EnemyHealth;
        healthbar.minValue = 0;

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
        if (other.CompareTag("Attack") && !Iframe)
        {
            EnemyHealth -= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().GetPlayerAttack();

            //play attacked animation
            GetComponentInChildren<Animator>().SetBool("attacked", true);
            //

            player.GetComponent<PlayerStats>().addConsecutiveHit();
            player.GetComponent<PlayerStats>().resetCombo_timer();
            GetComponent<Rigidbody>().AddForce(
               (GetComponent<Transform>().position - other.GetComponentInParent<Transform>().position).normalized * 50.0f,
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
            //if (EnemyHealth == 0)
            //{
            //    Debug.Log("Precise Kill!");
            //    EnemyDie(true);
            //}
            //else if (EnemyHealth < 0)
            //{
            //    Debug.Log("Killed!");
            //    EnemyDie(false);
            //}
        }
    }

    

    private void Update()
    {
        GetComponentInChildren<Animator>().SetFloat("health", EnemyHealth);

        Debug.Log("ENEMY " + (int)GetComponentInChildren<Animator>().GetFloat("health"));

        //when to stop hurt animation
        if (EnemyHealth > 0
            && GetComponentInChildren<Animator>().GetBool("attacked") == true)
        {
            //Debug.Log("STILL HURT!");

            float myTime = GetComponentInChildren<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length
        * GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;

            //Debug.Log("ANIMATOR REGISTER " + myTime);

            if (myTime >= 0.95f)
            {
                //Debug.Log("NOT HURT!");

                GetComponentInChildren<Animator>().SetBool("attacked", false);
            }
        }
        //


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

        switch (phase)
        {
            case Phases.ABOUT_TO_ATTACK:
                {
                    abouttoattackUpdate();

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
                    
                    cooldownUpdate();

                    break;
                }
        }

    }

    public void abouttoattackUpdate()
    {
        timer += 1.0f * Time.deltaTime;
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
        }

    }

    public void cooldownUpdate()
    {
        timer += 1.0f * Time.deltaTime;
        GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
        GetComponent<NavMeshAgent>().speed = 0.0f;

        attackhitbox.GetComponent<BoxCollider>().enabled = false;
        GetComponent<BoxCollider>().enabled = true;

        if (timer >= cooldownend)
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

    public void setCoolDownEnd(float time)
    {
        cooldownend = time;
    }

    public void setabouttoattackend(float time)
    {
        abouttoattackend = time;
    }

    public float gettimer()
    {
        return timer;
    }

    public float getcooldownend()
    {
        return cooldownend;
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
        attackhitbox.SetActive(false);
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
