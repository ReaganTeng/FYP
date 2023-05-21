using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using Com.LuisPedroFonseca.ProCamera2D;

public class EnemyScript : MonoBehaviour
{

    //UNUSED CODE
    List<float> ray_distances;
    float targetVelocity;
    int numberOfRays;
    float angle;
    float rayRange;
    [SerializeField] LayerMask lm;
    [SerializeField] LayerMask lm_2;
    [SerializeField] LayerMask player_lm;
    //
    Dictionary<float, Vector3> ray_distance_n_direction;
    float furthestdistance;
    //


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


    float currentAnimationLength;

    bool attacked;


    public enum Phases
    {
        ABOUT_TO_ATTACK,
        COOLDOWN,
        ATTACK_TYPE_1,
        ATTACK_TYPE_2,
        AVOID,
        TOTAL
    }


    public enum EnemyType
    {
        JUMPER,
        CHARGER,
        CHASER,
        SHOOTER,
        RANGER,
        TOTAL
    }


    public enum AttackPattern
    {
        PATTERN_1,
        PATTERN_2,

        TOTAL
    }
    [SerializeField] AttackPattern atkPattern;


    [SerializeField] Phases phase;
    EnemyType enemy_type;

    Transform spawnerparent;


    [SerializeField] GameObject line;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject handle;


    float chasingspeed;

    float new_destination_interval;
    float offset_x;
    float offset_z;

    public void setchasingspeed(float cs)
    {
        chasingspeed = cs;
    }
    public float getchasingspeed()
    {
        return chasingspeed;
    }


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
        attacked = false;
        offset_z = 0;
        offset_x = 0;
        new_destination_interval = 0.0f;
        //particles.SetActive(false);
        currentHealth = EnemyHealth;

        gamemanager = GameObject.FindGameObjectWithTag("GameManager");

        transitionFromAttackTimer = 0.0f;
        chasingspeed = 0.0f;
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
        healthbar.maxValue = currentHealth;
        healthbar.minValue = 0;
        attackhitbox.GetComponent<BoxCollider>().enabled = false;
        BoundaryCheck();
        setzoneno = zoneno;
        timer = 0.0f;



        ray_distances = new List<float>();
        ray_distance_n_direction = new Dictionary<float, Vector3>();

