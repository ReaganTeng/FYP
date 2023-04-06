using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Chatbox : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] TextMeshProUGUI outputText;
    [SerializeField] PlayerProgress pp;
    [SerializeField] LevelManager lm;

    void Start()
    {
        outputText.text = "";
        // Attach a listener to the input field to detect when the user submits text
        inputField.onEndEdit.AddListener(HandleInput);
    }

    void HandleInput(string input)
    {
        // Do something with the user's input
        Debug.Log("User input: " + input);

        ProcessFunction(input);

        // Clear the input field
        inputField.text = "";
    }

    void ProcessFunction(string input)
    {
        string OutputMessage = "";
        // Reset Progress
        if (input.StartsWith("/nuke"))
        {
            pp.ResetCredibility();
            pp.ResetInventory();

            for (int i = 0; i < lm.levelInfo.Count; i++)
            {
                // if first lvl, dont lock
                if (i == 0)
                    lm.levelInfo[i].ResetLevel(false);
                else
                    lm.levelInfo[i].ResetLevel(true);
            }
            OutputMessage = "Nuked Successful! \n";
        }

        else if (input.StartsWith("/skip"))
        {
            DayTimer dayTimer = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DayTimer>();
            dayTimer.SetTimer(1);
            OutputMessage = "Day Skipped! \n";
        }

        else
        {
            OutputMessage = "Invalid Command! \n";
        }

        outputText.text += OutputMessage;
    }
}
