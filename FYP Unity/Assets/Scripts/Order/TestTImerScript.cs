using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTImerScript : MonoBehaviour
{
    public Slider slider;
    public Transform pivot;

    void Update()
    {
        float angle = slider.value * 360f;
        pivot.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
