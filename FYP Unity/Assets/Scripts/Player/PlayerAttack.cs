using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AI;
using DigitalRuby.SoundManagerNamespace;

public class PlayerAttack : MonoBehaviour
{
    
    public enum Weapon
    {
        SPATULA, //0
        KNIFE, //1
        ROLLINGPIN, //2
    }
    SoundManagerDemo SoundManager;
    [SerializeField] GameObject HitBox;
    [SerializeField] float attackcd;
    [SerializeField] float attackingtime;
    [SerializeField] PlayerMovement pm;
    float attackcdtimer;
    float attackingtimer;
    bool attacking;
    int direction;
    GameObject currentWeaponDisplay;

    // Set the attack for the player

    [SerializeField] GameObject KnifeWeaponDisplay;
    [SerializeField] GameObject RollerWeaponDisplay;
    [SerializeField] GameObject SpatulaWeaponDisplay;

    [SerializeField] GameObject spaculaHitbox;
    [SerializeField] GameObject knifeHitbox;
    [SerializeField] GameObject pinHitbox;
    bool disableControls;
    bool CanSwapWeapon = true;

    float chargeCurrentLvl;
    float chargeMaxLvl;
    //[SerializeField] float chargingSpeed;
    float last_known_notch;
    float next_known_notch;
    //[SerializeField] float percentage_reduction;

    //the default charging duration in seconds
    [SerializeField] float chargingduration;
    //number of charges the player has
    [SerializeField] float number_of_charges;
    //how much % you want to reduce in charging duration
    public float reduction_in_percentage;

    float min_notch_value;
    [SerializeField] Slider chargeBar;
    [SerializeField] Canvas canvas;
    int time;
    [SerializeField] TextMeshProUGUI chargertimertext;
    [SerializeField] GameObject line;
    [SerializeField] GameObject handle;
    Weapon currentweapon = Weapon.ROLLINGPIN;
    // Start is called before the first frame update


    GameObject closestenemy;
    bool isclicked;
    bool heavyattackclicked;
    bool lightattackclicked;

    float click_timer;

    float currentAnimationLength;

    // Enabling/Disabling Heavy attack (for tutorial purposes. DO NOT REMOVE)
    bool CanHeavyAttack = true;


    [SerializeField] LayerMask enemyLM;
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip attackanimation;

    [SerializeField] AnimationClip attackanimation_spatula;
    [SerializeField] AnimationClip attackanimation_pin;


    [SerializeField] PlayerProgress pp;

