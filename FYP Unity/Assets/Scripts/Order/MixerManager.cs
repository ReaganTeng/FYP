using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerManager : MonoBehaviour
{
    public static MixerManager instance;
    List<Mixer> mixers = new List<Mixer>();

    private void Start()
    {
        instance = this;

        Mixer[] tempMixerArray = gameObject.GetComponentsInChildren<Mixer>();
        for (int i = 0; i < tempMixerArray.Length; i++)
        {
            mixers.Add(tempMixerArray[i]);
        }
    }

    public void SetAllQTEActive(bool ActiveOrNot)
    {
        for (int i = 0; i < mixers.Count; i++)
        {
            if (mixers[i].GetMixerType() == Mixer.MixerType.COOKER)
            {
                mixers[i].SetQTEActive(ActiveOrNot);
            }    
        }
    }

    // Toggle the first mixer type that appear in the list.
    public void ToggleMixers(bool active)
    {
        for (int i = 0; i < mixers.Count; i++)
        {
            mixers[i].SetIsActive(active);
        }
    }

    public void TutorialMixer(Mixer.MixerType whichMixer, bool active)
    {
        for (int i = 0; i < mixers.Count; i++)
        {
            if (mixers[i].GetMixerType() == whichMixer)
            {
                mixers[i].SetIsActive(active);
                HighlightMixer(i, active);
                break;
            }
        }
    }

    void HighlightMixer(int index, bool active)
    {
         mixers[index].gameObject.GetComponent<ObjectHighlighted>().ToggleHighlight(active);
    }
}
