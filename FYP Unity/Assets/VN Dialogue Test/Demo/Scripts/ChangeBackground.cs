using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackground : MonoBehaviour
{
    public Sprite[] backgroundImages; // array of background images to choose from
    public int index; // index of the current background image

    private Camera cam; // reference to the camera component

    void Start()
    {
        cam = GetComponent<Camera>(); // get the camera component
        ChangeBackgroundImage(index); // set the initial background image
    }

    void Update()
    {
        // change the background image if the index has been updated
        if (Input.GetKeyDown(KeyCode.Space))
        {
            index = (index + 1) % backgroundImages.Length; // wrap around to the beginning if we reach the end
            ChangeBackgroundImage(index);
        }
    }

    void ChangeBackgroundImage(int index)
    {
        // set the camera's background image to the sprite at the given index
        cam.backgroundColor = new Color(0, 0, 0, 0);
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.cullingMask = 1 << 31;
        cam.depth = -1;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>(); // get the sprite renderer component
        renderer.sprite = backgroundImages[index]; // set the sprite to the one at the given index
    }
}







