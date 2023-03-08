using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralCardBase : MonoBehaviour
{
    /*    
    Type of cards: Action, Resource
    Action cost (Applicable to action only): How much energy is needed to play
    Food/Thirst/Health changes if applicable
    */

    public string type;
    public int ActionCost;
    public int FoodChanges;
    public int ThirstChanges;
    public int HealthChanges;

    public virtual void executecard()
    {
        Debug.Log("No workey");
    }
}
