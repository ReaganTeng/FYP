using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientBarrelManager : MonoBehaviour
{
    public static IngredientBarrelManager instance;
    List<IngredientBarrel> ingrebarrel = new List<IngredientBarrel>();

    public enum BarrelTypes
    {
        FLOUR,
        SEAWEED,
        SUGAR,
        SALT,
    }


    private void Awake()
    {
        instance = this;

        IngredientBarrel[] tempBarrelArray = gameObject.GetComponentsInChildren<IngredientBarrel>();
        for (int i = 0; i < tempBarrelArray.Length; i++)
        {
            ingrebarrel.Add(tempBarrelArray[i]);
        }
    }

    public void ToggleIngredientBarrel(bool active)
    {
        for (int i = 0; i < ingrebarrel.Count; i++)
        {
            ingrebarrel[i].SetIsActive(active);
        }
    }

    public void TutorialIngredientBarrel(BarrelTypes barrelType, bool active)
    {
        for (int i = 0; i < ingrebarrel.Count; i++)
        {
            if (ingrebarrel[i].GetBarrelType() == barrelType)
            {
                HighlightIngredientBarrel(i, active);
                ingrebarrel[i].SetIsActive(active);
            }
        }
    }

    void HighlightIngredientBarrel(int index, bool active)
    {
        ingrebarrel[index].gameObject.GetComponent<ObjectHighlighted>().ToggleHighlight(active);
    }
}
