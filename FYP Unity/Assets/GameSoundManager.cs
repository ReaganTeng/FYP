using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalRuby.SoundManagerNamespace
{

    public class GameSoundManager : MonoBehaviour
    {
        public Slider SoundSlider;
        public Slider MusicSlider;
        public InputField SoundCountTextBox;
        public Toggle PersistToggle;

        [SerializeField] int numberofsounds;
        [SerializeField] List<string> SoundName = new List<string>();
        [SerializeField] List<GameObject> Sound = new List<GameObject>();

        IDictionary<string, GameObject> SoundNames = new Dictionary<string, GameObject>();




        private void Start()
        {
            for (int i = 0; i < SoundNames.Count; i++)
            {
                SoundNames.Add(SoundName[i], Sound[i]);
            }
        }

        public void SoundVolumeChanged()
        {
            SoundManager.SoundVolume = SoundSlider.value;
        }

        public void MusicVolumeChanged()
        {
            SoundManager.MusicVolume = MusicSlider.value;
        }

        public void PersistToggleChanged(bool isOn)
        {
            SoundManager.StopSoundsOnLevelLoad = !isOn;
        }


    }

}