    void Start()
    {
        UpdateWeaponDisplay();
        click_timer = 0.0f;
        isclicked = false;
        heavyattackclicked = false;
        lightattackclicked = false;

        chargeCurrentLvl = chargingduration;
        chargeMaxLvl = chargingduration;
        if (chargeBar != null)
        {
            chargeBar.maxValue = chargeMaxLvl;
            chargeBar.minValue = 0.0f;
            chargeBar.value = chargeBar.minValue;
        }
        min_notch_value = chargeMaxLvl / number_of_charges;
        next_known_notch = chargingduration;
        last_known_notch = (int)next_known_notch - (int)min_notch_value;

        drawdivider();

        switchWeapon();
        spaculaHitbox.SetActive(false);

        HitBox.transform.rotation = Quaternion.Euler(0, 180, 0);
        HitBox.SetActive(false);

        attackingtimer = attackingtime;
        attackcdtimer = attackcd;
        attacking = false;
        direction = 1;
    }


    
    void drawdivider()
    {
        if (chargeBar != null)
        {
            for (int i = 0; i < number_of_charges;)
            {
                if (i < number_of_charges - 1)
                {
                    chargeBar.value += min_notch_value;

                    GameObject l = Instantiate(line,
                            new Vector3(0, 0),
                            Quaternion.Euler(0, 0, 0)
                            );
                    l.transform.SetParent(canvas.transform);
                    l.transform.position = new Vector2(chargeBar.handleRect.position.x, 
                        chargeBar.transform.position.y);
                    //l.transform.localScale = new Vector2(1, chargeBar.transform.lossyScale.y);
                    //yourUIElement.GetComponent(RectTransform).sizeDelta = new Vector2(width, height);
                    l.GetComponentInChildren<RectTransform>().sizeDelta = 
                        new Vector2(5,
                            chargeBar.GetComponent<RectTransform>().rect.height);
                    i++;
                }
                else
                {
                    handle.SetActive(false);
                    break;
                }
            }

            chargeBar.value = chargeMaxLvl;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (disableControls)
            return;

        if (chargeBar != null)
        {
            updatecharge();
        }

        if (chargertimertext != null)
        {
            if (chargeBar.value >= chargeBar.maxValue)
            {
                chargertimertext.enabled = false;
            }
            else
            {
                chargertimertext.enabled = true;
            }
        }
        //

        GameObject.FindGameObjectWithTag("playerspriterenderer").GetComponent<Animator>().SetInteger("WeaponEquipped", (int)currentweapon);

        currentAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;

        // Attacking
        if (attackcdtimer > 0)
        {
            attackcdtimer -= Time.deltaTime;
        }
        else 
        {
            GameObject.FindGameObjectWithTag("playerspriterenderer").GetComponent<Animator>().SetBool("click", lightattackclicked);
            GameObject.FindGameObjectWithTag("playerspriterenderer").GetComponent<Animator>().SetBool("heavyattackclick", heavyattackclicked);


            if (!GameObject.FindGameObjectWithTag("playerspriterenderer").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("hurt_knife")
                && !GameObject.FindGameObjectWithTag("playerspriterenderer").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("hurt_pin")
                && !GameObject.FindGameObjectWithTag("playerspriterenderer").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("hurt_spatula"))
            {
                //LIGHT ATTACK
                if (Input.GetMouseButtonDown(0) && !attacking
                    && isclicked == false
                    && click_timer <= 0
                    && notlightattacking())
                {
                    //Debug.Log("LIGHT ATTACK");
                    isclicked = true;
                    AttackWhichDirection(direction);
                    //HitBox.SetActive(true);
                    //attacking = true;

                    lightattackclicked = true;
                    attackingtimer = attackingtime;
                    //Debug.Log("LIGHT ATTACK" + transform.parent.GetComponent<PlayerStats>().GetPlayerAttack());

                    if (currentweapon == Weapon.ROLLINGPIN)
                    {
                        GameSoundManager.PlaySound("RollerAttack");
                    }
                    if (currentweapon == Weapon.SPATULA)
                    {
                        GameSoundManager.PlaySound("SpatAttack");
                    }
                    if (currentweapon == Weapon.KNIFE)
                    {
                        GameSoundManager.PlaySound("KnifeAttack");
                    }
                }
                //

                //HEAVY ATTACK
                if (Input.GetMouseButtonDown(1) && !attacking
                    && isclicked == false
                     && click_timer <= 0
                    && CanHeavyAttack
                    && (int)chargeCurrentLvl >= (int)min_notch_value
                    && notheavyattacking()
                        )
                {
                   //Debug.Log("HEAVY ATTACK");
                    isclicked = true;
                    transform.parent.GetComponent<PlayerStats>().setAttack(true);
                    heavyattackclicked = true;
                    AttackWhichDirection(direction);
                    //HitBox.SetActive(true);
                    //attacking = true;
                    attackingtimer = attackingtime;
                    depletecharge();

                    if (
                    currentweapon == Weapon.KNIFE)
                    {
                        click_timer = attackanimation.length;
                    }
                    else if (
                    currentweapon == Weapon.ROLLINGPIN)
                    {
                        click_timer = attackanimation_pin.length;
                    }
                    else
                    {
                        click_timer = attackanimation_spatula.length;
                    }

                    //GET CLOSEST ENEMY
                    closestenemy = GetClosestEnemy();
                    //
                }
                //
            }

            //FIND CLOSEST ENEMY
            //if(heavyattackclicked)
            //{
            //    //closestenemy.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
            //    closestenemy.GetComponent<NavMeshAgent>().speed= 0;
            //    closestenemy.GetComponent<Rigidbody>().velocity = new Vector3 (0, 0, 0);
            //}
            //else
            //{
            //    if(closestenemy != null)
            //    {
            //        //closestenemy.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            //        closestenemy = null;
            //    }
            //}
            //

            if (isclicked)
            {
                //if is reaches 50% of the attack animation
                click_timer -= Time.deltaTime;
                //SPATULA, //0
                //ROLLINGPIN, //2
                if (
                    (currentweapon == Weapon.KNIFE &&  click_timer <= (attackanimation.length  *.5f)
                    / pp.return_heavyattackspeed())
                    ||
                     (currentweapon == Weapon.ROLLINGPIN && click_timer <= (attackanimation_pin.length * .25f)
                    / pp.return_heavyattackspeed())
                    ||
                     (currentweapon == Weapon.SPATULA && click_timer <= (attackanimation_spatula.length * .5f)
                    / pp.return_heavyattackspeed())
                    )
                {
                    animator.speed = pp.return_heavyattackspeed();
                    attacking = true;
                }
                //

                if (click_timer < 0)
                {
                    animator.speed = 1;
                    isclicked = false;
                }
            }


            /*if (!GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Dash")
                  && !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack_with_knife")
                && !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack_with_pin")
                && !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack_with_spatula")
                && !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("heavyattack_knife")
                 && !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("heavyattack_pin")
                 && !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("heavyattack_pin"))*/


            //activate hitbox
            if (attacking)
            {
                attackingtimer -= Time.deltaTime;

                if (attackingtimer <= 0)
                {
                    HitBox.SetActive(false);
                    click_timer = 0.0f;
                    attackcdtimer = attackcd;
                    attacking = false;

                    heavyattackclicked = false;
                    lightattackclicked = false;
                }
                else
                {
                    HitBox.SetActive(true);
                }
            }
            //
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (animator.GetBool("click") == false)
            {
                direction = pm.GetDirection(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                AttackWhichDirection(direction);
            }
        }


        if (!animator.GetBool("click")
            && !animator.GetBool("heavyattackclick")
            && CanSwapWeapon)
        {
            // To swap between weapons
            if (Input.GetKey(KeyCode.Z))
            {
                currentweapon = Weapon.SPATULA;
                UpdateWeaponDisplay();
            }
            else if (Input.GetKey(KeyCode.X))
            {
                currentweapon = Weapon.KNIFE;
                UpdateWeaponDisplay();
            }
            else if (Input.GetKey(KeyCode.C))
            {
                currentweapon = Weapon.ROLLINGPIN;
                UpdateWeaponDisplay();
            }
        }

        //Collider[] hitEnemy = Physics.OverlapBox(HitBox.transform.position, HitBox.transform.lossyScale,
        //    HitBox.transform.rotation, enemyLM);

        //foreach(Collider enemies in hitEnemy)
        //{
        //    Debug.Log("Enemy hit");
        //}

        switchWeapon();
    }

    public void resetattacking()
    {
        click_timer = 0;
        attackingtimer = 0;
        heavyattackclicked = false;
        lightattackclicked = false;
    }
    public bool attacking_or_not()
    {
        return attacking;
    }

    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireCube
    //        (HitBox.transform.position, HitBox.transform.lossyScale);
    //}


    public GameObject GetClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(GameObject potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestEnemy = potentialTarget;
            }
            else
            {

            }
        }

        return closestEnemy;
    }

