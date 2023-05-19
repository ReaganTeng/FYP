using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenzyMode : MonoBehaviour
{
    [SerializeField] PlayerProgress pp;
    public static FrenzyMode instance;
    private float BaseFrenzyTime = 5.0f;
    private float frenzyTimer;
    private bool InFrenzyMode;
    private int frenzyStack = 0;
    private int maxStack = 0;
    bool CanGoFrenzy = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ResetFrenzy();
        InFrenzyMode = false;
        maxStack = pp.GetFrenzyModeMaxStack();
        if (maxStack > 0)
        {
            CanGoFrenzy = true;
        }
    }

    private void Update()
    {
        if (InFrenzyMode)
        {
            frenzyTimer -= Time.deltaTime;

            if (frenzyTimer <= 0)
            {
                InFrenzyMode = false;
                ResetFrenzy();
            }
        }
    }

    public void ActivateFrenzyMode()
    {
        if (!CanGoFrenzy)
            return;

        if (InFrenzyMode)
        {
            if (frenzyStack < maxStack)
            {
                frenzyStack++;
            }
        }
        InFrenzyMode = true;
    }

    void ResetFrenzy()
    {
        frenzyTimer = BaseFrenzyTime;
        frenzyStack = 0;
    }

    public bool GetCanGoFrenzy()
    {
        return CanGoFrenzy;
    }

    public int GetFrenzyStack()
    {
        return frenzyStack;
    }
}
