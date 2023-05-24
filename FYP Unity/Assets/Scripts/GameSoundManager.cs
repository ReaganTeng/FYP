using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DigitalRuby.SoundManagerNamespace;

public class GameSoundManager : MonoBehaviour
{
    public Slider soundSlider;
    public Slider musicSlider;

    [SerializeField] string[] soundName;
    [SerializeField] AudioSource[] sound;
    private static IDictionary<string, AudioSource> soundDict = new Dictionary<string, AudioSource>();

    [SerializeField] string[] musicName;
    [SerializeField] AudioSource[] music;
    private static IDictionary<string, AudioSource> musicDict = new Dictionary<string, AudioSource>();

    private void Awake()
    {
        soundDict.Clear();
        musicDict.Clear();

        for (int i = 0; i < soundName.Length; i++)
        {
            soundDict.Add(soundName[i], sound[i]);
        }
        for (int i = 0; i < musicName.Length; i++)
        {
            musicDict.Add(musicName[i], music[i]);
        }

        if (PlayerPrefs.HasKey("SoundVolume"))
            soundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
        if (PlayerPrefs.HasKey("MusicVolume"))
            soundSlider.value = PlayerPrefs.GetFloat("MusicVolume");

        SoundManager.StopSoundsOnLevelLoad = false;
    }

    public static void PlaySound(string name)
    {
        soundDict[name].PlayOneShotSoundManaged(soundDict[name].clip);
    }

    public static void PlayMusic(string name)
    {
        musicDict[name].PlayLoopingMusicManaged(1.0f, 1.0f, false);
    }

    public void SoundVolumeChanged()
    {
        SoundManager.SoundVolume = soundSlider.value;
        PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
    }

    public void MusicVolumeChanged()
    {
        SoundManager.MusicVolume = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }
}