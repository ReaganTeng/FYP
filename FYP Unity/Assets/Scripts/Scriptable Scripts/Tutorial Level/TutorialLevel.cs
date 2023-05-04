using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialLevel", menuName = "TutorialLevel")]
public class TutorialLevel : ScriptableObject
{
    public List<Tutorial.TutorialPopUp> DialogueAndInstructions;
}
