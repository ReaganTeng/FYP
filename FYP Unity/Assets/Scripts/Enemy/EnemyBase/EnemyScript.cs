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
    [SerializeField] LayerMask player_lm;

    [SerializeField] int EnemyHealth; // the base health of the enemy
    int currentHealth; // the current health of the enemy
    [SerializeField] float AttackDamage;
    [SerializeField] GameObject prepPrefab;
    [SerializeField] GameObject choppedPefab;
    [SerializeField] GameObject smashedPrefab;
    [SerializeField] GameObject Mush;
    [SerializeField] GameObject attackhitbox;

    float rand_x;
    float rand_y;

    int rand_gen;

    float Iframemaxtime = 0.1f;
    float Iframetimer = 0.1f;
    bool Iframe = false;
    bool FirstAttack = false;
    int AttackByWhatWeapon = 0;
    bool AttackByOtherWeapon = false;

    [SerializeField] Slider healthbar;
     GameObject player;

    float phase3timer;
    float timer;
    [SerializeField] float cooldown_period;
    [SerializeField] float abouttoattack_period;

    float transitionFromHurtTimer;
    float transitionFromAttackTimer;

    int attack_type;

    int setzoneno;

    float post_attack_duration;
    bool updating;
    int zoneno;
    GameObject[] zone;
    GameObject hitbox;

    List<float> ray_distances;

    Dictionary<float, Vector3> ray_distance_n_direction;

    [SerializeField] int projectile_numbers;
    int projectile_shots;
    float interval_between_shots;

    float furthestdistance;

    GameObject[] other_enemies;

    float currentAnimationLength;
    public enum Phases
    {
        ABOUT_TO_ATTACK,
        COOLDOWN,
        ATTACK_TYPE_1,
        ATTACK_TYPE_2,
        ATTACK_TYPE_3,

        AVOID,


        TOTAL
    }


    public enum EnemyType
    {
        JUMPER,
        CHARGER,
        CHASER,

        TOTAL
    }


    public enum AttackPattern
    {
        PATTERN_1,
        PATTERN_2,
        PATTERN_3,

        TOTAL
    }
    [SerializeField] AttackPattern atkPattern;


    [SerializeField] Phases phase;
    EnemyType enemy_type;

    Transform spawnerparent;

    float shootTimer;
    [SerializeField] GameObject projectileGO;
    float proejctilespeed;
    GameObject projectile;


    [SerializeField] GameObject line;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject handle;

    [SerializeField] TextMeshProUGUI healthtext;

    float chasingspeed;

    float backawayTimer;
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
    public AttackPattern return_attackptn()
    {
        return atkPattern;
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

    GameObject gamemanager;
    void Start()
    {

        gamemanager = GameObject.FindGameObjectWithTag("GameManager");

        projectile_shots = 0;
        interval_between_shots = 0.0f;

        transitionFromAttackTimer = 0.0f;
        phase3timer = 0.0f;
        chasingspeed = 0.0f;
        backawayTimer = 0.0f;
        proejctilespeed = 2.0f;
        post_attack_duration = 0.0f;
        targetVelocity = 1.5f;
        numberOfRays = 30;
        angle = 90.0f;
        rayRange = 1.0f;

        updating = false;
        zone = GameObject.FindGameObjectsWithTag("Zone");
        hitbox = GameObject.FindGameObjectWithTag("Attack");
        player = GameObject.FindGameObjectWithTag("Player");
        transitionFromHurtTimer = 0.0f;

        currentHealth = EnemyHealth;
        healthbar.maxValue = currentHealth;
        healthbar.minValue = 0;

        attackhitbox.GetComponent<BoxCollider>().enabled = false ;
       
        BoundaryCheck();

        setzoneno = zoneno;

        timer = 0.0f;

        ray_distances = new List<float>();
        ray_distance_n_direction = new Dictionary<float, Vector3>();

        //if (enemy_type == EnemyType.JUMPER)
        //{
        //    GetComponent<JumperScript>().enabled = false;
        //}
        //if (enemy_type == EnemyType.CHARGER)
        //{
        //    GetComponent<ChargerScript>().enabled = false;
        //}
        //if (enemy_type == EnemyType.CHASER)
        //{
        //    GetComponent<ChaserScript>().enabled = false;
        //}

        shootTimer = 0.0f;
        drawdivider();
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
            currentHealth -= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().GetPlayerAttack();

            //ADD CONSECUTIVE HITS
            func();
            //

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

            //Debug.Log("Enemy Health Left: " + EnemyHealth);

            //SET THE ENEMY BACK TO IDLE MODE
            switch (enemy_type)
            {
                case EnemyType.JUMPER:
                    {
                        GetComponentInChildren<Animator>().SetBool("jump", false);
                        GetComponentInChildren<SpriteRenderer>().transform.position 
                            = transform.position + new Vector3(0.0f, 0.66f, 0.0f);
                        canvas.transform.position = transform.position + new Vector3(0.0f, 0.66f, 0.0f);
                        break;
                    }
                case EnemyType.CHASER:
                    {
                        GetComponentInChildren<Animator>().SetBool("about2attack", false);
                        GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
                        GetComponentInChildren<Animator>().SetBool("attack", false);
                        break;
                    }
                case EnemyType.CHARGER:
                    {
                        GetComponentInChildren<Animator>().SetBool("charge", false);
                        GetComponentInChildren<Animator>().SetBool("about2charge", false);

                        GetComponent<Rigidbody>().velocity = -GetComponent<Rigidbody>().velocity * 7.0f;
                       
                        break;
                    }
            }
            //

            transitionFromHurtTimer = 0.0f;

            //play attacked animation
            GetComponentInChildren<Animator>().SetBool("attacked", true);
            //

            gamemanager.GetComponent<EnemyManager>().setupdating(false);

            Iframe = true;

            //Debug.Log("HIT");
        }
    }


    public void func()
    {
        //for (int i = 0; i < 2; i++)
        //{
            player.GetComponent<PlayerStats>().addConsecutiveHit();
            player.GetComponent<PlayerStats>().resetCombo_timer();
            
        //}
    }
    void drawdivider()
    {
        if (healthbar != null)
        {
            for (int i = 0; i < EnemyHealth;)
            {
                if (i < EnemyHealth - 1)
                {
                    healthbar.value += 1;
                    GameObject l = Instantiate(line,
                           new Vector3(0, 0, 0),
                           Quaternion.Euler(0, 0, 0)
                           );
                    l.transform.SetParent(healthbar.transform);
                    l.transform.localPosition =
                        new Vector3(0, healthbar.handleRect.localPosition.y, 0.0f);
                    l.GetComponent<RectTransform>().sizeDelta =
                        new Vector2(healthbar.GetComponent<RectTransform>().rect.width
                        * canvas.GetComponent<RectTransform>().lossyScale.x,
                            .015f);
                    i++;
                }
                else
                {
                    handle.SetActive(false);
                    break;
                }
            }

            healthbar.value = EnemyHealth;
        }
    }


    private void Update()
    {

        //Debug.Log(setzoneno);
        gamemanager = GameObject.FindGameObjectWithTag("GameManager");

        hitbox = GameObject.FindGameObjectWithTag("Attack");
        player = GameObject.FindGameObjectWithTag("Player");

        currentAnimationLength = GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length;
        //healthtext.text = EnemyHealth.ToString();

        GetComponentInChildren<Animator>().SetFloat("health", currentHealth);
        healthbar.value = currentHealth;

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

        //POST ATTACK
        if (GetComponentInChildren<EnemyAttack>().getpostattack())
        {
            post_attack_duration += Time.deltaTime;
            if (post_attack_duration >= 8.0f)
            {
                post_attack_duration = 0.0f;
                GetComponentInChildren<EnemyAttack>().setpostattack(false);
            }
        }
        //

        zone = GameObject.FindGameObjectsWithTag("Zone");
        BoundaryCheck();
        //Debug.Log(zoneno);
        if (zoneno == player.GetComponent<PlayerZoneCheck>().getZoneno())
        {
            updating = true;

            if (phase == Phases.AVOID)
            {
                GetComponent<NavMeshAgent>().enabled = true;
                if (enemy_type == EnemyType.CHARGER)
                {
                    GetComponent<ChargerScript>().DestroyBeams();
                }

                if(enemy_type == EnemyType.JUMPER)
                {
                    GetComponentInChildren<SpriteRenderer>().transform.position = transform.position + new Vector3(0.0f, 0.66f, 0.0f);

                }

                //GetComponentInChildren<SpriteRenderer>().color = Color.black;
                //Debug.Log("AVOID");
                GetComponentInChildren<Animator>().SetBool("chasingPlayer", true);
                GetComponent<NavMeshAgent>().speed = 2.0f;
                GetComponent<NavMeshAgent>().SetDestination(new Vector3(
                player.transform.position.x + rand_x,
                transform.position.y,
                player.transform.position.z + rand_y
                ));
            }
            else
            {
                //GetComponentInChildren<SpriteRenderer>().color = Color.white;

                if(enemy_type == EnemyType.CHARGER)
                {
                    GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
                }

                //gamemanager.GetComponent<EnemyManager>().setupdating(true);
                int rand_range_x = Random.Range(0, 11);
                int rand_range_y = Random.Range(0, 11);

                if (rand_range_x % 2 == 0)
                {
                    rand_x = Random.Range(-7, -6);
                }
                else
                {
                    rand_x = Random.Range(5, 7);
                }

                if (rand_range_y % 2 == 0)
                {
                    rand_y = Random.Range(-7, -6);
                }
                else
                {
                    rand_y = Random.Range(5, 7);
                }
            }


            if (GetComponentInChildren<Animator>().GetBool("attacked") == false)
            {
                if (phase == Phases.ATTACK_TYPE_3)
                {
                    //Debug.Log("SHOOTER");
                    phase3timer += Time.deltaTime;

                    attackhitbox.GetComponent<BoxCollider>().enabled = false;
                    float dist = Vector3.Distance(transform.position, player.transform.position);
                    GetComponentInChildren<NavMeshAgent>().speed = chasingspeed;
                    GetComponentInChildren<NavMeshAgent>().acceleration = chasingspeed;

                    //continue to chase the player
                    //else
                    //{
                    if (dist <= 5.0f
                        && shootTimer >= 2)
                    {
                        chasingspeed = 0;
                        GetComponentInChildren<NavMeshAgent>().enabled = false;
                        Vector3 resultingVector = -player.transform.position + transform.position;
                        resultingVector.y = 0;
                        GetComponent<Rigidbody>().velocity = resultingVector * .5f;
                    }
                    else
                    {
                        GetComponentInChildren<NavMeshAgent>().enabled = true;
                        //GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                        GetComponentInChildren<NavMeshAgent>().SetDestination(player.transform.position);
                        chasingspeed = dist;
                    }

                    //GetComponentInChildren<NavMeshAgent>().enabled = true;
                    //GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
                    //GetComponentInChildren<NavMeshAgent>().SetDestination(player.transform.position);
                    GetComponentInChildren<Animator>().SetBool("chasingPlayer", true);

                    //shoot every interval
                    shootTimer += Time.deltaTime;
                    if (shootTimer >= 2)
                    {
                        //GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
                        GetComponentInChildren<SpriteRenderer>().color = Color.red;

                        if (shootTimer >= 3.0f)
                        {
                            //play attack animation
                            GetComponentInChildren<Animator>().SetBool("attack", true);
                            //
                            GetComponentInChildren<SpriteRenderer>().color = Color.white;
                            //shoot();
                        }
                    }
                    //}
                    //

                    if (GetComponentInChildren<Animator>().GetBool("attack") &&
                        GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack"))
                    {
                        transitionFromAttackTimer += Time.deltaTime;

                        if (projectile_shots == 0)
                        {
                            shoot();
                        }

                        //GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f
                        if (transitionFromAttackTimer >=
                        currentAnimationLength)
                            //if (
                            //GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                        {
                            if (projectile_shots >= projectile_numbers)
                            {
                                shootTimer = 0.0f;
                                projectile_shots = 0;
                                GetComponentInChildren<Animator>().SetBool("attack", false);
                                GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
                            }
                            else
                            {
                                shoot();
                            }
                            //Debug.Log("SHOOT");
                            transitionFromAttackTimer = 0.0f;
                        }
                    }

                    if (phase3timer >= 20.0f)
                    {
                        phase = Phases.COOLDOWN;
                        gamemanager.GetComponent<EnemyManager>().setupdating(false);
                    }
                }
                
            }
           
            else
            {
                shootTimer = 0.0f;
            }

            //if (GetComponent<EnemyScript>().getzoneno() == 0)
            //{
            //    Vector3 resultingVector = GetComponent<EnemyScript>().getparent().position - transform.position;
            //    resultingVector.y = 0;
            //    resultingVector.Normalize();
            //    GetComponent<Rigidbody>().velocity = resultingVector;
            //}

        }
        else if(zoneno != player.GetComponent<PlayerZoneCheck>().getZoneno()
            || zoneno != setzoneno)
        {

            updating = false;
        }


        if (updating == false)
        {
            //GetComponent<NavMeshAgent>().enabled = false;
            GetComponentInChildren<SpriteRenderer>().color = Color.white;

            if (enemy_type == EnemyType.CHARGER)
            {
                GetComponent<ChargerScript>().DestroyBeams();
            }

            Vector3 resultingVector = GetComponent<EnemyScript>().getparent().position - transform.position;
            resultingVector.y = 0;
            resultingVector.Normalize();
            GetComponent<Rigidbody>().velocity = resultingVector;
            //Debug.Log("GO BACK");

            //if all enemies are not updating
            if (!gamemanager.GetComponent<EnemyManager>().getEnemy_update())
            {
                gamemanager.GetComponent<EnemyManager>().setupdating(false);
            }
            //

            timer = 0;
            phase = Phases.COOLDOWN;
        }

        //if player left the zone
        if(player.GetComponent<PlayerZoneCheck>().getZoneno() == 0)
        {
            FirstAttack = false;
            AttackByOtherWeapon = false;
            currentHealth = EnemyHealth;
        }
        //

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

        transform.LookAt(player.transform.position);
        GetComponentInChildren<SpriteRenderer>().transform.rotation = Quaternion.Euler(0, 0, 0);

        //back away while one of the enemies is in attack mode
        //other_enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //foreach (GameObject enemies in other_enemies)
        //{
        //    if (enemies.GetComponent<EnemyScript>().getzoneno() == getzoneno())
        //    {
        //        if (GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack")
        //            || GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("about2attack"))
        //        {
        //            if (enemies.GetComponent<EnemyScript>().return_enemyType() == EnemyType.CHASER
        //            && enemies.GetComponent<EnemyScript>().return_attackptn() != AttackPattern.PATTERN_3)
        //            {
        //                //Debug.Log("AVOID");
        //                enemies.GetComponent<EnemyScript>().avoidanceCode(3);
        //                    break;
        //            }
        //        }



        //        //IF ENEMY IS JUMPER
        //        if (enemies.GetComponent<EnemyScript>().return_enemyType() == EnemyType.JUMPER
        //            && enemies.GetComponent<EnemyScript>().return_attackptn() != AttackPattern.PATTERN_3)
        //        {
        //            if (/*enemies.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("about2jump")
        //                &&*/ GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("about2jump"))
        //            {
        //                enemies.GetComponent<EnemyScript>().avoidanceCode(3);
        //            }
        //        }
        //        //

        //        //IF ENEMY IS CHARGER
        //        if (enemies.GetComponent<EnemyScript>().return_enemyType() == EnemyType.CHARGER
        //            && enemies.GetComponent<EnemyScript>().return_attackptn() != AttackPattern.PATTERN_3)
        //        {
        //            if (enemies.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("about2charge")
        //                && GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("about2charge"))
        //            {
        //                enemies.GetComponent<EnemyScript>().avoidanceCode(3);
        //                break;

        //            }
        //        }
        //        //

        //        if (GetComponent<EnemyScript>().return_attackptn() == AttackPattern.PATTERN_3
        //            && enemies.GetComponent<EnemyScript>().return_attackptn() == AttackPattern.PATTERN_3)
        //        {
        //            if (enemies.GetComponent<EnemyScript>().getst() > 2)
        //            {
        //                enemies.GetComponent<EnemyScript>().avoidanceCode(3);
        //                break;
        //            }
        //        }
        //    }
        //    break;
        //}

    }

    public float getst()
    {
        return shootTimer;
    }

    public float getCurrentAnimationLength()
    {
        return currentAnimationLength;
    }

    public void shoot()
    {
        Vector3 resultingVector = player.transform.position - transform.position;
        resultingVector.y = 0;

        //if (projectile_shots == 0)
        //{
            //GetComponentInChildren<Animator>().SetBool("attack", true);
            projectile = Instantiate(projectileGO,
            new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z),
            Quaternion.Euler(0, 0, 0));
            projectile.GetComponent<Rigidbody>().velocity = resultingVector * proejctilespeed;
            projectile_shots += 1;
            //Debug.Log("SHOOT");
        //}

        /*if (interval_between_shots > 1.0f
            && transitionFromAttackTimer >= currentAnimationLength)
        {
            GetComponentInChildren<Animator>().SetBool("attack", true);
            projectile = Instantiate(projectileGO,
            new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z),
            Quaternion.Euler(0, 0, 0));
            projectile.GetComponent<Rigidbody>().velocity = resultingVector * proejctilespeed;
            projectile_shots += 1;
            Debug.Log("SHOOT2");
        }*/

        
    }

    public void BoundaryCheck()
    {
        for (int i = 0; i < zone.Length; i++)
        {
            if (transform.position.x < zone[i].GetComponent<Transform>().position.x + (zone[i].GetComponent<Transform>().lossyScale.x / 2)
             && transform.position.x > zone[i].GetComponent<Transform>().position.x - (zone[i].GetComponent<Transform>().lossyScale.x / 2)
             && transform.position.z > zone[i].GetComponent<Transform>().position.z - (zone[i].GetComponent<Transform>().lossyScale.z / 2)
            && transform.position.z < zone[i].GetComponent<Transform>().position.z + (zone[i].GetComponent<Transform>().lossyScale.z / 2)
             )

            {
                zoneno = zone[i].GetComponent<WhatZone>().return_zonenumber();
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
        //GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        attackhitbox.GetComponent<BoxCollider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = true;

        //Debug.Log("UPDATING FALSE");

        //if (GetComponent<NavMeshAgent>().enabled == true)
        //{
            GetComponent<NavMeshAgent>().speed = 5.0f;
            GetComponent<NavMeshAgent>().SetDestination(getparent().position);
        //}

        GetComponentInChildren<Canvas>().transform.localPosition = new Vector3(0, 0, 0);
        if (enemy_type == EnemyType.JUMPER)
        {
            GetComponentInChildren<SpriteRenderer>().transform.localPosition = new Vector3(0, 0.66f, 0);
        }
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
            if (Physics.Raycast(ray, rayRange, lm))
            {
                deltaPosition -= (1.0f / numberOfRays) * targetVelocity * direction;
                transform.position += deltaPosition * Time.deltaTime;
                Debug.Log("HIT ENEMY");
            }
            else if (Physics.Raycast(ray2, rayRange, lm))
            {
                deltaPosition -= (1.0f / numberOfRays) * targetVelocity * direction2;
                transform.position += deltaPosition * Time.deltaTime;
                Debug.Log("HIT ENEMY");
            }
        }
    }
    public void steering_3()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);


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
            if (Physics.Raycast(ray, dist
                , player_lm))
            {
                deltaPosition -= (1.0f / numberOfRays) * targetVelocity * direction;
                transform.position += deltaPosition * Time.deltaTime;
            }
            else if (Physics.Raycast(ray2, dist
                , player_lm))
            {
                deltaPosition -= (1.0f / numberOfRays) * targetVelocity * direction2;
                transform.position += deltaPosition * Time.deltaTime;
            }
        }
    }
    public void steering_2()
    {
        GetComponent<NavMeshAgent>().enabled = false;

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

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 50.0f, lm_2))
            {
                //first array = magnitude, second array = direction
                //find the most open area without the player/enemy blocking

                if (hitInfo.transform.gameObject.layer
                    != LayerMask.NameToLayer("Player")
                    && hitInfo.transform.gameObject.layer
                    != LayerMask.NameToLayer("Enemy"))
                {
                    if (!ray_distance_n_direction.ContainsKey(hitInfo.distance))
                    {
                        ray_distance_n_direction.Add(hitInfo.distance, ray.direction);
                    }
                    else
                    {
                        ray_distance_n_direction[hitInfo.distance] = ray.direction;
                    }
                    //Debug.Log("I HIT SOMETHING");
                    ray_distances.Add(hitInfo.distance);
                }

                if (hitInfo.transform.gameObject.layer
                   == LayerMask.NameToLayer("Player"))
                {
                    Debug.Log("HIT PLAYER");
                }
            }

            if (Physics.Raycast(ray2, out hitInfo, 50.0f, lm_2))
            {
                //first array = magnitude, second array = direction
                //find the most open area without the player/enemy blocking
                if (hitInfo.transform.gameObject.layer
                    != LayerMask.NameToLayer("Player")
                    && hitInfo.transform.gameObject.layer
                    != LayerMask.NameToLayer("Enemy"))
                {
                    if (!ray_distance_n_direction.ContainsKey(hitInfo.distance))
                    {
                        ray_distance_n_direction.Add(hitInfo.distance, ray2.direction);
                    }
                    else
                    {
                        ray_distance_n_direction[hitInfo.distance] = ray2.direction;
                    }
                    //Debug.Log("I HIT SOMETHING");
                    ray_distances.Add(hitInfo.distance);
                }

                if (hitInfo.transform.gameObject.layer
                   == LayerMask.NameToLayer("Player"))
                {
                    Debug.Log("HIT PLAYER");
                }
            }
        }


        ray_distances.Sort();
        furthestdistance = ray_distances[ray_distances.Count - 1];
        //get the direction using the key, which is the furthestdistance
        Vector3 resultingVector =  transform.position - (furthestdistance *
            ray_distance_n_direction[furthestdistance]);
        resultingVector.y = 0;
        //normalise resulting vector
        resultingVector.Normalize();
        GetComponent<Rigidbody>().velocity = resultingVector * 5.0f;
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



    public void cooldownUpdate()
    {
        GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
        
        shootTimer = 0.0f;
        phase3timer = 0.0f;
        //turn off attack hitbox
        attackhitbox.GetComponent<BoxCollider>().enabled = false;
        timer += 1.0f * Time.deltaTime;
        GetComponent<BoxCollider>().enabled = true;
        if (timer >= cooldown_period)
        {
            timer = 0.0f;
            rand_gen = Random.Range(1, 5);
            phase = Phases.ABOUT_TO_ATTACK;
        }
    }
    public void abouttoattackUpdate()
    {
        GetComponent<BoxCollider>().enabled = true;
        if (enemy_type != EnemyType.CHARGER)
        {
            GetComponentInChildren<Animator>().SetBool("chasingPlayer", true);
        }
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        attackhitbox.GetComponent<BoxCollider>().enabled = false;

     
        if (updating)
        {
            timer += Time.deltaTime;
        }
        if (timer >= abouttoattack_period)
        {
            //attack_type = Random.Range(1, 3);
            if (atkPattern == AttackPattern.PATTERN_1)
            {
                phase = Phases.ATTACK_TYPE_1;
            }
            else if (atkPattern == AttackPattern.PATTERN_2)
            {
                phase = Phases.ATTACK_TYPE_2;
            }
            else if (atkPattern == AttackPattern.PATTERN_3)
            {
                phase = Phases.ATTACK_TYPE_3;
            }
            timer = 0.0f;
        }
    }

    

    public void avoidanceCode(float random_number_z)
    {
        Vector3 resultingVector = -player.transform.position + transform.position;
        resultingVector.y = 0;
        GetComponent<Rigidbody>().velocity = resultingVector + new Vector3(random_number_z, 0, 0);
    }


    public void avoidanceCode_2(float random_number_x, float random_number_z)
    {
        float x = random_number_x;
        float z = random_number_z;

        GetComponent<NavMeshAgent>().SetDestination(new Vector3(player.transform.position.x + x,
                 transform.position.y,
                 player.transform.position.z + z));
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
            GameObject temp = Instantiate(Mush, gameObject.transform.position, Quaternion.identity);
            temp.transform.SetParent(GameObject.FindGameObjectWithTag("Drops").transform);
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
            tempobj.transform.SetParent(GameObject.FindGameObjectWithTag("Drops").transform);

            // if it is exact kill
            if (ExactKill)
            {
                tempobj.GetComponent<Food>().SetPerfect(true);
            }
        }
      
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        Destroy(gameObject);
    }

    public void Death()
    {
        attackhitbox.GetComponent<BoxCollider>().enabled = false;

        //getparent().GetComponent<Spawner>().resetSpawnerTimer(5, false);

        if (currentHealth == 0)
        {
            Debug.Log("Precise Kill!");
            EnemyDie(true);
        }
        else if (currentHealth < 0)
        {
            Debug.Log("Killed!");
            EnemyDie(false);
        }
    }

    public int GetEnemyHealth()
    {
        return currentHealth;
    }

    public void SetEnemyHealth(int enemyHealth)
    {
        EnemyHealth = enemyHealth;
        currentHealth = EnemyHealth;
    }
}
