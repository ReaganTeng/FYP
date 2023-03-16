using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doublsb.Dialog;

public class TestMessage : MonoBehaviour
{
    public DialogManager DialogManager;

    //public GameObject[] Example;

    private void Awake()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/size:up/Hi, /size:init/my name is Li.", "Li"));

        dialogTexts.Add(new DialogData("I am Sa. Popped out to let you know Asset can show other characters.", "Sa"));
        
        dialogTexts.Add(new DialogData("Like me for example!", "Rico"));

        dialogTexts.Add(new DialogData("And me", "Amanuid"));

        dialogTexts.Add(new DialogData("And me too!", "Niko" ));

        dialogTexts.Add(new DialogData("You can also change the character's sprite /emote:Sad/like this, /click//emote:Happy/Smile.", "Li"));

        dialogTexts.Add(new DialogData("I wish I could show emotions like you Li, best I can do is just a smiley face here", "Rico"));

        dialogTexts.Add(new DialogData("What do you mean? We only have one sprite, how on earth can you smile", "Niko" ));

        dialogTexts.Add(new DialogData(":)", "Rico"));

        dialogTexts.Add(new DialogData(":V /wait:0.5/... sigh, fine I guess that counts too", "Amanuid"));

        dialogTexts.Add(new DialogData("Ok, enough fooling around haha, we've got an island to escape :D", "Rico"));

        dialogTexts.Add(new DialogData("Whatever you say Rico... Good luck with your space restaurant game guys! Make us proud :)", "Niko"));

        DialogManager.Show(dialogTexts);
    }
}
