using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleChatbox : MonoBehaviour
{
    [SerializeField] PlayerMovement pm;
    [SerializeField] GameObject theChatBox;
    bool ChatBoxActive;

    private void Start()
    {
        theChatBox.SetActive(false);
        ChatBoxActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            EnableChatBox();
        }
    }

    void EnableChatBox()
    {
        pm.DisablePlayerControls();
        theChatBox.SetActive(true);
    }

    public void DisableChatBox()
    {
        pm.EnablePlayerControls();
        theChatBox.SetActive(false);
    }
}