    public bool getHitbox()
    {
        return HitBox.activeSelf;
    }

    public void switchWeapon()
    {
       // Debug.Log("WEAPON_EQUIPPED " + (int)currentweapon);


        switch (currentweapon)
        {
            case Weapon.SPATULA:
                if (!isclicked)
                {
                    GetComponentInParent<PlayerStats>().setAttack(false);
                }
                spaculaHitbox.SetActive(HitBox.activeSelf);

                spaculaHitbox.transform.rotation = HitBox.transform.rotation;
                knifeHitbox.SetActive(false);
                pinHitbox.SetActive(false);
                break;
            case Weapon.KNIFE:
                if (!isclicked)
                {
                    GetComponentInParent<PlayerStats>().setAttack(false);
                }
                knifeHitbox.SetActive(HitBox.activeSelf);
                knifeHitbox.transform.rotation = HitBox.transform.rotation;
                spaculaHitbox.SetActive(false);
                pinHitbox.SetActive(false);
                break;
            case Weapon.ROLLINGPIN:
                if (!isclicked)
                {
                    GetComponentInParent<PlayerStats>().setAttack(false);
                }
                pinHitbox.SetActive(HitBox.activeSelf);
                pinHitbox.transform.rotation = HitBox.transform.rotation;
                spaculaHitbox.SetActive(false);
                knifeHitbox.SetActive(false);
                break;
            default:
                break;
        }
    }

