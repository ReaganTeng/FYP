using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    private TextMeshProUGUI text;
    float attackcdtimer;
    float attackingtimer;
    bool attacking;
    int direction;

    

    [SerializeField] GameObject spaculaHitbox;
    [SerializeField] GameObject knifeHitbox;
    [SerializeField] GameObject pinHitbox;
   


    float cooldown;


    Weapon currentweapon = Weapon.SPATULA;
    // Start is called before the first frame update



    void Start()
    {
        cooldown = 0.0f;
        HitBox.SetActive(false);
        attackingtimer = attackingtime;
        attackcdtimer = attackcd;
        attacking = false;
        direction = 1;
        text = GameObject.FindGameObjectWithTag("WeaponText").GetComponent<TextMeshProUGUI>();
        text.text = "Weapon: Spatula";
    }

   
    // Update is called once per frame
    void Update()
    {

        cooldown -= Time.deltaTime;

        switch (currentweapon)
        { 
            case Weapon.SPATULA:
                GetComponentInParent<PlayerStats>().setAttack(10.0f);
                //spaculaHitbox.SetActive(true);
                HitBox = spaculaHitbox;
                knifeHitbox.SetActive(false);
                pinHitbox.SetActive(false);
                break;
            case Weapon.KNIFE:
                GetComponentInParent<PlayerStats>().setAttack(20.0f);
                //knifeHitbox.SetActive(true);
                HitBox = knifeHitbox;
                spaculaHitbox.SetActive(false);
                pinHitbox.SetActive(false);
                break;
            case Weapon.ROLLINGPIN:
                GetComponentInParent<PlayerStats>().setAttack(10.0f);
                //pinHitbox.SetActive(true);
                HitBox = pinHitbox;
                spaculaHitbox.SetActive(false);
                knifeHitbox.SetActive(false);
                break;
            default:
                break;
        }

        // Attacking
        if (attackcdtimer > 0)
            attackcdtimer -= Time.deltaTime;
        else if (attackcdtimer <= 0)
        {
            //LIGHT ATTACK
            if (Input.GetMouseButtonDown(0) && !attacking
                && cooldown <= 0)
            {
                //GetComponentInParent<PlayerStats>().setAttack(10.0f);
                Debug.Log("LIGHT ATTACK " + GetComponentInParent<PlayerStats>().getAttack());
                AttackWhichDirection(direction);
                cooldown = 1.0f;
                HitBox.SetActive(true);
                attacking = true;
                attackingtimer = attackingtime;
            }
            //

            //HEAVY ATTACK
            if (Input.GetMouseButtonDown(1) && !attacking
                && cooldown <= 0)
            {
                GetComponentInParent<PlayerStats>().setAttack(
                    GetComponentInParent<PlayerStats>().getAttack(5.0f));
                Debug.Log("HEAVY ATTACK " + GetComponentInParent<PlayerStats>().getAttack());
                cooldown = 5.0f;
                AttackWhichDirection(direction);
                HitBox.SetActive(true);
                attacking = true;
                attackingtimer = attackingtime;
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
                    //HitBox.SetActive(false);
                    //attackcdtimer = attackcd;
                }
            }
            //else
            //{
            //    HitBox.SetActive(false);
            //    attackcdtimer = attackcd;
            //}
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            direction = pm.GetDirection(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        // To swap between weapons
        if (Input.GetKey(KeyCode.Z))
        {
            currentweapon = Weapon.SPATULA;
            text.text = "Weapon: Spatula";
        }
        else if (Input.GetKey(KeyCode.X))
        {
            currentweapon = Weapon.KNIFE;
            text.text = "Weapon: Knife";
        }
        else if (Input.GetKey(KeyCode.C))
        {
            currentweapon = Weapon.ROLLINGPIN;
            text.text = "Weapon: RollingPin";
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
                break;
            case 2:
                newrotation = Quaternion.Euler(0, 0, 0);
                HitBox.transform.rotation = newrotation;
                break;
            case 3:
                newrotation = Quaternion.Euler(0, 90, 0);
                HitBox.transform.rotation = newrotation;
                break;
            case 4:
                newrotation = Quaternion.Euler(0, 180, 0);
                HitBox.transform.rotation = newrotation;
                break;
        }
    }

    public int GetWeaponType()
    {
        // 0 is spatula, 1 is knife, 2 is rolling pin
        return (int)currentweapon;
    }
}
