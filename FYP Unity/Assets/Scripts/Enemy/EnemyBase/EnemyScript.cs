using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
public class EnemyScript : MonoBehaviour
{
    float targetVelocity;
    int numberOfRays;
    float angle;
    float rayRange;
    [SerializeField] LayerMask lm;

    [SerializeField] LayerMask lm_2;


    int EnemyHealth;
    [SerializeField] float AttackDamage;
    [SerializeField] GameObject prepPrefab;
    [SerializeField] GameObject choppedPefab;
    [SerializeField] GameObject smashedPrefab;
    [SerializeField] GameObject Mush;

    [SerializeField] GameObject attackhitbox;


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


    float post_attack_duration;
    bool updating;
    int zoneno;
    GameObject[] zone;
    GameObject hitbox;


    List<float> ray_distances;
    float furthestdistance;

    float currentAnimationLength;
    public enum Phases
    {
        ABOUT_TO_ATTACK,
        COOLDOWN,
        ATTACK_TYPE_1,
        ATTACK_TYPE_2,
        
        TOTAL
    }


    public enum EnemyType
    {
        JUMPER,
        CHARGER,
        CHASER,

        TOTAL
    }


    [SerializeField] Phases phase;
    EnemyType enemy_type;

    Transform spawnerparent;

    float shootTimer;
    [SerializeField] GameObject projectileGO;
    GameObject projectile;

    [SerializeField] TextMeshProUGUI healthtext;
    public void setparent(Transform parentSpawner)
    {
        spawnerparent = parentSpawner;
    }

    public Transform getparent()
    {
        return spawnerparent;
    }

    public Phases return_current_phase()
    {
        return phase;
    }


    public void set_current_phase(Phases current_phase)
    {
        phase = current_phase;
    }

    public EnemyType return_enemyType()
    {
        return enemy_type;
    }

    public void set_enemyType(EnemyType type)
    {
        enemy_type = type;
    }

    void Start()
    {
        post_attack_duration = 0.0f;
        targetVelocity = 3.0f;
        numberOfRays = 30;
        angle = 90.0f;
        rayRange = 1.0f;

        EnemyHealth = 10;
        updating = false;
        zoneno = 0;
        zone = GameObject.FindGameObjectsWithTag("Zone");
        hitbox = GameObject.FindGameObjectWithTag("Attack");
        player = GameObject.FindGameObjectWithTag("Player");
        transitionFromHurtTimer = 0.0f;
        healthbar.maxValue = EnemyHealth;
        healthbar.minValue = 0;
        attackhitbox.GetComponent<BoxCollider>().enabled = false ;
       
        BoundaryCheck();
        timer = 0.0f;

        ray_distances = new List<float>();
        shootTimer = 0.0f;
    }

    


    private void OnTriggerEnter(Collider other)
    //private void OnCollisionEnter(Collision other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Floor"))
            return;

        // if the object calling this is not the base enemy, ignore
        if (!gameObject.CompareTag("Enemy"))
            return;

