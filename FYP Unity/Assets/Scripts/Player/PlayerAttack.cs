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
    Weapon currentweapon = Weapon.SPATULA;
    // Start is called before the first frame update



    void Start()
    {
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


        // Attacking
        if (attackcdtimer > 0)
            attackcdtimer -= Time.deltaTime;
        else if (attackcdtimer <= 0)
        {
            if (
                (Input.GetMouseButtonDown(0) && !attacking)
                //||
                //(Input.GetMouseButtonDown(0) && !attacking)
                )
            {
                AttackWhichDirection(direction);
                HitBox.SetActive(true);
                attacking = true;
                attackingtimer = attackingtime;
            }

            if (attacking)
            {
                attackingtimer -= Time.deltaTime;
                if (attackingtimer <= 0)
                {
                    attacking = false;
                    HitBox.SetActive(false);
                    attackcdtimer = attackcd;
                }
            }
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
