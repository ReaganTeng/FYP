using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    //public GameObject[] zones;
    //public int zone_no;
    float timer;
    [SerializeField] public float cooldownend;
    [SerializeField] public float abouttoattackend;

    int attack_type;

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

        phase = Phases.ATTACK_TYPE_1;
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

            Debug.Log("Enemy Health Left: " + EnemyHealth);
            // Precise Kill
            if (EnemyHealth == 0)
            {
                Debug.Log("Precise Kill!");
                EnemyDie();
            }

            else if (EnemyHealth < 0)
            {
                Debug.Log("Killed!");
                EnemyDie();
            }
        }
    }

    private void Update()
    {
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
                    Debug.Log("ABOUT TO ATTACK");
                    timer += 1.0f * Time.deltaTime;

                    if (timer >= abouttoattackend)
                    {
                        timer = 0.0f;
                        attack_type = Random.Range(1, 3);


                        if (attack_type == 1)
                        {
                            phase = Phases.ATTACK_TYPE_1;
                        }
                        else /*if (attack_type == 2)*/
                        {
                            phase = Phases.ATTACK_TYPE_2;
                        }
                    }
                    break;
                }
            case Phases.COOLDOWN:
                {
                    Debug.Log("COOLDOWN");

                    timer += 1.0f * Time.deltaTime;

                    if (timer >= cooldownend)
                    {
                        timer = 0.0f;
                        phase = Phases.ABOUT_TO_ATTACK;


                    }

                    break;
                }
        }

    }



    


    void EnemyDie()
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
            Instantiate(whichobject, gameObject.transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
