using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public static Tutorial instance;

    public bool InTutorial = false;

    private float TimeTillNextText;

    [SerializeField] LevelManager lm;
    [SerializeField] GameObject SquidChatBox;
    private TextMeshProUGUI SquidText;
    [SerializeField] GameObject SkipPrompt;
    private TextMeshProUGUI skipPromptText;
    private int currentIndex = 0;
    private bool ConditionTriggered;
    private Instructions currentInstruction = Instructions.NONE;

    private bool IsInstruction; // Check to see if its an instruction, if it is, ignore the input to skip. Ignored when start
    private bool SkipPromptActive; // Check to see if the skipprompt is active
    private int skipprompttimechange; // affects whether the timer is + or - by deltatime

    // Blinking effect for skip prompt
    [SerializeField] float BlinkDuration = 1;
    private float blinktimer = 2;

    // Reference to the GameManager
    [SerializeField] GameObject gm;
    [SerializeField] OrderRecipeDisplay ord;
    [SerializeField] GameObject MiniMapReference;
    [SerializeField] Collider ootatooCollider;
    [SerializeField] Collider kitchenCollider;
    [SerializeField] GameObject mashedPotatoPrefab;
    [SerializeField] GameObject[] mashedPotatoSpawnLocation;
    [SerializeField] GameObject MixerList;

    private float DelayTime = 3;

    [System.Serializable]
    public class TutorialPopUp
    {
        public string TextDisplay; // display the text shown on the chatbox
        public Instructions conditions; // If there is any conditions needed to fulfill the instructions
        public float TimeTillPromptSkip; // Set the time for when the prompt to skip comes in
    }

    public enum Instructions
    {
        NONE, // No instruction is assigned
        WASD_MOVEMENT,
        DASHING,
        ORDER_COME_IN,
        ORDER_CHECKING,
        OPEN_MINIMAP,
        CLOSE_MINIMAP,
        HEAD_TO_OOTATOO,
        KILL_AN_OOTATOO,
        PICK_UP_DROPS,
        PROCEED_TO_KITCHEN,
        PICK_UP_SCENE_POTATO,
        MAKE_A_REFINED_INGREDIENT,
        GRAB_REFINED_INGREDIENT,
        MAKE_AND_GET_REFINED_INGREDIENT,
        MAKE_A_DISH,
        SERVE_THE_DISH,
        OPEN_HALLWAY,
        SKIP_TUTORIAL,
    }

    List<TutorialPopUp> tut = new List<TutorialPopUp>();

    private void Start()
    {
        instance = this;

        if (lm.levelInfo[lm.DaySelected - 1].isThereTutorial != null)
        {
            InTutorial = true;
            tut = lm.levelInfo[lm.DaySelected - 1].isThereTutorial.DialogueAndInstructions;
            SquidText = SquidChatBox.GetComponentInChildren<TextMeshProUGUI>();
            skipPromptText = SkipPrompt.GetComponent<TextMeshProUGUI>();
            SkipPrompt.SetActive(false);
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<FreezeGame>().IgnoreStartUp();
            TimeTillNextText = 0;
            ConditionTriggered = true;
            IsInstruction = true;
            SkipPromptActive = false;
            blinktimer = BlinkDuration;
            gm.GetComponent<UnlockHallway>().LockHallway(LevelHallway.Hallway.KITCHEN_TO_OOTATOO);
            SpawnerManager.instance.SetSpawner(SpawnerManager.SPAWNERTYPE.ALL, false);
            GameObject tempPlayer = GameObject.FindGameObjectWithTag("Player");
            tempPlayer.GetComponentInChildren<PlayerAttack>().SetCanSwapWeapon(false);
            tempPlayer.GetComponentInChildren<PlayerAttack>().SetCanDoHeavyAttack(false);
            tempPlayer.GetComponentInChildren<PlayerPickup>().CannotInteractWithMixer = true;
            tempPlayer.GetComponentInChildren<PlayerStats>().SetIfFervorActive(false);
            MixerManager.instance.SetAllQTEActive(false);
        }
        else
        {
            SquidChatBox.SetActive(false);
        }
    }

    private void Update()
    {
        if (!InTutorial)
            return;
            
        // If the condition has been met, either its a dialogue, or an instruction that has been completed
        if (ConditionTriggered)
        {
            // If it is a dialogue, reduce the timer till skipprompt appear
            if (!SkipPromptActive)
            {
                TimeTillNextText -= Time.deltaTime;
                // set the prompt to appear
                if (TimeTillNextText <= 0)
                {
                    blinktimer = 0;
                    SetTransparency();
                    SkipPromptActive = true;
                    SkipPrompt.SetActive(true);
                }
            }

            else
            {
                // make it blink by setting alpha from 0 to 1 in timegiven/2 and vise verca.
                if (blinktimer <= 0)
                {
                    skipprompttimechange = 1;
                }

                if (blinktimer >= BlinkDuration * 0.5f)
                {
                    skipprompttimechange = -1;
                }

                blinktimer += skipprompttimechange * Time.deltaTime;
                SetTransparency();
            }

            // if it is an instruction that has been completed, or a dialogue and user press SPACE, skip to next dialogue
            // Note: The first run automatically trigger this function
            if (Input.GetKeyDown(KeyCode.Space) || IsInstruction)
            {
                // Set Skipprompt to not active
                SkipPrompt.SetActive(false);
                SkipPromptActive = false;

                // Set what is next in the List to the textbox and values required.
                if (currentIndex < tut.Count)
                {
                    SquidText.text = tut[currentIndex].TextDisplay;
                    TimeTillNextText = tut[currentIndex].TimeTillPromptSkip;
                    currentInstruction = tut[currentIndex].conditions;

                    if (currentInstruction != Instructions.NONE)
                    {
                        ConditionTriggered = false;
                        TimeTillNextText = 0;
                        IsInstruction = true;
                    }
                    else
                    {
                        IsInstruction = false;
                    }

                    currentIndex++;
                }
                // if it finish running the list, stop the tutorial, and let the game run normally.
                else
                {
                    InTutorial = false;
                    SquidChatBox.SetActive(false);
                }
            }
        }
        // if it is an instruction that has not be done yet, check to see if player fulfill the requirement
        else
        {
            InstructionDialogue();
        }
    }

    public void SetTransparency()
    {
        Color color = skipPromptText.color;
        float ratioTimer = blinktimer / (BlinkDuration * 0.5f);
        color.a = Mathf.Lerp(0, 1, ratioTimer);
        skipPromptText.color = color;
    }

    float MaxCheckTimer = 1.0f;
    float InputCheckTimer = 1.0f;

    float GeneralTimer;
    GameObject objectReference = null;

    // The list of instructions
    void InstructionDialogue()
    {
        switch(currentInstruction)
        {
            // For first Tutorial Level

            // If player has a wasd input for at least 1 second.
            case Instructions.WASD_MOVEMENT:
                {
                    if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                    {
                        InputCheckTimer -= Time.deltaTime;

                        if (InputCheckTimer <= 0)
                        {
                            ConditionTriggered = true;
                        }
                    }
                    else
                    {
                        InputCheckTimer = MaxCheckTimer;
                    }
                    break;
                }

            case Instructions.DASHING:
                {
                    if (Input.GetKeyDown(KeyCode.LeftShift))
                    {
                        ConditionTriggered = true;
                    }
                    break;
                }

            case Instructions.ORDER_COME_IN:
                {
                    if (RunOnce())
                    {
                        // Prepare for order Instruction
                        GeneralTimer = DelayTime;
                    }

                    if (GeneralTimer == DelayTime)
                    {
                        gm.GetComponent<OrderSystem>().SetWaitingTime(10000000);
                        gm.GetComponent<OrderSystem>().CreateAnOrder();
                        gm.GetComponent<OrderSystem>().SetWaitingTime();
                    }

                    GeneralTimer -= Time.deltaTime;
                    if (GeneralTimer <= 0)
                    {
                        ConditionTriggered = true;
                        ResetRun();
                    }
                    break;
                }

            case Instructions.ORDER_CHECKING:
                {
                    if (ord.CheckIfAnySelected())
                    {
                        ConditionTriggered = true;
                    }
                    break;
                }

            case Instructions.OPEN_MINIMAP:
                {
                    if (Input.GetKeyDown(KeyCode.M) && !MiniMapReference.activeSelf)
                    {
                        ConditionTriggered = true;
                    }
                    break;
                }

            case Instructions.CLOSE_MINIMAP:
                {
                    if (Input.GetKeyDown(KeyCode.M) && MiniMapReference.activeSelf)
                    {
                        ConditionTriggered = true;
                    }
                    break;
                }

            case Instructions.HEAD_TO_OOTATOO:
                {
                    if (RunOnce())
                    {
                        gm.GetComponent<UnlockHallway>().OpenHallway(LevelHallway.Hallway.KITCHEN_TO_OOTATOO);
                    }
                    if (ootatooCollider.GetComponent<InZone>().GetIsPlayerInZone())
                    {
                        gm.GetComponent<UnlockHallway>().LockHallway(LevelHallway.Hallway.KITCHEN_TO_OOTATOO);
                        ConditionTriggered = true;
                        ResetRun();
                    }
                    break;
                }

            case Instructions.KILL_AN_OOTATOO:
                {
                    if (RunOnce())
                    {
                        objectReference = SpawnerManager.instance.GetSpawner(SpawnerManager.SPAWNERTYPE.OOTATOO).GetComponent<Spawner>().SpawnEnemy();
                    }

                    if (objectReference == null)
                    {
                        ConditionTriggered = true;
                        ResetRun();
                    }
                    break;
                }

            case Instructions.PICK_UP_DROPS:
                {
                    // if there are no drops in the scene
                    if (GameObject.FindGameObjectWithTag("Drops").GetComponentInChildren<Food>() == null)
                    {
                        ConditionTriggered = true;
                    }
                    break;
                }

            case Instructions.PROCEED_TO_KITCHEN:
                {
                    if (RunOnce())
                    {
                        gm.GetComponent<UnlockHallway>().OpenHallway(LevelHallway.Hallway.KITCHEN_TO_OOTATOO);
                    }

                    if (kitchenCollider.GetComponent<InZone>().GetIsPlayerInZone())
                    {
                        gm.GetComponent<UnlockHallway>().LockHallway(LevelHallway.Hallway.KITCHEN_TO_OOTATOO);
                        ConditionTriggered = true;
                        ResetRun();
                    }
                    break;
                }

            case Instructions.PICK_UP_SCENE_POTATO:
                {
                    if (RunOnce())
                    {
                        // Create the potatoes
                        for (int i = 0; i < mashedPotatoSpawnLocation.Length; i++)
                        {
                            GameObject mashedPotatoes = Instantiate(mashedPotatoPrefab, mashedPotatoSpawnLocation[i].transform.position, Quaternion.identity);
                            mashedPotatoes.transform.SetParent(GameObject.FindGameObjectWithTag("Drops").transform);
                        }
                    }

                    // if there are no drops in the scene
                    if (GameObject.FindGameObjectWithTag("Drops").GetComponentInChildren<Food>() == null)
                    {
                        ConditionTriggered = true;
                        ResetRun();
                    }
                    break;
                }

            case Instructions.MAKE_A_REFINED_INGREDIENT:
                {
                    if (RunOnce())
                    {
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerPickup>().CannotInteractWithMixer = false;
                    }

                    if (MixerManager.instance.CheckIfAnyFilled(Mixer.MixerType.REFINER) != null)
                    {
                        objectReference = MixerManager.instance.CheckIfAnyFilled(Mixer.MixerType.REFINER);
                        ConditionTriggered = true;
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerPickup>().CannotInteractWithMixer = true;
                        ResetRun();
                    }
                    break;
                }

            case Instructions.GRAB_REFINED_INGREDIENT:
                {
                    if (RunOnce())
                    {
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerPickup>().CannotInteractWithMixer = false;
                    }

                    if (objectReference.GetComponent<Mixer>().CheckIfEmptied())
                    {
                        ConditionTriggered = true;
                        objectReference = null;
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerPickup>().CannotInteractWithMixer = true;
                        ResetRun();
                    }
                    break;
                }

            case Instructions.MAKE_AND_GET_REFINED_INGREDIENT:
                {
                    if (RunOnce())
                    {
                        objectReference = GameObject.FindGameObjectWithTag("Inventory");
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerPickup>().CannotInteractWithMixer = false;
                    }

                    // if player has 2 mashed potato cup in their inventory
                    if (objectReference.GetComponentsInChildren<RefinedItem>().Length >= 2 && MixerManager.instance.CheckIfAllAreEmpty(Mixer.MixerType.REFINER))
                    {
                        objectReference = null;
                        ConditionTriggered = true;
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerPickup>().CannotInteractWithMixer = true;
                        ResetRun();
                    }
                    break;
                }

            case Instructions.MAKE_A_DISH:
                {
                    if (RunOnce())
                    {
                        objectReference = GameObject.FindGameObjectWithTag("Inventory");
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerPickup>().CannotInteractWithMixer = false;
                    }

                    if (objectReference.GetComponentsInChildren<Dish>().Length >= 1 && MixerManager.instance.CheckIfAllAreEmpty(Mixer.MixerType.COOKER))
                    {
                        objectReference = null;
                        ConditionTriggered = true;
                        ResetRun();
                    }
                    break;
                }

            case Instructions.SERVE_THE_DISH:
                {
                    if (RunOnce())
                    {
                        objectReference = GameObject.FindGameObjectWithTag("Inventory");
                    }

                    if (objectReference.GetComponentInChildren<Dish>() == null)
                    {
                        objectReference = null;
                        ConditionTriggered = true;
                        ResetRun();
                    }
                    break;
                }

            case Instructions.OPEN_HALLWAY:
                {
                    gm.GetComponent<UnlockHallway>().OpenHallway(LevelHallway.Hallway.KITCHEN_TO_OOTATOO);
                    SpawnerManager.instance.SetSpawner(SpawnerManager.SPAWNERTYPE.OOTATOO, true);
                    ConditionTriggered = true;
                    break;
                }

            case Instructions.SKIP_TUTORIAL:
                {
                    if (Input.GetKeyDown(KeyCode.Y))
                    {
                        // skip tutorial next time player comes back
                        ConditionTriggered = true;
                        PlayerPrefs.SetInt("TutorialComplete", 1);
                        SceneManager.LoadScene("Level Select");
                    }

                    else if (Input.GetKeyDown(KeyCode.N))
                    {
                        ConditionTriggered = true;
                    }
                    break;
                }


            // For Second Tutorial level
        }

        // If the instructions has been done. Skip to the next dialogue/Instructions.
        if (ConditionTriggered)
        {
            currentInstruction = Instructions.NONE;
        }
    }

    private bool runOnce = true;
    bool RunOnce()
    {
        bool Returnvalued = runOnce;
        if (runOnce)
            runOnce = false;

        return Returnvalued;
    }

    void ResetRun()
    {
        runOnce = true;
    }
}
