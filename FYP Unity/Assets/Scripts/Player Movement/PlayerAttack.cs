using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject HitBox;
    [SerializeField] float attackcd;
    [SerializeField] float attackingtime;
    float attackcdtimer;
    float attackingtimer;
    bool attacking;
    // Start is called before the first frame update
    void Start()
    {
        HitBox.SetActive(false);
        attackingtimer = attackingtime;
        attackcdtimer = attackcd;
        attacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackcdtimer > 0)
            attackcdtimer -= Time.deltaTime;
        else if (attackcdtimer <= 0)
        {
            if (Input.GetMouseButtonDown(0) && !attacking)
            {
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
    }
}
