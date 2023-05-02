using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackHitboxPivot : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        transform.LookAt(player.transform.position);
    }
}
