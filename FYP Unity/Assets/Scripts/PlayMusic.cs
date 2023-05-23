using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameSoundManager.PlayMusic("GameMusic");
        Debug.Log("PlayMusic");
    }

  
}
