using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Doublsb.Dialog;

public class TestMessage : MonoBehaviour
{
    public DialogManager DialogManager;
    SceneName SceneNo;
    [SerializeField] LevelManager LM;
    bool SkipVN;
    public Button button;

    [SerializeField] Image background;
    public Sprite[] imageList;

    enum SceneName
    {
        TUT1VN, // tutorial //-2
        TUT2VN, // tutorial // -1
        ETUT2VN, // tutorial // 0
        L1VN, // 1
    }

    private void Awake()
    {
        // if it is tutorial levels, load those scene instead
        SceneNo = (SceneName)(LM.DaySelected + 2);

        for (int i = 0; i < LM.levelInfo.Count; i++)
        {
            if (LM.levelInfo[i].WhatDay == LM.DaySelected)
            {
                SkipVN = LM.levelInfo[i].CanSkipVN;
                break;
            }
        }


        switch (SceneNo)
        {
            case SceneName.TUT1VN:
                intro();
                break;
            case SceneName.TUT2VN:
                tut2vn();
                break;
            case SceneName.ETUT2VN:
                tut2end();
                break;
        }

        
    }

    private void Start()
    {
        UpdateButtonState();
        GameSoundManager.PlayMusic("VNMusic");
    }

    private void UpdateButtonState()
    {
        button.gameObject.SetActive(SkipVN); // Activate or deactivate the button based on the boolean value
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


    private void intro()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("…?", "Chef"));

        dialogTexts.Add(new DialogData("It seemth that the human hast awaketh.", "Alien1", () => background.sprite = imageList[0])); //background imgs need to be called 1 line before

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

        LM.DaySelected = -2;

        dialogTexts.Add(new DialogData("Sigh, where do I start.." , "Ramsay", () => SceneManager.LoadScene("Game Scene")));

        DialogManager.Show(dialogTexts);

        
    }

    private void tut2vn()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("Ughhh, my body is sore everywhere...", "Ramsay"));

        dialogTexts.Add(new DialogData("Oh, the human is back!", "Knip"));

        dialogTexts.Add(new DialogData("Welcome back, seeing as you are still in one piece I assume you've passed your first day", "Renge"));

        dialogTexts.Add(new DialogData("Wait a minute, what do you mean by in one piece", "Ramsay"));

        dialogTexts.Add(new DialogData("Oh, the head chef is well known for permanently disabling his employess that cause trouble in his kitchen", "Knip"));

        dialogTexts.Add(new DialogData("The last chef before you is no where to be found after they failed too many orders", "Knip"));

        dialogTexts.Add(new DialogData("/size:up/ *gulp*", "Ramsay"));

        dialogTexts.Add(new DialogData("/size:up/ /emote:nosprite/ *gulp*", "Chef"));

        dialogTexts.Add(new DialogData("Don't worry, he won't lay a tentacle on you if you keep up your work performance", "Renge"));

        dialogTexts.Add(new DialogData("Every passing day his expectations from you will get higher and higher", "Renge"));

        dialogTexts.Add(new DialogData("Try your best to keep up so we don't have to go hunting for another replacement chef again", "Renge"));

        dialogTexts.Add(new DialogData("Ill try my best...", "Ramsay"));

        dialogTexts.Add(new DialogData("Anyways, get some sleep for now. Tomorrow is a new day!", "Knip"));

        dialogTexts.Add(new DialogData("Oh, and one advice, try to aim for a high performance score", "Knip"));

        dialogTexts.Add(new DialogData("It will help improve your Reputation and allow you to progress to the next day", "Knip"));

        dialogTexts.Add(new DialogData("Will improving my reputaion prevent me from getting murdured by the head chef?", "Ramsay"));


        LM.DaySelected = -1;

        dialogTexts.Add(new DialogData("Im leaving, bye.", "Ramsay", () => SceneManager.LoadScene("Game Scene")));

        DialogManager.Show(dialogTexts);
    }

    private void tut2end()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("Temp placeholder", "Knip"));

        dialogTexts.Add(new DialogData("Bye Bye!", "Ramsay", () => SceneManager.LoadScene("Level Select")));

        DialogManager.Show(dialogTexts);
    }


}