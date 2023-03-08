using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralCardManager : MonoBehaviour
{
    // Probably Create a list to store all the cards?
    public GeneralCardBase[] testcards = new GeneralCardBase[10];
    public static GeneralCardManager instance;
    /* Variables that each card can contain:
    
    Type of cards: Action, Quick Action, Resource
    Action cost (Applicable to action only): How much energy is needed to play
    Food/Thirst/Health changes if applicable
     
     */
    private void Start()
    {
        instance = this;
    }

    public void Execute()
    {
        testcards[0].executecard();
        testcards[1].executecard();
    }
}
