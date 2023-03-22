using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serving : MonoBehaviour
{
    public void Serve()
    {
        OrderSystem os = GameObject.FindGameObjectWithTag("GameManager").GetComponent<OrderSystem>();
        os.Serving();
    }
}
