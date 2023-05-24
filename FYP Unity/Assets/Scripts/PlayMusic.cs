using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    [SerializeField] string musicName;

    void Start()
    {
        GameSoundManager.PlayMusic(musicName);
        Debug.Log("PlayMusic");
    }
}
