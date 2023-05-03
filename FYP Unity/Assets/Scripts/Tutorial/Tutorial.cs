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
    [SerializeField] Collider apoloCollider;
    [SerializeField] Collider raisuCollider;
    [SerializeField] GameObject mashedPotatoPrefab;
    [SerializeField] GameObject[] mashedPotatoSpawnLocation;
    [SerializeField] GameObject MixerList;

    // time for delay
    private float DelayTime = 1;

    // Check the previous room
    LevelHallway.Hallway whichHallway;

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
        POTATO_SALAD_COME_IN,
        DO_HEAVY_ATTACK,
        OOTATOO_DROP_CHECK,
        OOTATOO_PERFECT_KILLING,
        PICKUP_SKIP,
        PICK_UP_PERFECT_POTATO,
        THROW_INTO_COOKER,
        GET_DISH_QTE,
        APPLE_CAKE_COME_IN,
        HEAD_TO_APOLO,
        KILL_APOLO_CORRECT,
        GET_FLOUR,
        HEAD_TO_RAISUU,
        GAME_CONTINUE,
    }

    List<TutorialPopUp> tut = new List<TutorialPopUp>();

    private void Start()
    {
        instance = this;

        // If a tutorial exist, sest it as a tutorial
        if (lm.levelInfo[lm.DaySelected - 1].isThereTutorial != null)
        {
            InTutorial = true; // Set to be a tutorial
            tut = lm.levelInfo[lm.DaySelected - 1].isThereTutorial.DialogueAndInstructions; // Load in all the dialogues
            // Get the component for the dialogue chatbox
            SquidText = SquidChatBox.GetComponentInChildren<TextMeshProUGUI>();
            skipPromptText = SkipPrompt.GetComponent<TextMeshProUGUI>();
            SkipPrompt.SetActive(false); // A variable that set the "Press Space to continue" to false first
            // Ignore the 3 2 1 startup
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<FreezeGame>().IgnoreStartUp();

            // Setting up variables for the tutorials
            TimeTillNextText = 0; // Time until "prompt comes"
            ConditionTriggered = true; //true during dialogue or when requirements are fulfilled. Skip to next dialogue
            IsInstruction = true; // Is it an instruction
            SkipPromptActive = false; // Is the "Press Space to continue" gonna be active
            blinktimer = BlinkDuration; // Duration of the blinking

            // Disable the hallway
            gm.GetComponent<UnlockHallway>().LockHallway(LevelHallway.Hallway.ALL);
            // Disable all spawners
            SpawnerManager.instance.SetSpawner(SpawnerManager.SPAWNERTYPE.ALL, false);
            GameObject tempPlayer = GameObject.FindGameObjectWithTag("Player");
            //tempPlayer.GetComponentInChildren<PlayerAttack>().SetCanSwapWeapon(false); // Disable swapping weapon
            tempPlayer.GetComponentInChildren<PlayerAttack>().SetCanDoHeavyAttack(false); // disable doing heavy attack
            tempPlayer.GetComponentInChildren<PlayerPickup>().CannotInteractWithMixer = true; // disable interacting with mixer
            tempPlayer.GetComponentInChildren<PlayerPickup>().CannotInteractWithDrawer = true; // disable interacting with drawer
            tempPlayer.GetComponentInChildren<PlayerPickup>().CannotInteractWithDustbin = true; // disable interacting with dustbin
            tempPlayer.GetComponentInChildren<PlayerStats>().SetIfFervorActive(false);// disable fervor system
            MixerManager.instance.SetAllQTEActive(false); // disable cooke QTE
        }
        else
        {
            SquidChatBox.SetActive(false);
        }
        //InTutorial = false;
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
    int numReference = 0;

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
                        whichHallway = LevelHallway.Hallway.KITCHEN_TO_OOTATOO;
                    }
                    if (ootatooCollider.GetComponent<InZone>().GetIsPlayerInZone())
                    {
                        gm.GetComponent<UnlockHallway>().LockHallway(whichHallway);
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
                        gm.GetComponent<UnlockHallway>().OpenHallway(whichHallway);
                    }

                    if (kitchenCollider.GetComponent<InZone>().GetIsPlayerInZone())
                    {
                        gm.GetComponent<UnlockHallway>().LockHallway(whichHallway);
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
                        for (int i = 0; i < 2; i++)
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
                        MixerManager.instance.HighlightMixer(Mixer.MixerType.REFINER, true);
                    }

                    if (MixerManager.instance.CheckIfAnyFilled(Mixer.MixerType.REFINER) != null)
                    {
                        objectReference = MixerManager.instance.CheckIfAnyFilled(Mixer.MixerType.REFINER);
                        ConditionTriggered = true;
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerPickup>().CannotInteractWithMixer = true;
                        MixerManager.instance.HighlightMixer(Mixer.MixerType.REFINER, false);
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
                        numReference = objectReference.GetComponentsInChildren<RefinedItem>().Length;
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerPickup>().CannotInteractWithMixer = false;
                    }

                    // if player has 2 mashed potato cup in their inventory
                    if (CheckIfValidRefined() && MixerManager.instance.CheckIfAllAreEmpty(Mixer.MixerType.REFINER))
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
                        MixerManager.instance.HighlightMixer(Mixer.MixerType.COOKER, true);
                    }

                    if (objectReference.GetComponentsInChildren<Dish>().Length >= 1 && MixerManager.instance.CheckIfAllAreEmpty(Mixer.MixerType.COOKER))
                    {
                        objectReference = null;
                        ConditionTriggered = true;
                        MixerManager.instance.HighlightMixer(Mixer.MixerType.COOKER, false);
                        ResetRun();
                    }
                    break;
                }

            case Instructions.SERVE_THE_DISH:
                {
                    if (RunOnce())
                    {
                        objectReference = GameObject.FindGameObjectWithTag("Inventory");
                        GameObject.FindGameObjectWithTag("Serve").GetComponent<ObjectHighlighted>().ToggleHighlight(true);
                    }

                    if (objectReference.GetComponentInChildren<Dish>() == null)
                    {
                        objectReference = null;
                        ConditionTriggered = true;
                        GameObject.FindGameObjectWithTag("Serve").GetComponent<ObjectHighlighted>().ToggleHighlight(false);
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
                        InTutorial = false;
                        //PlayerPrefs.SetInt("TutorialComplete", 1);
                        gm.GetComponent<UnlockHallway>().OpenHallway(LevelHallway.Hallway.KITCHEN_TO_OOTATOO);
                        SpawnerManager.instance.SetSpawner(SpawnerManager.SPAWNERTYPE.OOTATOO, true);
                    }

                    else if (Input.GetKeyDown(KeyCode.N))
                    {
                        ConditionTriggered = true;
                    }
                    break;
                }


            // For Second Tutorial level
            case Instructions.POTATO_SALAD_COME_IN:
                {
                    if (RunOnce())
                    {
                        // Prepare for order Instruction
                        GeneralTimer = DelayTime;
                        gm.GetComponent<OrderSystem>().SetWaitingTime(10000000);
                        gm.GetComponent<OrderSystem>().CreateAnOrder(0);
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

            case Instructions.DO_HEAVY_ATTACK:
                {
                    if (RunOnce())
                    {
                        objectReference = SpawnerManager.instance.GetSpawner(SpawnerManager.SPAWNERTYPE.OOTATOO).GetComponent<Spawner>().SpawnEnemy(3);
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerAttack>().SetCanDoHeavyAttack(true);
                    }

                    if (objectReference != null && objectReference.GetComponent<EnemyScript>().GetEnemyHealth() > 0 && objectReference.GetComponent<EnemyScript>().GetEnemyHealth() < 3)
                    {
                        objectReference.GetComponent<EnemyScript>().SetEnemyHealth(3);
                    }

                    if (objectReference == null)
                    {
                        Destroy(GameObject.FindGameObjectWithTag("Drops").GetComponentInChildren<Food>().gameObject);
                        ConditionTriggered = true;
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerAttack>().SetCanAttack(false);
                        ResetRun();
                    }
                    break;
                }

            case Instructions.OOTATOO_DROP_CHECK:
                {
                    if (RunOnce())
                    {
                        objectReference = GameObject.FindGameObjectWithTag("Drops");
                    }

                    // if there are no drops in the scene
                    if (objectReference.GetComponentInChildren<Food>() == null)
                    {
                        // Check to see if the ingredient is in a perfect quality
                        if (GameObject.FindGameObjectWithTag("Inventory").GetComponentInChildren<Food>().GetIsPerfect())
                        {
                            currentIndex += 7;
                        }
                        
                        ConditionTriggered = true;
                        ResetRun();
                    }

                    break;
                }

            case Instructions.OOTATOO_PERFECT_KILLING:
                {
                    if (RunOnce())
                    {
                        gm.GetComponent<InventoryImageControl>().ClearAll();
                        objectReference = SpawnerManager.instance.GetSpawner(SpawnerManager.SPAWNERTYPE.OOTATOO).GetComponent<Spawner>().SpawnEnemy();
                    }

                    // if it die
                    if (objectReference == null)
                    {
                        Food theDrop = GameObject.FindGameObjectWithTag("Drops").GetComponentInChildren<Food>();
                        // if it is a perfect quality
                        if (theDrop.GetIsPerfect())
                        {
                            ConditionTriggered = true;
                            ResetRun();
                        }
                        else
                        {
                            Destroy(theDrop.gameObject);
                            objectReference = SpawnerManager.instance.GetSpawner(SpawnerManager.SPAWNERTYPE.OOTATOO).GetComponent<Spawner>().SpawnEnemy();
                        }
                    }

                    break;
                }

            case Instructions.PICKUP_SKIP:
                {
                    if (RunOnce())
                    {
                        objectReference = GameObject.FindGameObjectWithTag("Drops");
                    }

                    // if the ingredient drop from the scene disappear
                    if (objectReference.GetComponentInChildren<Food>() == null)
                    {
                        currentIndex += 1;
                        ConditionTriggered = true;
                        ResetRun();
                    }
                    break;
                }

            case Instructions.PICK_UP_PERFECT_POTATO:
                {
                    if (RunOnce())
                    {
                        // Create the potatoes
                        for (int i = 0; i < mashedPotatoSpawnLocation.Length; i++)
                        {
                            GameObject mashedPotatoes = Instantiate(mashedPotatoPrefab, mashedPotatoSpawnLocation[i].transform.position, Quaternion.identity);
                            mashedPotatoes.transform.SetParent(GameObject.FindGameObjectWithTag("Drops").transform);
                            mashedPotatoes.GetComponent<Food>().SetPerfect(true);
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

            case Instructions.THROW_INTO_COOKER:
                {
                    if (RunOnce())
                    {
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerPickup>().CannotInteractWithMixer = false;
                    }

                    if (MixerManager.instance.CheckIfAnyFilled(Mixer.MixerType.COOKER))
                    {
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerPickup>().CannotInteractWithMixer = true;
                        ConditionTriggered = true;
                        ResetRun();
                    }
                    break;
                }

            case Instructions.GET_DISH_QTE:
                {
                    if (RunOnce())
                    {
                        objectReference = GameObject.FindGameObjectWithTag("Inventory");
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerPickup>().CannotInteractWithMixer = false;
                        MixerManager.instance.SetAllQTEActive(true);
                    }

                    if (objectReference.GetComponentsInChildren<Dish>().Length >= 1 && MixerManager.instance.CheckIfAllAreEmpty(Mixer.MixerType.COOKER))
                    {
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerPickup>().CannotInteractWithMixer = true;
                        SpecialChange(currentIndex, objectReference.GetComponentInChildren<Dish>().gameObject.GetComponent<Food>().GetAmtOfStars());
                        ConditionTriggered = true;
                        ResetRun();
                    }
                    break;
                }

            case Instructions.APPLE_CAKE_COME_IN:
                {
                    if (RunOnce())
                    {
                        // Prepare for order Instruction
                        GeneralTimer = DelayTime;
                        gm.GetComponent<OrderSystem>().SetWaitingTime(10000000);
                        gm.GetComponent<OrderSystem>().CreateAnOrder(1);
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

            case Instructions.HEAD_TO_APOLO:
                {
                    if (RunOnce())
                    {
                        gm.GetComponent<UnlockHallway>().OpenHallway(LevelHallway.Hallway.KITCHEN_TO_AAPOLO);
                        whichHallway = LevelHallway.Hallway.KITCHEN_TO_AAPOLO;
                    }
                    if (apoloCollider.GetComponent<InZone>().GetIsPlayerInZone())
                    {
                        gm.GetComponent<UnlockHallway>().LockHallway(whichHallway);
                        ConditionTriggered = true;
                        ResetRun();
                    }
                    break;
                }

            case Instructions.KILL_APOLO_CORRECT:
                {
                    if (RunOnce())
                    {
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerAttack>().SetCanSwapWeapon(true);
                        objectReference = SpawnerManager.instance.GetSpawner(SpawnerManager.SPAWNERTYPE.AAPOLO).GetComponent<Spawner>().SpawnEnemy();
                    }

                    // if the apolo died
                    if (objectReference == null)
                    {
                        GameObject dropReference = GameObject.FindGameObjectWithTag("Drops");
                        // if it is chopped apple
                        if (dropReference.GetComponentInChildren<Item>().GetItemType() == ItemManager.Items.APPLE_CHOPPED)
                        {
                            ConditionTriggered = true;
                        }
                        else
                        {
                            Destroy(dropReference.GetComponentInChildren<Item>().gameObject);
                        }
                        ResetRun();
                    }
                    break;
                }

            case Instructions.GET_FLOUR:
                {
                    if (RunOnce())
                    {
                        objectReference = GameObject.FindGameObjectWithTag("Inventory");
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerPickup>().CannotInteractWithDrawer = false;
                    }

                    // check to see if there is a flour in player inventory
                    for (int i = 0; i < objectReference.GetComponentsInChildren<Item>().Length; i++)
                    {
                        if (objectReference.GetComponentsInChildren<Item>()[i].GetItemType() == ItemManager.Items.FLOUR)
                        {
                            ConditionTriggered = true;
                            ResetRun();
                            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerPickup>().CannotInteractWithDrawer = true;
                        }
                    }
                    break;
                }

            case Instructions.HEAD_TO_RAISUU:
                {
                    if (RunOnce())
                    {
                        gm.GetComponent<UnlockHallway>().OpenHallway(LevelHallway.Hallway.KITCHEN_TO_OOTATOO);
                        gm.GetComponent<UnlockHallway>().OpenHallway(LevelHallway.Hallway.OOTATOO_TO_RAISUU);
                    }

                    if (raisuCollider.GetComponent<InZone>().GetIsPlayerInZone())
                    {
                        ConditionTriggered = true;
                        SpawnerManager.instance.SetSpawner(SpawnerManager.SPAWNERTYPE.RAISUU, true);
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerPickup>().CannotInteractWithDrawer = false;
                        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerPickup>().CannotInteractWithDustbin = false;
                        ResetRun();
                    }
                    break;
                }

            case Instructions.GAME_CONTINUE:
                {
                    gm.GetComponent<UnlockHallway>().OpenHallway(LevelHallway.Hallway.KITCHEN_TO_AAPOLO);
                    SpawnerManager.instance.SetSpawner(SpawnerManager.SPAWNERTYPE.OOTATOO, true);
                    SpawnerManager.instance.SetSpawner(SpawnerManager.SPAWNERTYPE.AAPOLO, true);
                    ConditionTriggered = true;
                    break;
                }
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

    void SpecialChange(int theCurrentIndex, int starAmt)
    {
        lm.levelInfo[lm.DaySelected - 1].isThereTutorial.DialogueAndInstructions[theCurrentIndex].TextDisplay =
            "As you can see, your dish has " + starAmt + " star quality.";

        lm.levelInfo[lm.DaySelected - 1].isThereTutorial.DialogueAndInstructions[theCurrentIndex + 1].TextDisplay =
            "2 from perfect mashed potato cup and " + (starAmt-2).ToString() + "from QTE.";
    }

    bool CheckIfValidRefined()
    {
        RefinedItem[] tempArray = objectReference.GetComponentsInChildren<RefinedItem>();
        int ValidRefinedAmt = 0;

        for (int i = 0; i < tempArray.Length; i++)
        {
            if (tempArray[i].GetItemType() != RefinedItemManager.RItems.MUSHY)
            {
                ValidRefinedAmt++;
            }
        }

        if (ValidRefinedAmt > numReference || ValidRefinedAmt == 2)
        {
            return true;
        }

        return false;
    }
}