        drawdivider();
    }
    public void set_newdestination()
    {
        int rand_range_x = Random.Range(0, 11);
        int rand_range_y = Random.Range(0, 11);

        if (rand_range_x % 2 == 0)
        {
            rand_x = Random.Range(-3, 0);
        }
        else
        {
            rand_x = Random.Range(1, 4);
        }

        if (rand_range_y % 2 == 0)
        {
            rand_y = Random.Range(-3, 0);
        }
        else
        {
            rand_y = Random.Range(1, 4);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Floor"))
            return;

        // if the object calling this is not the base enemy, ignore
        if (!gameObject.CompareTag("Enemy"))
            return;

        // If its from player attack
        if (
            other.CompareTag("Attack") && !Iframe
            && GetComponent<BoxCollider>().enabled == true
            && !other.GetComponentInParent<PlayerAttack>().getalready_attacked()
            //&& other.GetComponent<BoxCollider>().enabled == true
            //&& (!GetComponentInChildren<Animator>().GetBool("jump") && enemy_type == EnemyType.JUMPER)
            )
        {
            currentHealth -= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().GetPlayerAttack();
            //currentHealth -= 999;

            //JUST DIE ALREADY POWERUP
            if (other.GetComponentInParent<PlayerAttack>().getheavyattacked()
                && player.GetComponent<PlayerStats>().getinstantkillmode())
            {
                player.GetComponent<PlayerStats>().reducefervor();
                currentHealth -= 999;
            }
            //

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
                        GetComponent<ChargerScript>().DestroyBeams();

                        GetComponent<Rigidbody>().velocity = -GetComponent<Rigidbody>().velocity * 7.0f;
                       
                        break;
                    }
                case EnemyType.SHOOTER:
                    {
                        GetComponentInChildren<Animator>().SetBool("about2shoot", false);
                        GetComponentInChildren<Animator>().SetBool("attack", false);
                        GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);

                        break;
                    }
            }
            //

           

            transitionFromHurtTimer = 0.0f;
            //play attacked animation
            GetComponentInChildren<Animator>().SetBool("attacked", true);
            //
            gamemanager.GetComponent<EnemyManager>().setupdating(false);
            attacked = true;
            ProCamera2DShake.Instance.ShakeUsingPreset("HitShake");

            Iframe = true;
        }
    }

    public void setbool(bool attk)
    {
        attacked = attk;
    }

    public bool getbool()
    {
        return attacked;
    }
    public void func()
    {
        player.GetComponent<PlayerStats>().addConsecutiveHit();
        player.GetComponent<PlayerStats>().resetCombo_timer();
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

    bool side_checking(float a, float b)
    {
        if ((int)a == (int)b)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public GameObject returnhitbox()
    {
        return attackhitbox;
    }


    public void set_transitionfromattacktimer(float time)
    {
        transitionFromAttackTimer = time;
    }
    public void add_transitionfromattacktimer(float time)
    {
        transitionFromAttackTimer += time;
    }
    public float return_transitionfromatktimer()
    {
        return transitionFromAttackTimer;
    }


    public float return_transitionfromhurttimer()
    {
        return transitionFromHurtTimer;
    }

    private void Update()
    {

        //Debug.Log(setzoneno);
        //GetComponent<NavMeshAgent>().enabled = false;

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


        //flip the sprite
        Vector3 mypos = transform.position;
        var targetPos = player.transform.position;
        if (player.transform.position.x < mypos.x && side_checking(mypos.y, targetPos.y))
        {
            //print("Left");
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        else if (side_checking(mypos.y, targetPos.y))
        {
            //print("Right");
            //flip the sprite
            GetComponentInChildren<SpriteRenderer>().flipX = true;

        }
        //

        //turn purple when get sus dish
        if (AttackByOtherWeapon)
        {
            healthbar.GetComponentInChildren<Image>().color = new Color32(160, 32, 240, 100);// Color.blue;
        }
        else
        {
            healthbar.GetComponentInChildren<Image>().color = Color.white;

        }


        //POST ATTACK
        if (GetComponentInChildren<EnemyAttack>().getpostattack())
        {
            post_attack_duration += Time.deltaTime;
            if (post_attack_duration >= 3.0f)
            {
                post_attack_duration = 0.0f;
                GetComponentInChildren<EnemyAttack>().setpostattack(false);
            }
        }
        //

        BoundaryCheck();
        if (zoneno == player.GetComponent<PlayerZoneCheck>().getZoneno())
        {
            updating = true;

            if (phase == Phases.AVOID
                || (enemy_type == EnemyType.CHASER &&
                (GetComponentInChildren<EnemyAttack>().getpostattack()
                            || GetComponent<ChaserScript>().return_change_of_attk_type_1() >= 5.0f))
                )
            {
                attackhitbox.GetComponent<BoxCollider>().enabled = true;

                //GetComponentInChildren<SpriteRenderer>().color = Color.black;
                new_destination_interval += Time.deltaTime;
                if (new_destination_interval >= 3.0f)
                {
                    //set_newdestination();
                    offset_x = Random.Range(-7, 8);
                    offset_z = Random.Range(-7, 8);
                    new_destination_interval = 0;
                }

                GetComponent<NavMeshAgent>().enabled = true;
                if (enemy_type == EnemyType.CHARGER)
                {
                    GetComponent<ChargerScript>().DestroyBeams();
                }

                if (enemy_type == EnemyType.RANGER)
                {
                    GetComponent<RangerScript>().DestroyBeams();
                }

                if (enemy_type == EnemyType.JUMPER)
                {
                    GetComponentInChildren<SpriteRenderer>().transform.position = transform.position + new Vector3(0.0f, 0.66f, 0.0f);
                }

                GetComponentInChildren<Animator>().SetBool("chasingPlayer", true);
                GetComponent<NavMeshAgent>().speed = 2.0f;
                GetComponent<NavMeshAgent>().SetDestination(new Vector3(
                player.transform.position.x + rand_x + (int)offset_x,
                transform.position.y,
                player.transform.position.z + rand_y + (int)offset_z
                ));
            }
            else
            {
                //GetComponent<NavMeshAgent>().enabled = true;
                //GetComponentInChildren<SpriteRenderer>().color = Color.white;
                new_destination_interval = 0;
                offset_x = 0;
                offset_z = 0;
                if (enemy_type == EnemyType.CHARGER)
                {
                    GetComponentInChildren<Animator>().SetBool("chasingPlayer", false);
                }
                set_newdestination();
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


        //IF ENEMY IS HURT
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
        //

        transform.LookAt(player.transform.position);
        GetComponentInChildren<SpriteRenderer>().transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    

    public float getCurrentAnimationLength()
    {
        return currentAnimationLength;
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
            if (enemy_type == EnemyType.CHASER)
            {
                GetComponent<NavMeshAgent>().enabled = true;
            }

            //attack_type = Random.Range(1, 3);
            if (atkPattern == AttackPattern.PATTERN_1)
            {
                phase = Phases.ATTACK_TYPE_1;
            }
            else if (atkPattern == AttackPattern.PATTERN_2)
            {
                phase = Phases.ATTACK_TYPE_2;
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

            //RATION POWER UP
            player.GetComponentInChildren<PlayerAttack>().addenemykilled();
            if (player.GetComponentInChildren<PlayerAttack>().getenemykilled() >= player.GetComponentInChildren<PlayerStats>().getpp().return_enemykilledrequirement()
                && player.GetComponentInChildren<PlayerStats>().getpp().return_enemykilledrequirement() > 0)
            {
                //Debug.Log("RATIONED");
                GameObject additoional_drop = Instantiate(whichobject, gameObject.transform.position, Quaternion.identity);
                additoional_drop.transform.SetParent(GameObject.FindGameObjectWithTag("Drops").transform);
                additoional_drop.GetComponent<Food>().SetPerfect(false);
                player.GetComponentInChildren<PlayerAttack>().resetenemykilled();
            }
            //

            // if it is exact kill
            if (ExactKill)
            {
                tempobj.GetComponent<Food>().SetPerfect(true);
            }
        }



        GetComponentInChildren<SpriteRenderer>().enabled = false;
        Destroy(gameObject);
        gamemanager.GetComponent<EnemyManager>().recalculate_numberofenemies();
    }

    public void Death()
    {
        attackhitbox.GetComponent<BoxCollider>().enabled = false;

        if (currentHealth == 0)
        {
            Debug.Log("Precise Kill!");
            //particles.SetActive(true);
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
        Vector3 resultingVector = transform.position - (furthestdistance *
            ray_distance_n_direction[furthestdistance]);
        resultingVector.y = 0;
        //normalise resulting vector
        resultingVector.Normalize();
        GetComponent<Rigidbody>().velocity = resultingVector * 5.0f;
    }
}
