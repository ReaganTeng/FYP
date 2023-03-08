using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATestCard : GeneralCardBase
{
    public string type;
    public int ActionCost;
    public int FoodChanges;
    public int ThirstChanges;
    public int HealthChanges;

    private void Start()
    {
        type = "Action";
        ActionCost = 0;
        FoodChanges = 3;
        ThirstChanges = 3;
        HealthChanges = 3;
    }

    public override void executecard()
    {
        Debug.Log("Lame!");
    }
}
