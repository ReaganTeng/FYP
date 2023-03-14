using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGlow : MonoBehaviour
{
    [SerializeField] Material noglow;
    [SerializeField] Material glow;

    public void TurnOnHighlight()
    {
        gameObject.GetComponent<Renderer>().material = glow;
    }

    public void TurnOffHighlight()
    {
        gameObject.GetComponent<Renderer>().material = noglow;
    }
}
