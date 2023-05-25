﻿using System.Collections;
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
            case SceneName.L2VN:
                lvl2vn();
                break;
            case SceneName.L3VN:
                lvl3vn();
                break;
            case SceneName.L4VN:
                lvl4vn();
                break;
            case SceneName.L5VN:
                lvl5vn();
                break;
            case SceneName.L6VN:
                lvl6vn();
                break;
            case SceneName.L7VN:
                lvl7vn();
                break;
            case SceneName.L8VN:
                lvl8vn();
                break;
            case SceneName.L8VN_Win:
                lvl1vn();
                break;
            case SceneName.L8VN_Lose:
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

        dialogTexts.Add(new DialogData("Probabaly?", "Knip"));


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

        LM.DaySelected = 1;

        var dialogTexts = new List<DialogData>();



        dialogTexts.Add(new DialogData("Time to go to the head chef's briefing", "Ramses", () => background.sprite = imageList[1]));

        dialogTexts.Add(new DialogData("/emote:nosprite/Good morning Ramses,I hope you slept well.", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/I slept fine, thank you. So today's the first official day right?", "Ramses"));

        dialogTexts.Add(new DialogData("/emote:nosprite/You are correct. Today we will be serving some classic food that most of the alien population here is used to already.", "HeadChef", () => background.sprite = imageList[2]));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Over the course of the next week, the plan is to slowly introduce a new set of dishes each day from the various ingredients we have prepared in our kitchen", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ We will slowly add more and more ingredient diversity into the mix but lets start with the basics just for today.", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/Im sure you are already familiar with these ingredients but lets quickly go over them again", "HeadChef", () => background.sprite = imageList[3]));

        dialogTexts.Add(new DialogData("/emote:nosprite/ First up, is the easiest ingredient to prepare, the Ootatoo.", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Its extremely easy to kill and can be killed in batches easily since they love to group up.", "HeadChef", () => background.sprite = imageList[4]));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Next, we have the Appoloo", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Appoloo's charging attacks can be easily deflected with a well timed attack while they are charging ", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ They are also fairly easy to kill but they often spread out, making it hard to hit multiple of them with one attack", "HeadChef", () => background.sprite = imageList[5]));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Last but not least, We have Raisuu! One of the most used ingredients in our kitchen due to how many delicacies stem from them", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Raisuu's will be abit challenging for you as they have quite alot of HP and on top of that they love to jump around.", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ So they will probably take awhile more to kill than the other 2 ingredients but the dishes made from it is worth alot points, so its very well worth the effort.", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Well, I think that about covers all the ingredients that are currently availible to use at the moment, and its almost opening time anyways", "HeadChef", () => background.sprite = imageList[1]));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Hopefully those tips will aid you in bringing glory to Isarata's name", "HeadChef", () => background.sprite = imageList[12]));

        dialogTexts.Add(new DialogData("/emote:worried/ That's alot to take in, but thanks anyways. You seem to be in an awfully good mood today", "Ramses", () => background.sprite = imageList[1]));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Well, I want this restuarant to succeed as much as you want to go home human. So quickly get to work station, the first customers are about to order up!", "HeadChef", () => SceneManager.LoadScene("Game Scene")));


        DialogManager.Show(dialogTexts);
    }

    private void lvl2vn()
    {

        LM.DaySelected = 2;

        var dialogTexts = new List<DialogData>();



        dialogTexts.Add(new DialogData("Time to go to the head chef's briefing", "Ramses", () => background.sprite = imageList[1]));

        dialogTexts.Add(new DialogData("/emote:nosprite/Good morning Ramses, yesterday's opening day went smoothly thanks to you.", "HeadChef", () => background.sprite = imageList[12]));

        dialogTexts.Add(new DialogData("Yesterday was tough but still managable I guess", "Ramses", () => background.sprite = imageList[1]));

        dialogTexts.Add(new DialogData("/emote:nosprite/Keep up with the good work, its only going to get tougher from here on out so prepare yourself.", "HeadChef", () => background.sprite = imageList[11]));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Today's recipes will require you to prepare some Saamono, an all time favourite ingredient among all aliens both young and old", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ The Saamono we have here are the fresh water type so they are relatively easy to prepare", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Recipes involving Saamono will usually also require varying amounts of premade ingredients as well so you could even prepare for them in advance if you wanted to", "HeadChef", () => background.sprite = imageList[6]));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Next, we have Eggyoo. They will charge at you similar to Aapolos you've prepared before except these guys are one tough shell to crack", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ So its advised to try and get multiple ingredients from them to save you time spent walking over to them at the other corner of the kitchen", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ There are benefits to going out of your way though since they can help you build your combo and fervor meter up easily.", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Just a re-fresher in case you have forgotten, your fervor will grant a bonus star to any final dish you make while it is active so be sure to build up your combos", "HeadChef", () => background.sprite = imageList[1]));

        dialogTexts.Add(new DialogData("/emote:nosprite/ And that just about wraps up todays briefing. Are there any questions?", "HeadChef", () => background.sprite = imageList[12]));

        dialogTexts.Add(new DialogData("Are the dishes we served yesterday still in today's menu?", "Ramses", () => background.sprite = imageList[1]));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Yes, you can expect to see repeat dishes from previous days so all your cooking knowledge will be cumilative", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Oh! Here comes the first few customers, lets get cooking.", "HeadChef", () => SceneManager.LoadScene("Game Scene")));


        DialogManager.Show(dialogTexts);
    }

    private void lvl3vn()
    {

        LM.DaySelected = 3;

        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("Time to go to the head chef's briefing", "Ramses", () => background.sprite = imageList[1]));

        dialogTexts.Add(new DialogData("/emote:nosprite/Morning Ramses, I see you are very quickly adapting to our kitchen already", "HeadChef", () => background.sprite = imageList[12]));

        dialogTexts.Add(new DialogData("Well, theres a saying back on earth that The perfect chef is flexible and adapts to the needs and requests of clients always", "Ramses", () => background.sprite = imageList[1]));

        dialogTexts.Add(new DialogData("/emote:nosprite/Well said. I think I'll also start incoporating that motto into my junior chef training school. Wait, we are getting sidetracked...", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Anyways, we are reaching the point where all new ingredients introduced from now on are all fairly hard to prepare without sufficient upgrades.", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Make sure you are always using up any cosmic credit you have on you since Respec-ing your upgrades is always free and can be done anytime in the shop", "HeadChef", () => background.sprite = imageList[8]));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Firstly, Kaaroots will charge and fire lasers at you constantly so it will probably take longer than usual to prepare them", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Keep an eye out for their laser indicator and dodge at the right timing to avoid getting hit", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Thankfully they can be interupted when ever they take damage while charging up their lasers so use this knowledge as you see fit.", "HeadChef", () => background.sprite = imageList[9]));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Then we have the Pinaachoo's that fires concentrated toxic balls in bursts of 3 at you.", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ You could try to dash away or through their projectiles if you are elusive enough", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ But my advise is to use the large room to manipulate their behaviours to your advantage", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ And that just about covers the new ingredients introduced today. Good luck with today's orders, they can be quite difficult at first", "HeadChef", () => background.sprite = imageList[12]));

        dialogTexts.Add(new DialogData("/emote:worried/ What if I get stuck and can't complete today's orders in time", "Ramses", () => background.sprite = imageList[1]));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Have you tried going back to a previous day for a higher ranking grade? You can earn more cosmic credits that way then come back to today when you get more upgrades", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Ah crap, no time to explain the quantam physics on how that works now. People are already queing up to eat, Go Go GO!", "HeadChef", () => SceneManager.LoadScene("Game Scene")));


        DialogManager.Show(dialogTexts);


    }

    private void lvl4vn()
    {

        LM.DaySelected = 4;

        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("Time to go to the head chef's briefing", "Ramses", () => background.sprite = imageList[1]));

        dialogTexts.Add(new DialogData("/emote:nosprite/Hey Ramses, you're here early today.. Are you that eager to cook?", "HeadChef", () => background.sprite = imageList[12]));

        dialogTexts.Add(new DialogData("I don't know what came over me this morning, I just felt like I'm way more energetic in the mornings lately", "Ramses", () => background.sprite = imageList[1]));

        dialogTexts.Add(new DialogData("/emote:nosprite/Its probably got something to do with all the upgrades you have by this point. They not only improve your cooking skills but also your overall health", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Once you get even more upgrades you will start to feel like a super human in the kitchen mostly but still a great feeling nonetheless.. Onto todays briefing!", "HeadChef", () => background.sprite = imageList[7]));

        dialogTexts.Add(new DialogData("/emote:nosprite/ These Jiikens might seem cute and harmless, but they do not go down easily at all. Killing just 1 of them is already a herculean task", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ If I were you I would round them all up together before every heavy attack and try to hit as many as I can with the same attacks", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ This method also builds up a combo really fast so by the time you are done with them you should have your fervor ready to cook", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Each Jiiken also attacks really fast so watch out for that.", "HeadChef", () => background.sprite = imageList[1]));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Finally, we've come to our last enemy, the Moo Moos and its also the beefiest one yet.", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ These guys here actually managed to form their own cult and they gained a new power to shoot out demonic spells recently", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Even I don't dare go them these days but we don't have a choice if we want tasty beef dishes", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ With that I have covered all the ingredients our restaurant Isarata will be using to impress our Cosmic Critics arriving any day now.", "HeadChef", () => background.sprite = imageList[12]));

        dialogTexts.Add(new DialogData("/emote:worried/ Who are the Cosmic Critics anyways? And why are you so hell bent on getting a review from them", "Ramses", () => background.sprite = imageList[1]));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Nobody knows who they really are. The Cosmic Critics are a mysterious society that has the power to make any restaurant extremely successful after they leave a 5 Star Boogle Review post behind", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:nosprite/ Our objective is just to obtain that 5 star review at all costs... Anyways enough chit chat, these orders won't serve themselves! Get a move on!", "HeadChef", () => SceneManager.LoadScene("Game Scene")));


        DialogManager.Show(dialogTexts);
    }

    private void lvl5vn()
    {

        LM.DaySelected = 5;

        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("Theres no head chef briefing for today, I should just head to the kitchen and start todays work", "Ramses", () => SceneManager.LoadScene("Game Scene")));


        DialogManager.Show(dialogTexts);
    }

    private void lvl6vn()
    {

        LM.DaySelected = 6;

        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("Theres no head chef briefing for today, I should just head to the kitchen and start todays work", "Ramses", () => SceneManager.LoadScene("Game Scene")));


        DialogManager.Show(dialogTexts);
    }

    private void lvl7vn()
    {

        LM.DaySelected = 7;

        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("Theres no head chef briefing for today, I should just head to the kitchen and start todays work", "Ramses", () => SceneManager.LoadScene("Game Scene")));


        DialogManager.Show(dialogTexts);
    }

    private void lvl8vn()
    {

        LM.DaySelected = 8;

        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:worried/ *This is it Ramses, today is finally the day that decides if I get to go home in 1 piece or not.*.", "Ramses"));

        dialogTexts.Add(new DialogData("Time to go to the head chef's final briefing", "Ramses"));

        dialogTexts.Add(new DialogData("I must say Ramses, your cooking reputation on earth really precedes you. I was actually skeptical at first but you blew our expectations out of the water", "HeadChef"));

        dialogTexts.Add(new DialogData("As Isarata stands now we already have a steady flow of customers everyday thanks to your hard work but Isarata can become something greater.", "HeadChef"));

        dialogTexts.Add(new DialogData("Once we secure that 5 Star Boogle review we will definitely be able to expand and become a popular chain store across the galaxy.", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:angry/Hey. What in it for me then? I’ve been working really hard this past week for your success but what do i get out of it?", "Ramses"));

        dialogTexts.Add(new DialogData("Well, you get to go home", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:angry/…", "Ramses"));

        dialogTexts.Add(new DialogData("And you may leave with all of the upgrades you have earned through out your time here at Isarata", "HeadChef"));

        dialogTexts.Add(new DialogData("Wow, thats actually a pretty good deal! I’ve kinda grown used to these abilities so it would be a shame to part with them", "Ramses"));

        dialogTexts.Add(new DialogData("Yes, its a really good deal indeed. But only if you pass their standards. Speaking of their standards we estimate that you will need at least a minimum score of 250 to meet their expectations.", "HeadChef"));

        dialogTexts.Add(new DialogData("/emote:worried/250 is alot… What about they types of dishes they might order? Do they have a favourite type of food or such?", "Ramses"));

        dialogTexts.Add(new DialogData("Unfortunately we have no clue. So just be prepared for anything, cause his orders are always unpredictable. Its almost as if its randomized or something.", "HeadChef"));

        dialogTexts.Add(new DialogData("They should be here any minute now, I suggest you stand prepared at your work station in advance. All the best Ramses.", "HeadChef"));

        dialogTexts.Add(new DialogData("Ok. Its Time /wait:1/To /wait:1/Cook.", "Ramses", () => SceneManager.LoadScene("Game Scene")));


        DialogManager.Show(dialogTexts);
    }
}