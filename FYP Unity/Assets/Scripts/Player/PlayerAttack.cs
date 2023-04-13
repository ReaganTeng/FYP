using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    enum Weapon
    {
        SPATULA,
        KNIFE,
        ROLLINGPIN,
    }

    [SerializeField] GameObject HitBox;
    [SerializeField] float attackcd;
    [SerializeField] float attackingtime;
    [SerializeField] PlayerMovement pm;
    float attackcdtimer;
    float attackingtimer;
    bool attacking;
    int direction;



    [SerializeField] GameObject spaculaHitbox;
    [SerializeField] GameObject knifeHitbox;
    [SerializeField] GameObject pinHitbox;



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
    [SerializeField] TextMeshProUGUI txt;
    [SerializeField] GameObject line;
    [SerializeField] GameObject handle;
    Weapon currentweapon = Weapon.SPATULA;
    // Start is called before the first frame update


    void Awake()
    {
        chargeCurrentLvl = chargingduration;
        chargeMaxLvl = chargingduration;
        if (chargeBar != null)
        {
            chargeBar.maxValue = chargeMaxLvl;
            chargeBar.minValue = 0.0f;
            chargeBar.value = chargeBar.minValue;
        }
        //min_notch_value = chargeMaxLvl * (percentage_reduction/100);
        min_notch_value = chargeMaxLvl / number_of_charges;
        next_known_notch = chargingduration;
        last_known_notch = (int)next_known_notch - (int)min_notch_value;

        //Debug.Log("LOCALSCALE " + chargeBar.fillRect.rect.width);
        if (chargeBar != null)
        {
            for (int i = 1; i < number_of_charges; i++)
            {
                chargeBar.value += min_notch_value;
                if (i < number_of_charges - 1)
                {
                    GameObject l = Instantiate(line,
                        new Vector3(0, 0),
                        Quaternion.Euler(0, 0, 0)
                        );
                    l.transform.SetParent(canvas.transform);
                    l.transform.position = new Vector2(chargeBar.handleRect.position.x, chargeBar.transform.position.y);
                    l.transform.localScale = new Vector2(1, chargeBar.transform.localScale.y * 3);
                }
                else
                {
                    handle.SetActive(false);
                }
            }

            chargeBar.value = chargeMaxLvl;
        }

        switchWeapon();
        spaculaHitbox.SetActive(false);
        HitBox.SetActive(false);

        attackingtimer = attackingtime;
        attackcdtimer = attackcd;
        attacking = false;
        direction = 1;
    }
    void Start()
    {
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

        if (chargeBar != null)
        {
            for (int i = 1; i < number_of_charges; i++)
            {
                chargeBar.value += min_notch_value;
                if (i < number_of_charges - 1)
                {
                    GameObject l = Instantiate(line,
                        new Vector3(0, 0),
                        Quaternion.Euler(0, 0, 0)
                        );
                    l.transform.SetParent(canvas.transform);
                    l.transform.position = new Vector2(chargeBar.handleRect.position.x, chargeBar.transform.position.y);
                    l.transform.localScale = new Vector2(1, chargeBar.transform.localScale.y * 3);
                }
                else
                {
                    handle.SetActive(false);
                }
            }
        
            chargeBar.value = chargeMaxLvl;
        }

        switchWeapon();
        spaculaHitbox.SetActive(false);
        HitBox.SetActive(false);

        attackingtimer = attackingtime;
        attackcdtimer = attackcd;
        attacking = false;
        direction = 1;
    }


    

    // Update is called once per frame
    void Update()
    {

        if (chargeBar != null)
        {
            updatecharge();
        }

        //if (HitBox.activeSelf == true)
        //{
        //    Debug.Log("ACTIVE");
        //}
        //else
        //{
        //    Debug.Log("NOT ACTIVE");
        //}

        // Attacking
        if (attackcdtimer > 0)
        {
            attackcdtimer -= Time.deltaTime;
        }
        else 
        {
            //LIGHT ATTACK
            if (Input.GetMouseButtonDown(0) && !attacking)
            {
                //GetComponentInParent<PlayerStats>().setAttack(10.0f);
                AttackWhichDirection(direction);
                HitBox.SetActive(true);

                attacking = true;
                attackingtimer = attackingtime;
            }
            //

            //HEAVY ATTACK
            if (Input.GetMouseButtonDown(1) && !attacking
                && (int)chargeCurrentLvl>= (int)min_notch_value)
            {
                GetComponentInParent<PlayerStats>().setAttack(
                GetComponentInParent<PlayerStats>().getAttack(5.0f));
                AttackWhichDirection(direction);
                HitBox.SetActive(true);

                attacking = true;
                attackingtimer = attackingtime;
                depletecharge();
            }
            //


            if (attacking)
            {
                attackingtimer -= Time.deltaTime;
                if (attackingtimer <= 0)
                {
                    HitBox.SetActive(false);
                    attackcdtimer = attackcd;
                    attacking = false;
                }
            }

        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            direction = pm.GetDirection(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            AttackWhichDirection(direction);
        }

        // To swap between weapons
        if (Input.GetKey(KeyCode.Z))
        {
            currentweapon = Weapon.SPATULA;
            //switchWeapon();
        }
        else if (Input.GetKey(KeyCode.X))
        {
            currentweapon = Weapon.KNIFE;
           //switchWeapon();
        }
        else if (Input.GetKey(KeyCode.C))
        {
            currentweapon = Weapon.ROLLINGPIN;
            //switchWeapon();
        }

        switchWeapon();
    }


    public bool getHitbox()
    {
        return HitBox.activeSelf;
    }

    public void switchWeapon()
    {

       

        switch (currentweapon)
        {
            case Weapon.SPATULA:
                GetComponentInParent<PlayerStats>().setAttack(10.0f);

                //spaculaHitbox.transform.rotation
                //   //= new Quaternion(1, 0, 0, (Mathf.PI / 180) * (-90));
                //   //= Quaternion.Euler(-90, 0, 0);
                //   = Quaternion.Euler(-90, HitBox.transform.rotation.y, HitBox.transform.rotation.z);
                //HitBox = spaculaHitbox;
                spaculaHitbox.SetActive(HitBox.activeSelf);
                spaculaHitbox.transform.rotation = HitBox.transform.rotation;

                knifeHitbox.SetActive(false);
                pinHitbox.SetActive(false);
                //spaculaHitbox.SetActive(false);
                break;
            case Weapon.KNIFE:
                GetComponentInParent<PlayerStats>().setAttack(20.0f);

                //knifeHitbox.transform.rotation
                //    //= Quaternion.Euler(-90, 0, 0);
                //    = Quaternion.Euler(-90, HitBox.transform.rotation.y, HitBox.transform.rotation.z);
                //HitBox = knifeHitbox;
                knifeHitbox.SetActive(HitBox.activeSelf);
                knifeHitbox.transform.rotation = HitBox.transform.rotation;


                //knifeHitbox.SetActive(false);
                spaculaHitbox.SetActive(false);
                pinHitbox.SetActive(false);
                break;
            case Weapon.ROLLINGPIN:
                GetComponentInParent<PlayerStats>().setAttack(10.0f);

                //pinHitbox.transform.rotation
                //    //= Quaternion.Euler(-90, 0, 0);
                //    = Quaternion.Euler(-90, HitBox.transform.rotation.y, HitBox.transform.rotation.z);
                //HitBox = pinHitbox;
                pinHitbox.SetActive(HitBox.activeSelf);
                pinHitbox.transform.rotation = HitBox.transform.rotation;

                //pinHitbox.SetActive(false);
                spaculaHitbox.SetActive(false);
                knifeHitbox.SetActive(false);
                break;
            default:
                break;
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

        if (txt != null)
        {
            txt.text = time.ToString();
        }
        //Debug.Log("LKN: " + last_known_notch);
        //Debug.Log("NKN: " + next_known_notch);
        //Debug.Log("CHA: " + charge);
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
            case 1:
                newrotation = Quaternion.Euler(0, -90, 0);
                HitBox.transform.rotation = newrotation;
                //rotatehWeapon(newrotation);

                break;
            case 2:
                newrotation = Quaternion.Euler(0, 0, 0);
                HitBox.transform.rotation = newrotation;
                //rotatehWeapon(newrotation);

                break;
            case 3:
                newrotation = Quaternion.Euler(0, 90, 0);
                HitBox.transform.rotation = newrotation;
                //rotatehWeapon(newrotation);

                break;
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
}
