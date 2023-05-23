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
    /*List<float> ray_distances;
    float targetVelocity;
    int numberOfRays;
    float angle;
    float rayRange;
    [SerializeField] LayerMask lm;
    [SerializeField] LayerMask lm_2;
    [SerializeField] LayerMask player_lm;
    //
    Dictionary<float, Vector3> ray_distance_n_direction;
    float furthestdistance;*/
    //

    // THE BASE HEALTH OF THE ENEMY
    [SerializeField] int EnemyHealth; 
    //THE CURRENT HEALTH OF THE ENEMY
    int currentHealth;

    //THE DIFFERENT STATES OF THE INGREDIENTS THE ENEMY WILL DROP
    [SerializeField] GameObject prepPrefab;
    [SerializeField] GameObject choppedPefab;
    [SerializeField] GameObject smashedPrefab;
    [SerializeField] GameObject Mush;
    //


    float Iframemaxtime = 0.1f;
    float Iframetimer = 0.1f;
    bool Iframe = false;
    
    bool FirstAttack = false;
    
    int AttackByWhatWeapon = 0;
    bool AttackByOtherWeapon = false;


    float timer;
    [SerializeField] float cooldown_period;
    [SerializeField] float abouttoattack_period;

    float transitionFromHurtTimer;
    float transitionFromAttackTimer;


    bool updating;

    //THE CURRENT ZONE THE PLAYER IS IN
    //THE DESIGNATED ZONE THE ENEMY IS SPAWNED IN
    int setzoneno;
    int zoneno;
    GameObject[] zone;

    float currentAnimationLength;

    //WHETHER THE ENEMY IS ATTACKED BY THE PLAYER
    bool attacked;


    //THE CURRENT PHASE THE ENEMY CURRENTLY IS IN
    [SerializeField] Phases phase;
    public enum Phases
    {
        ABOUT_TO_ATTACK,
        COOLDOWN,
        ATTACK_TYPE_1,
        ATTACK_TYPE_2,
        AVOID,
        TOTAL
    }


    //THE TYPE OF ENEMY
    EnemyType enemy_type;
    public enum EnemyType
    {
        JUMPER,
        CHARGER,
        CHASER,
        SHOOTER,
        RANGER,
        TOTAL
    }


    //THE ATTACK PATTERN THE ENEMY WILL PERFORM, WHICH DECIDES WHAT TYPE OF ATTACK TYPE THE ENEMY WILL GO TO
    [SerializeField] AttackPattern atkPattern;
    public enum AttackPattern
    {
        PATTERN_1,
        PATTERN_2,

        TOTAL
    }



    //THE ENEMY'S SPAWNER
    Transform spawnerparent;
    

    //PROPERTIES OF THE HEALTH BAR
    //HEALTH BAR SLIDER
    [SerializeField] Slider healthbar;
    //LINE PREFAB
    [SerializeField] GameObject line;
    //THE CANVAS FOR THE HEALTH BAR
    [SerializeField] Canvas canvas;
    //THE HANDLE FOR THE HEALTH BAR SLIDER
    [SerializeField] GameObject handle;
    //


    //THE SPEED OF THE NAVMESH
    float navmeshspeed;

    float new_destination_interval;
    //HOW FAR YOU WANT THE NAVMESH DESTINATION TO BE FROM THE PLAYER BY THE X AXIS AT EVERY INTERVAL
    float offset_x;
    //HOW FAR YOU WANT THE NAVMESH DESTINATION TO BE FROM THE PLAYER BY THE Z AXIS AT EVERY INTERVAL
    float offset_z;
    //HOW FAR YOU WANT THE NAVMESH DESTINATION TO BE FROM THE PLAYER BY THE X AXIS
    float rand_x;
    //HOW FAR YOU WANT THE NAVMESH DESTINATION TO BE FROM THE PLAYER BY THE Z AXIS
    float rand_y;



    //ENEMY'S HITBOX
    [SerializeField] GameObject hitbox;
    //THE PLAYER PREFAB
    GameObject player;
    //THE ENEMY MANAGER SCRIPT IN GAME MANAGER PREFAB
    EnemyManager em;
    //ENEMY'S NAVMESHAGENT
    NavMeshAgent navmeshagent;
    //ENEMY'S ANIMATION CONTROLLER
    Animator anim;

    public void setnavmeshspeed(float cs)
    {
        navmeshspeed = cs;
    }
    public float getnavmeshspeed()
    {
        return navmeshspeed;
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


    void Start()
    {
        attacked = false;
        offset_z = 0;
        offset_x = 0;
        new_destination_interval = 0.0f;
        currentHealth = EnemyHealth;


        transitionFromAttackTimer = 0.0f;
        navmeshspeed = 0.0f;


        /*targetVelocity = 1.5f;
        numberOfRays = 30;
        angle = 90.0f;
        rayRange = 1.0f;
        ray_distances = new List<float>();
        ray_distance_n_direction = new Dictionary<float, Vector3>();*/

        updating = false;
        zone = GameObject.FindGameObjectsWithTag("Zone");
        transitionFromHurtTimer = 0.0f;
        healthbar.maxValue = currentHealth;
        healthbar.minValue = 0;
        BoundaryCheck();
        setzoneno = zoneno;
        timer = 0.0f;


        player = GameObject.FindGameObjectWithTag("Player");
        hitbox.GetComponent<BoxCollider>().enabled = false;
        em = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EnemyManager>();
        navmeshagent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();


        

        drawdivider();
    }

    //CALCULATE THE NEW DESTINATION FOR THE ENEMY TO GO NEXT IN AVOID MODE
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
    void OnTriggerEnter(Collider other)
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
            && phase != Phases.COOLDOWN
            /*&& (enemy_type == EnemyType.JUMPER
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("jump")
            && !anim.GetBool("jump"))
             && (enemy_type == EnemyType.CHASER 
            && phase != Phases.ABOUT_TO_ATTACK)*/
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
            player.GetComponent<PlayerStats>().addConsecutiveHit();
            player.GetComponent<PlayerStats>().resetCombo_timer();
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
                        anim.SetBool("jump", false);
                        GetComponentInChildren<SpriteRenderer>().transform.position 
                            = transform.position + new Vector3(0.0f, 0.66f, 0.0f);
                        canvas.transform.position = transform.position + new Vector3(0.0f, 0.66f, 0.0f);
                        break;
                    }
                case EnemyType.CHASER:
                    {
                        anim.SetBool("about2attack", false);
                        anim.SetBool("run", false);
                        anim.SetBool("attack", false);
                        break;
                    }
                case EnemyType.CHARGER:
                    {
                        anim.SetBool("charge", false);
                        anim.SetBool("about2charge", false);
                        GetComponent<ChargerScript>().DestroyBeams();
                        GetComponent<Rigidbody>().velocity = -GetComponent<Rigidbody>().velocity * 7.0f;
                        break;
                    }
                case EnemyType.SHOOTER:
                    {
                        anim.SetBool("about2shoot", false);
                        anim.SetBool("attack", false);
                        anim.SetBool("run", false);
                        break;
                    }
                case EnemyType.RANGER:
                    {
                        anim.SetBool("run", false);
                        anim.SetBool("attack", false);
                        GetComponent<RangerScript>().DestroyBeams();
                        break;
                    }
            }
            //

            transitionFromHurtTimer = 0.0f;
            anim.SetBool("attacked", true);
            em.setupdating(false);
            attacked = true;

          
            ProCamera2DShake.Instance.ShakeUsingPreset("HitShake");

            Iframe = true;
        }
    }

    public void setattacked(bool attk)
    {
        attacked = attk;
    }

    public bool getbool()
    {
        return attacked;
    }
    

    //USE TO INSTANTIATE LINES BASE ON THE NUMBER OF HEALTH THE ENEMY HAS
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
    //


    //CHECK WHICH SIDE PLAYER IS ON RELATIVE TO THE ENEMY, LEFT OR RIGHT
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
    //

    public GameObject returnhitbox()
    {
        return hitbox;
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
        navmeshagent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

       
        BoundaryCheck();


        currentAnimationLength = anim.GetCurrentAnimatorStateInfo(0).length;

        anim.SetFloat("health", currentHealth);
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


        //FLIP SPRITE BASE ON WHICH SIDE THE PLAYER IS IN
        Vector3 mypos = transform.position;
        var targetPos = player.transform.position;
        if (player.transform.position.x < mypos.x && side_checking(mypos.y, targetPos.y))
        {
            //LEFT
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        else if (side_checking(mypos.y, targetPos.y))
        {
            //RIGHT
            GetComponentInChildren<SpriteRenderer>().flipX = true;

        }
        //

        //TURN PURPLE WHEN ENEMY IS ATTACKED BY A DIFFERNT WEAPON THAN PREVIOUS
        if (AttackByOtherWeapon)
        {
            healthbar.GetComponentInChildren<Image>().color = new Color32(160, 32, 240, 100);// Color.blue;
        }
        else
        {
            healthbar.GetComponentInChildren<Image>().color = Color.white;
        }
        //


        //IF ENEMY IS IN THE SAME ZONE AS THE PLAYER, AND CURRENT ZONE THE ENEMY IS IN ITS DESIGNATED ZONE
        if (zoneno == player.GetComponent<PlayerZoneCheck>().getZoneno()
            && zoneno == setzoneno)
        {
            updating = true;

        }
        //IF PLAYER LEFT THE ZONE, OR ENEMY LEFT IT'S DESIGNATED ZONE
        else if(zoneno != player.GetComponent<PlayerZoneCheck>().getZoneno()
            || zoneno != setzoneno)
        {

            updating = false;
        }


        if (updating)
        {
            //IF PLAYER IS IN AVOID STATE
            if (phase == Phases.AVOID)
            {
                hitbox.GetComponent<BoxCollider>().enabled = true;

                //GetComponentInChildren<SpriteRenderer>().color = Color.black;
                //CONSTANTLY SET THE NEWDESTINATION TO BE THE OFFSET OF THE CURRENT DESTINATION BY THE X AND Z AXIS DURING AVOID MODE
                new_destination_interval += Time.deltaTime;
                if (new_destination_interval >= 3.0f)
                {
                    offset_x = Random.Range(-7, 8);
                    offset_z = Random.Range(-7, 8);
                    new_destination_interval = 0;
                }
                //

                navmeshagent.enabled = true;
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

                anim.SetBool("run", true);
                navmeshagent.speed = 2.0f;
                navmeshagent.SetDestination(new Vector3(
                player.transform.position.x + rand_x + (int)offset_x,
                transform.position.y,
                player.transform.position.z + rand_y + (int)offset_z
                ));
            }
            //
            //CONSTANTLY CALCULATE THE NEW DESTINATION FOR THE ENEMY TO GO NEXT IN AVOID MODE
            else
            {
               
                new_destination_interval = 0;
                offset_x = 0;
                offset_z = 0;
                if (enemy_type == EnemyType.CHARGER)
                {
                    anim.SetBool("run", false);
                }
                set_newdestination();
            }
            //
            
        }
        else
        {

            if (enemy_type == EnemyType.CHARGER)
            {
                GetComponent<ChargerScript>().DestroyBeams();
            }

            if (enemy_type == EnemyType.RANGER)
            {
                GetComponent<RangerScript>().DestroyBeams();
            }

            Vector3 resultingVector = GetComponent<EnemyScript>().getparent().position - transform.position;
            resultingVector.y = 0;
            resultingVector.Normalize();
            GetComponent<Rigidbody>().velocity = resultingVector;

            //IF ALL ENEMIES ARE NOT UPDATING
            if (!em.getEnemy_update())
            {
                em.setupdating(false);
            }
            //

            timer = 0;
            phase = Phases.COOLDOWN;
        }

        //IF PLAYER LEFT THE ZONE
        if(player.GetComponent<PlayerZoneCheck>().getZoneno() == 0)
        {
            FirstAttack = false;
            AttackByOtherWeapon = false;
            currentHealth = EnemyHealth;
        }
        //


        //IF ENEMY IS HURT, PLAY HURT ANIMATION
        if (anim.GetBool("attacked"))
        {
            transitionFromHurtTimer += Time.deltaTime;
            if (transitionFromHurtTimer >=
                currentAnimationLength)
            {
                anim.SetBool("attacked", false);

                if (enemy_type == EnemyType.JUMPER)
                {
                    anim.SetBool("about2jump", false);
                    anim.SetBool("run", false);
                }
            }
        }
        //

        transform.LookAt(player.transform.position);
        //ENSURE ENEMY'S SPRITE ALWAYS FACE THE FRONT
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
        hitbox.GetComponent<BoxCollider>().enabled = false;
        navmeshagent.enabled = true;

       
        navmeshagent.speed = 5.0f;
        navmeshagent.SetDestination(getparent().position);
        

        GetComponentInChildren<Canvas>().transform.localPosition = new Vector3(0, 0, 0);
        if (enemy_type == EnemyType.JUMPER)
        {
            GetComponentInChildren<SpriteRenderer>().transform.localPosition = new Vector3(0, 0.66f, 0);
        }
        anim.SetBool("run", false);
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
        anim.SetBool("run", false);
        
        //turn off attack hitbox
        hitbox.GetComponent<BoxCollider>().enabled = false;
        timer += 1.0f * Time.deltaTime;
        GetComponent<BoxCollider>().enabled = true;
        if (timer >= cooldown_period)
        {
            timer = 0.0f;
            phase = Phases.ABOUT_TO_ATTACK;
        }
    }
    public void abouttoattackUpdate()
    {
        GetComponent<BoxCollider>().enabled = true;
        if (enemy_type != EnemyType.CHARGER)
        {
            anim.SetBool("run", true);
        }
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        hitbox.GetComponent<BoxCollider>().enabled = false;
     
        if (updating)
        {
            timer += Time.deltaTime;
        }
        if (timer >= abouttoattack_period)
        {
            if (enemy_type == EnemyType.CHASER)
            {
               navmeshagent.enabled = true;
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

            //DROP AN EXTRA INGREDIENT WHEN RATION POWER UP IS ACTIVATED, AND PLAYER KILL A CERTAIN NUMBER OF ENEMIES
            player.GetComponentInChildren<PlayerAttack>().addenemykilled();
            if (player.GetComponentInChildren<PlayerAttack>().getenemykilled() >= player.GetComponentInChildren<PlayerStats>().getpp().return_ration()
                && player.GetComponentInChildren<PlayerStats>().getpp().return_ration() > 0)
            {
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
        em.recalculate_numberofenemies();
    }

    public void Death()
    {
        hitbox.GetComponent<BoxCollider>().enabled = false;


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


    //UNUSED CODES
    /*public void avoidanceCode(float random_number_z)
    {
        Vector3 resultingVector = -player.transform.position + transform.position;
        resultingVector.y = 0;
        GetComponent<Rigidbody>().velocity = resultingVector + new Vector3(random_number_z, 0, 0);
    }


    public void avoidanceCode_2(float random_number_x, float random_number_z)
    {
        float x = random_number_x;
        float z = random_number_z;

       navmeshagent.SetDestination(new Vector3(player.transform.position.x + x,
                 transform.position.y,
                 player.transform.position.z + z));
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
       navmeshagent.enabled = false;

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
    }*/
}
