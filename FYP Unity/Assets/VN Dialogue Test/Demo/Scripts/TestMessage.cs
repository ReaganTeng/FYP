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
        // Level name // Level Index
        TUT1VN, // tutorial //-2
        TUT2VN, // tutorial // -1
        ETUT2VN, // tutorial // -1
        L1VN, // Level 1 // 1
        L2VN, // Level 2 // 2
        L3VN, // Level 3 // 3
        L4VN, // Level 4 // 4
        L5VN, // Level 5 // 5
        L6VN, // Level 6 // 6
        L7VN, // Level 7 // 7
        L8VN, // Level 8 // 8
        L8VN_Win, // Level 2 // 8
        L8VN_Lose // Level 2 // 8

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
            case SceneName.L1VN:
                lvl1vn();
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

        dialogTexts.Add(new DialogData("…?", "Ramses"));

        dialogTexts.Add(new DialogData("It seems that the human finally woke up", "Alien1")); 

        dialogTexts.Add(new DialogData(" About time too. Its almost opening time at the new restaurant.", "Alien2"));

        dialogTexts.Add(new DialogData("*still in a daze* /wait:1/ Give me 5 more minutes of sleep...", "Ramses"));

        dialogTexts.Add(new DialogData("Oh no you don't /size:up/(slaps him).", "Alien1"));

        dialogTexts.Add(new DialogData("/emote:sad/ Ow! Wait… /wait:0.5/ this isn't a dream?", "Ramses"));

        dialogTexts.Add(new DialogData("Oh poor human thinks he’s still in a dream. Look around you.", "Alien2"));

        dialogTexts.Add(new DialogData("/emote:awkward//size:up/?!!!!!!! ", "Ramses"));

        dialogTexts.Add(new DialogData("/emote:angry/Where am I? Who are you? What are you?!.", "Ramses"));

        dialogTexts.Add(new DialogData("Allow us to introduce ourselves. I am Knip, and my companion here is...", "Knip"));

        dialogTexts.Add(new DialogData("Renge and we come from planet Meeden IV. Can we move on with the important things now?", "Renge"));

        dialogTexts.Add(new DialogData("Of course. Human, consider yourself worthy to be a part of our grand opening of the great Isarata.", "Knip"));

        dialogTexts.Add(new DialogData("You have been selected to pioneer our newly established high end restaurant opening", "Knip"));

        dialogTexts.Add(new DialogData("/emote:awkward/Why me? I have my own restaurant to run too you know?", "Ramses"));

        dialogTexts.Add(new DialogData("Thats exactly why you’re here. Your cooking skills has been revered by the human world according to your Yolp reviews.", "Renge"));

        dialogTexts.Add(new DialogData("/emote:awkward/I really don’t want to be here. Can you just send me home so I can continue with my daily life?", "Ramses"));

        dialogTexts.Add(new DialogData("Well sorry human, but that is not an option. ", "Knip"));

        dialogTexts.Add(new DialogData("We need your help to secure a good Boogle restaurant review from the cosmic critics that will be arriving soon.", "Renge"));

        dialogTexts.Add(new DialogData("Once we have accomplished our primary objective of securing a 5 star review on Boogle then we will send you back home", "Knip"));

        dialogTexts.Add(new DialogData("/emote:worried/Sigh, guess I have no choice. ", "Ramses"));

        dialogTexts.Add(new DialogData("Then its settled, go and meet the head chef at the main kitchen ", "Knip"));

        dialogTexts.Add(new DialogData("He will give you a quick run down of how our kitchen operations work.", "Renge"));

        dialogTexts.Add(new DialogData("Lets hope that he’s in a good mood today.", "Knip"));

        LM.DaySelected = -2;

        dialogTexts.Add(new DialogData("Sigh, where do I start..", "Ramses", () => SceneManager.LoadScene("Game Scene")));

        DialogManager.Show(dialogTexts);


    }

    private void tut2vn()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:sad/ Ughhh, my body is sore everywhere...", "Ramses"));

        dialogTexts.Add(new DialogData("Oh, the human is back!", "Knip"));

        dialogTexts.Add(new DialogData("Welcome back, seeing as you are still in one piece I assume you've passed your first day", "Renge"));

        dialogTexts.Add(new DialogData("/emote:angry/ Wait a minute, what do you mean by in one piece", "Ramses"));

        dialogTexts.Add(new DialogData("Oh, the head chef is well known for permanently disabling his employess that cause trouble in his kitchen", "Knip"));

        dialogTexts.Add(new DialogData("The last chef before you is no where to be found after they failed too many orders", "Knip"));

        dialogTexts.Add(new DialogData("/emote:worried/ /size:up/ *gulp*", "Ramses"));

        dialogTexts.Add(new DialogData("Don't worry, he won't lay a tentacle on you if you keep up your work performance", "Renge"));

        dialogTexts.Add(new DialogData("Every passing day his expectations from you will get higher and higher", "Renge"));

        dialogTexts.Add(new DialogData("Try your best to keep up so we don't have to go hunting for another replacement chef again", "Renge"));

        dialogTexts.Add(new DialogData("/emote:awkward/ I'll try my best...", "Ramses"));

        dialogTexts.Add(new DialogData("Anyways, get some sleep for now. Tomorrow is a new day!", "Knip"));

        dialogTexts.Add(new DialogData("Oh, and one advice, try to aim for a high performance score", "Knip"));

        dialogTexts.Add(new DialogData("It will help improve your Reputation and allow you to progress to the next day", "Knip"));

        dialogTexts.Add(new DialogData("/emote:worried/ Will improving my reputaion prevent me from getting murdured by the head chef?", "Ramses"));


        LM.DaySelected = -1;

        dialogTexts.Add(new DialogData("Im leaving, bye.", "Ramses", () => SceneManager.LoadScene("Game Scene")));

        DialogManager.Show(dialogTexts);
    }

    private void tut2end()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("Alright Ramses, I think you are ready for the real thing. Starting from today we will be receiving real customers.", "Renge"));

        dialogTexts.Add(new DialogData("You are doing great! We have just about a week left for you to earn more cosmic credits that you can use to further upgrade yourself at the shop!", "Knip"));

        dialogTexts.Add(new DialogData("You currently have 2 Cosmic Credits so be sure to check out the shop later", "Knip"));

        dialogTexts.Add(new DialogData("Cosmic Credits will come with each successful grade you have achieved when each the day ends.", "Renge"));

        dialogTexts.Add(new DialogData("Also, thanks to our time machine you can go back and get an better score grade even if you mess up somehow.", "Knip"));

        dialogTexts.Add(new DialogData("How does that even work? ", "Ramses"));

        dialogTexts.Add(new DialogData("Err, it would take too long to explain selective time travel to the likes of you. Just know that there is a limit to how many credits can be earned in a day.", "Renge"));

        dialogTexts.Add(new DialogData("We have about a week left until our VIP Cosmic Critics reach restaurant Isarata so you still have some time to learn more of our alien cuisines and how best to cook them effectively.", "Knip"));

        dialogTexts.Add(new DialogData("Well, let’s keep cooking then! This alien kitchen really is alot more fun and convenient than cooking back at home.", "Ramses"));

        dialogTexts.Add(new DialogData("Im glad you feel that way, we made alot of adjustments and did alot of research to ensure it is to your liking.", "Knip"));

        dialogTexts.Add(new DialogData("Though the head chef can be really scary to deal with at times he seems like a nice guy.", "Ramses"));

        dialogTexts.Add(new DialogData("He really is! Just don’t get on his bad side and you won’t even need to see him angry ever! ", "Knip"));

        dialogTexts.Add(new DialogData("Will you guys save me if he gets to that point?", "Ramses"));

        dialogTexts.Add(new DialogData("We will try our best but no guarantees.", "Knip"));

        dialogTexts.Add(new DialogData("Oh, that reminds me. Follow me Ramses", "Renge", () => background.sprite = imageList[1])); //load empty whiteboard background upon next dialogue

        dialogTexts.Add(new DialogData("Starting from tomorrow the head chef will give you a brief run down on the new ingredients at the beginning of each new day", "Renge"));

        dialogTexts.Add(new DialogData("So be sure to reach here punctually on time before the start of each day so the chef can brief you on the day ahead", "Renge"));

        dialogTexts.Add(new DialogData("Make sure you pay attention! He might even give you some really useful tips to succeed if he's in a good mood that day", "Knip"));

        dialogTexts.Add(new DialogData("Great.. now I have to wake up even earlier. Im heading to bed now, today was exhausting as well", "Ramses"));

        dialogTexts.Add(new DialogData("You will get used to it, once you get those upgrades in the shop cooking will be a breeze too so look forward to it!", "Knip"));

        dialogTexts.Add(new DialogData("Thats all we have for you today, good night Ramses.The next time we meet will probably be after this whole ordeal is over before we send you home so good luck!", "Renge"));

        dialogTexts.Add(new DialogData("Good night", "Ramses", ()=> background.sprite = imageList[0]));

        dialogTexts.Add(new DialogData("Hey Renge, do you think Ramses has what it takes?", "Knip"));

        dialogTexts.Add(new DialogData("Who knows? We will just have to /size:up/ /wait:0.3/ Let /wait:0.3/ Him /wait:0.3/ Cook.", "Renge", () => SceneManager.LoadScene("Level Select")));

        DialogManager.Show(dialogTexts);
    }

    private void lvl1vn()
    {
        var dialogTexts = new List<DialogData>();



        dialogTexts.Add(new DialogData("Time to go to the head chef's briefing", "Ramses", () => background.sprite = imageList[1]));

        dialogTexts.Add(new DialogData("/emote:nosprite/Good morning Ramses,I hope you slept well.", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/I slept fine, thank you. So today's the first official day right?", "Ramses"));

        dialogTexts.Add(new DialogData("/emote:nosprite/You are correct. Today we will be serving some classic food that most of the alien population here is used to already.", "HeadChef", () => background.sprite = imageList[2]));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Over the course of the next week, the plan is to slowly introduce a new set of dishes each day from the various ingredients we have prepared in our kitchen", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ We will slowly add more and more ingredient diversity into the mix but lets start with the basics just for today.", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/You are correct. Today we will be serving some classic food that most of the alien population here is used to already.", "HeadChef", () => background.sprite = imageList[2]));


        LM.DaySelected = 1;

        dialogTexts.Add(new DialogData("Bye Bye!", "Ramses", () => SceneManager.LoadScene("Game Scene")));



        DialogManager.Show(dialogTexts);
    }

}