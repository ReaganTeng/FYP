using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Doublsb.Dialog;

public class TestMessage : MonoBehaviour
{
    public DialogManager DialogManager;
    [SerializeField] SceneName SceneNo;

    enum SceneName { INTRO,DAY1TO2,DAY2TOGAME }

    private void Awake()
    {
        switch(SceneNo)
        {
            case SceneName.INTRO:
                introduction();
                break;
            case SceneName.DAY1TO2:
                day1to2();
                break;
            case SceneName.DAY2TOGAME:
                day2togame();
                break;
                
        }
    }

    //private void Demo()
    //{
    //    var dialogTexts = new List<DialogData>();

    //    dialogTexts.Add(new DialogData("/size:up/Hi, /size:init/my name is Li.", "Li"));

    //    dialogTexts.Add(new DialogData("I am Sa. Popped out to let you know Asset can show other characters.", "Sa"));

    //    dialogTexts.Add(new DialogData("Like me for example!", "Rico"));

    //    dialogTexts.Add(new DialogData("And me", "Amanuid"));

    //    dialogTexts.Add(new DialogData("And me too!", "Niko"));

    //    dialogTexts.Add(new DialogData("You can also change the character's sprite /emote:Sad/like this, /click//emote:Happy/Smile.", "Li"));

    //    dialogTexts.Add(new DialogData("I wish I could show emotions like you Li, best I can do is just a smiley face here", "Rico"));

    //    dialogTexts.Add(new DialogData("What do you mean? We only have one sprite, how on earth can you smile", "Niko"));

    //    dialogTexts.Add(new DialogData(":)", "Rico"));

    //    dialogTexts.Add(new DialogData(":V /wait:0.5/... sigh, fine I guess that counts too", "Amanuid"));

    //    dialogTexts.Add(new DialogData("Ok, enough fooling around haha, we've got an island to escape :D", "Rico"));

    //    dialogTexts.Add(new DialogData("Whatever you say Rico... Good luck with your space restaurant game guys! Make us proud :)", "Niko"));

    //    DialogManager.Show(dialogTexts);
    //}


    private void introduction()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("…?", "Chef"));

        dialogTexts.Add(new DialogData("It seemth that the human hast awaketh.", "Alien1"));

        dialogTexts.Add(new DialogData("About time it did.", "Alien2"));

        dialogTexts.Add(new DialogData("What? …where am I?....I'm going to go back to bed….", "Chef"));

        dialogTexts.Add(new DialogData("Oh no you don't /size:up/(slaps him).", "Alien1"));

        dialogTexts.Add(new DialogData("Ow! Wait…this isn't a dream?", "Chef"));

        dialogTexts.Add(new DialogData("Oh human, you must be joking. Look around you..", "Alien2"));

        dialogTexts.Add(new DialogData("/size:up/?!!!!!!! ", "Chef"));

        dialogTexts.Add(new DialogData("Where am I? Who are you? What are you?!.", "Chef"));

        dialogTexts.Add(new DialogData("Allow us to introduce ourselves. I am Knip, and my companion here is...", "Knip"));

        dialogTexts.Add(new DialogData("Renge. Can we move on with the important things now?", "Renge"));

        dialogTexts.Add(new DialogData("Of course. Thou, human. Consider thyself worthy to be a part of our glorious cause.", "Knip"));

        dialogTexts.Add(new DialogData("You have been selected to work in our newly established high end restaurant, Isarata.", "Knip"));

        dialogTexts.Add(new DialogData("What planet are we on?", "Chef"));

        dialogTexts.Add(new DialogData("Planet WW, of course", "Knip"));

        dialogTexts.Add(new DialogData("Can we go back to Earth? Please? I haven't done anything to deserve this.", "Chef"));

        dialogTexts.Add(new DialogData("Oh yes you have. Your cooking skills have been revered by the human world, Chef Ramses. ", "Renge"));

        dialogTexts.Add(new DialogData("Oh wow, thanks. Wait, no! Can I leave?", "Ramsay"));

        dialogTexts.Add(new DialogData("Apologies but that is not an option.", "Knip"));

        dialogTexts.Add(new DialogData("You will ensure that our restaurant too is revered by the masses. ", "Renge"));

        dialogTexts.Add(new DialogData("Im leaving, bye." , "Ramsay", () => SceneManager.LoadScene("3")));

        DialogManager.Show(dialogTexts);

        
    }

    private void day1to2()
    {

    }

    private void day2togame()
    {

    }


}