        // If its from player attack
        if (
            other.CompareTag("Attack") && Iframe == false
            && GetComponent<BoxCollider>().enabled == true
            //&& other.GetComponent<BoxCollider>().enabled == true
            //&& (!GetComponentInChildren<Animator>().GetBool("jump") && enemy_type == EnemyType.JUMPER)
            )
        {
            EnemyHealth -= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().GetPlayerAttack();

            //Debug.Log("OUCH");

     
            for (int i = 0; i < 10; i++)
            {
                player.GetComponent<PlayerStats>().addConsecutiveHit();
                player.GetComponent<PlayerStats>().resetCombo_timer();
            }

            //GetComponent<Rigidbody>().AddForce(
            //   (GetComponent<Transform>().position - other.GetComponentInParent<Transform>().position).normalized * 100.0f,
            //   ForceMode.Impulse
            //   );

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


            //SET THE ENEMY BACK TO IDLE MODE
            switch (enemy_type)
            {
                case EnemyType.JUMPER:
                    {
                        GetComponentInChildren<Animator>().SetBool("jump", false);
                        break;
                    }
                case EnemyType.CHASER:
                    {
                        GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
                        GetComponentInChildren<Animator>().SetBool("attack", false);
                        break;
                    }
                case EnemyType.CHARGER:
                    {
                        GetComponentInChildren<Animator>().SetBool("charge", false);
                        GetComponentInChildren<Animator>().SetBool("about2charge", false);
                        break;
                    }
            }
            //
            

            transitionFromHurtTimer = 0.0f;

            //play attacked animation
            GetComponentInChildren<Animator>().SetBool("attacked", true);
            //

            Iframe = true;
        }
    }

    private void Update()
    {
        hitbox = GameObject.FindGameObjectWithTag("Attack");
        player = GameObject.FindGameObjectWithTag("Player");

        healthtext.text = EnemyHealth.ToString();

        GetComponentInChildren<Animator>().SetFloat("health", EnemyHealth);
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

        //POST ATTACK
        if (GetComponentInChildren<EnemyAttack>().getpostattack())
        {
            post_attack_duration += Time.deltaTime;

            if(post_attack_duration > 2.0f)
            {
                post_attack_duration = 0.0f;
                GetComponentInChildren<EnemyAttack>().setpostattack(false);
            }
        }
        //

        if (zoneno == player.GetComponent<PlayerZoneCheck>().getZoneno())
        {
            updating = true;
        }
        else
        {
            updating = false;
        }

        //if (updating == true)
        //{
            
        //}
        if (updating == false)
        //else
        {
            phase = Phases.ABOUT_TO_ATTACK;
        }


        currentAnimationLength = GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length;

        //Debug.Log("ANIMATION LENGTH " + currentAnimationLength);
        //GetComponentInChildren<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.frameRate;

        if (GetComponentInChildren<Animator>().GetBool("attacked"))
        {
            transitionFromHurtTimer += Time.deltaTime;
            if (transitionFromHurtTimer >=
                currentAnimationLength)
            {
                GetComponentInChildren<Animator>().SetBool("attacked", false);

                if (enemy_type == EnemyType.JUMPER)
                {
                    GetComponentInChildren<Animator>().SetBool("about2jump", false);
                    GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
                }
            }
        }

        //shootTimer += Time.deltaTime;
        //if (shootTimer >= 3.0f)
        //{
        //    shoot();
        //    shootTimer = 0.0f;
        //}

        //Debug.Log("Timer " + (int)transitionFromHurtTimer);
        //Debug.Log("ATTACK BOOL " + GetComponentInChildren<Animator>().GetBool("attacked"));
        //Debug.Log("ATTACK SPEED " + (GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).speed));
    }


    public void shoot()
    {
        Vector3 resultingVector = player.transform.position - transform.position;
        resultingVector.y = 0;
        projectile = Instantiate(projectileGO, transform.position, Quaternion.Euler(0, 0, 0));
        projectile.GetComponent<Rigidbody>().velocity = resultingVector * 5;
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

    public void ifUpdatingfalse()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        attackhitbox.GetComponent<BoxCollider>().enabled = false;

        if (GetComponent<NavMeshAgent>().enabled == true)
        {
            GetComponent<NavMeshAgent>().speed = 5.0f;
            GetComponent<NavMeshAgent>().SetDestination(getparent().position);
        }

        GetComponentInChildren<Canvas>().transform.localPosition = new Vector3(0, 0, 0);
        GetComponentInChildren<SpriteRenderer>().transform.localPosition = new Vector3(0, 0.66f, 0);
        GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
    }
    public void steering()
    {
        var deltaPosition = Vector3.zero;
        for (int i = 0; i < numberOfRays; i++)
        {
            //rotate enemy angle
            var rotation = transform.rotation;
            var rotationMod = Quaternion.AngleAxis(
                 (i / ((float)numberOfRays - 1)) * angle * 2 - angle,
                 transform.up);
            var direction = rotation * rotationMod * Vector3.forward;
            var direction2 = rotation * rotationMod * Vector3.back;

            var ray = new Ray(transform.position, direction);
            var ray2 = new Ray(transform.position, direction2);

            //if hits enemy
            if (Physics.Raycast(ray, rayRange
                , lm))
            {
                deltaPosition -= (1.0f / numberOfRays) * targetVelocity * direction;
                transform.position += deltaPosition * Time.deltaTime;
            }
            else if (Physics.Raycast(ray2, rayRange
                , lm))
            {
                deltaPosition -= (1.0f / numberOfRays) * targetVelocity * direction2;
                transform.position += deltaPosition * Time.deltaTime;
            }

            
            //RaycastHit hitInfo;
            //if (Physics.Raycast(ray, out hitInfo, 100.0f/*, lm_2*/))
            //{
            //    ray_distances.Add(hitInfo.distance);
            //}
            
        }

        //ray_distances.Sort();
        //furthestdistance = ray_distances[ray_distances.Count - 1];
        //Debug.Log("Largest Distance " + furthestdistance);

        //Vector3 resultingVector = furthestdistance - transform.position;


        /*float hoverHeight = 4.0f;
        RaycastHit hit;
        Ray downRay = new Ray(transform.position, -Vector3.right);
        // Cast a ray straight downwards.
        if (Physics.Raycast(downRay, out hit))
        {
            // The "error" in height is the difference between the desired height
            // and the height measured by the raycast distance.
            float hoverError = hoverHeight - hit.distance;

            Debug.Log("HIT DISTANCE " + hit.distance);
        }*/


    }



    public GameObject gethitbox()
    {
        return attackhitbox;
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
        GetComponent<BoxCollider>().enabled = true;

        //if (updating)
        //{
            GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
            timer += Time.deltaTime;
            GetComponent<NavMeshAgent>().speed = 0.0f;
            attackhitbox.GetComponent<BoxCollider>().enabled = false;

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
        //turn off attack hitbox
        attackhitbox.GetComponent<BoxCollider>().enabled = false;
        timer += 1.0f * Time.deltaTime;
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
        //Debug.Log("DESTROYED");
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