    public bool notlightattacking()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack_with_pin")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack_with_knife")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack_with_spatula"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool notheavyattacking()
    {
        if (
        /*&& !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack_with_knife")
        && !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack_with_pin")
        && !GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack_with_spatula")*/
        !animator.GetCurrentAnimatorStateInfo(0).IsName("heavyattack_knife")
        && !animator.GetCurrentAnimatorStateInfo(0).IsName("heavyattack_pin")
        && !animator.GetCurrentAnimatorStateInfo(0).IsName("heavyattack_pin"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool notswitchingweapons()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("SpatulaEquip")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("KnifeEquip")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("PinEquip"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void updatecharge()
    {
        if (Input.GetKey(KeyCode.G))
        {
            reduction_in_percentage = 50;
        }
        else
        {
            reduction_in_percentage = 0;
            
            if(chargeCurrentLvl >= chargeMaxLvl)
            {
                chargeCurrentLvl = chargeMaxLvl;
            }
        }


        if (reduction_in_percentage > 0)
        {
            chargeMaxLvl = chargingduration - (chargingduration * reduction_in_percentage / 100);
        }
        else
        {
            chargeMaxLvl = chargingduration;
        }

        chargeBar.maxValue = chargeMaxLvl;
        min_notch_value = chargeMaxLvl / number_of_charges;

        //CONTINUE TO INCREASE chargeCurrentLvlBAR WHEN IT'S BELOW MAXIMUM VALUE
        if ((int)chargeCurrentLvl< (int)chargeMaxLvl)
        {
            //chargeCurrentLvl+= chargingSpeed * Time.deltaTime;
            chargeCurrentLvl+= Time.deltaTime;

        }
        else if ((int)chargeCurrentLvl>= (int)chargeMaxLvl)
        {
            chargeCurrentLvl= (int)chargeMaxLvl;
        }
        //

        chargeBar.value = chargeCurrentLvl;

        if ((int)chargeCurrentLvl>= (int)next_known_notch + (int)min_notch_value
            && (int)next_known_notch < (int)chargeMaxLvl)
        {
            last_known_notch = (int)next_known_notch;
            next_known_notch = (int)next_known_notch  + (int)min_notch_value;
        }


        if(chargeCurrentLvl <= (int)min_notch_value)
        {
            time = ((int)min_notch_value - (int)chargeCurrentLvl);
        }

        else
        {
            time = (((int)next_known_notch + (int)min_notch_value)
                - (int)chargeCurrentLvl);
        }

        if (chargertimertext != null)
        {
            chargertimertext.text = time.ToString();
        }


       
    }
    public void depletecharge()
    {
        if ((int)last_known_notch <= 0)
        {
            int diff = (int)min_notch_value - 
                (((int)next_known_notch + (int)min_notch_value) - 
                (int)chargeCurrentLvl);
            //Debug.Log("INITIAL CHARGES LEFT " + (((int)next_known_notch + (int)min_notch_value) - (int)chargeCurrentLvl));

            chargeCurrentLvl = (int)last_known_notch + diff;

            //Debug.Log("CHARGES LEFT " + 
            //    ((int)next_known_notch/*(int)min_notch_value*//*)*/ - (int)chargeCurrentLvl));

        }
        else
        {
            int diff = (int)min_notch_value - 
                (((int)next_known_notch + (int)min_notch_value) - 
                (int)chargeCurrentLvl);
            //Debug.Log("INITIAL CHARGES LEFT" + (((int)next_known_notch + (int)min_notch_value) - (int)chargeCurrentLvl));

            chargeCurrentLvl = (int)last_known_notch + diff;
            last_known_notch = (int)last_known_notch - (int)min_notch_value;
            next_known_notch = (int)next_known_notch - (int)min_notch_value;
            //Debug.Log("CHARGES LEFT" + (((int)next_known_notch + (int)min_notch_value) - (int)chargeCurrentLvl));
        }
    }
    void AttackWhichDirection(int direction)
    {
        Quaternion newrotation;

        switch (direction)
        {
            //case 1:
            //    newrotation = Quaternion.Euler(0, -90, 0);
            //    HitBox.transform.rotation = newrotation;
            //    //rotatehWeapon(newrotation);

            //    break;
            case 2:
                newrotation = Quaternion.Euler(0, 0, 0);
                HitBox.transform.rotation = newrotation;
                //rotatehWeapon(newrotation);

                break;
            //case 3:
            //    newrotation = Quaternion.Euler(0, 90, 0);
            //    HitBox.transform.rotation = newrotation;
            //    //rotatehWeapon(newrotation);

            //    break;
            case 4:
                newrotation = Quaternion.Euler(0, 180, 0);
                HitBox.transform.rotation = newrotation;
                //rotatehWeapon(newrotation);

                break;
        }
        //Debug.Log("NEW ROTATION " + HitBox.transform.rotation);
    }

    public int GetWeaponType()
    {
        // 0 is spatula, 1 is knife, 2 is rolling pin
        return (int)currentweapon;
    }

    public bool GetDisableControls()
    {
        return disableControls;
    }

    public void SetDisableControls(bool set)
    {
        disableControls = set;
    }

    public void SetCanSwapWeapon(bool canswapweapon)
    {
        CanSwapWeapon = canswapweapon;
    }

    public void SetCanDoHeavyAttack(bool heavyAttackAllowed)
    {
        CanHeavyAttack = heavyAttackAllowed;
    }
    void UpdateWeaponDisplay()
    {
        switch((Weapon)GetWeaponType())
        {
            case Weapon.KNIFE:
                 KnifeWeaponDisplay.SetActive(true);
                SpatulaWeaponDisplay.SetActive(false);
                RollerWeaponDisplay.SetActive(false);

                break;

            case Weapon.ROLLINGPIN:
                RollerWeaponDisplay.SetActive(true);
                KnifeWeaponDisplay.SetActive(false);
                SpatulaWeaponDisplay.SetActive(false);
                break;

            case Weapon.SPATULA:
                SpatulaWeaponDisplay.SetActive(true);
                KnifeWeaponDisplay.SetActive(false);
                RollerWeaponDisplay.SetActive(false);
                break;
        }
    }    
}
