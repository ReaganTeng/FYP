using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerManager : MonoBehaviour
{
    public static MixerManager instance;
    [SerializeField] List<Mixer> mixers;

    private void Start()
    {
        instance = this;
    }

    public GameObject CheckIfAnyFilled(Mixer.MixerType whatType)
    {
        for (int i = 0; i < mixers.Count; i++)
        {
            if ((mixers[i].GetMixerType() == whatType) && (mixers[i].CheckIfFilled()))
            {
                return mixers[i].gameObject;
            }
        }

        return null;
    }

    public bool CheckIfAllAreEmpty(Mixer.MixerType whatType)
    {
        for (int i = 0; i < mixers.Count; i++)
        {
            if (!mixers[i].CheckIfEmptied() && whatType == mixers[i].GetMixerType())
                return false;
        }

        return true;
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

    public void HighlightMixer(Mixer.MixerType whatType, bool active)
    {
        for (int i = 0; i < mixers.Count; i++)
        {
            if (mixers[i].GetMixerType() == whatType)
            {
                mixers[i].gameObject.GetComponent<ObjectHighlighted>().ToggleHighlight(active);
            }
        }
    }
}
