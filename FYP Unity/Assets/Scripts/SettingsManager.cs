using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] Slider SoundSlider;
    [SerializeField] Slider MusicSlider;
    [SerializeField] Toggle screenSizeToggle;
    [SerializeField] TextMeshProUGUI screenSizeText;
    [SerializeField] LevelManager lm;
    [SerializeField] PlayerProgress pp;
    [SerializeField] ShopManager shop;

    // Start is called before the first frame update
    void Start()
    {
        SoundSlider.value = PlayerPrefs.GetFloat("SoundLVL", 1);
        MusicSlider.value = PlayerPrefs.GetFloat("MusicLVL", 1);

        if (PlayerPrefs.GetInt("WindowedMode", 0) == 0)
        {
            screenSizeToggle.isOn = false;
        }
        else
        {
            screenSizeToggle.isOn = true;
        }
    }

    //-game windowed/full screen 
    //-music volume slider
    //- Sound effect slider
    //- reset game/wipe player progress


    // Update is called once per frame
    //void Update()
    //{
    //    if (screenSizeToggle.isOn)
    //    {
    //        screenSizeText.text = "Windowed Mode";
    //    }
    //    else
    //    {
    //        screenSizeText.text = "Full Mode";
    //    }

    //}

    public void changeSoundVolume()
    {
        PlayerPrefs.SetFloat("SoundLVL", SoundSlider.value);
    }

    public void changeMusicVolume()
    {
        PlayerPrefs.SetFloat("MusicLVL", MusicSlider.value);
    }


    public void windowedMode()
    {
        if (screenSizeToggle.isOn == true)
        {
            PlayerPrefs.SetInt("WindowedMode", 1);
            Screen.fullScreen = true;

        }
        else
        {
            PlayerPrefs.SetInt("WindowedMode", 0);
            Screen.fullScreen = false;
        }
    }

    public void ResetGame()
    {
        Debug.Log("Game Reset");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("TutorialComplete", 0);
        lm.ResetGameLevel();
        lm.DaySelected = -1;
        pp.ResetPlayer();
        shop.ResetShopLevel();
    }

    //true = Full, false = windowed
    //1 = true, 2, false
    //[Screen.fullScreen] [1] = true;
}