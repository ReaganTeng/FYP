using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCard : GeneralCardBase
{
    public string type;
    public int ActionCost;
    public int FoodChanges;
    public int ThirstChanges;
    public int HealthChanges;

    private void Start()
    {
        type = "Action";
        ActionCost = 1;
        FoodChanges = 2;
        ThirstChanges = 2;
        HealthChanges = 2;
    }

    public override void executecard()
    {
        Debug.Log("Work!");
    }
}
