using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGlow : MonoBehaviour
{
    [SerializeField] Texture noglow;
    [SerializeField] Texture glow;

    public void TurnOnHighlight()
    {
        gameObject.GetComponent<Renderer>().material.mainTexture = glow;
    }

    public void TurnOffHighlight()
    {
        gameObject.GetComponent<Renderer>().material.mainTexture = noglow;
    }
}
