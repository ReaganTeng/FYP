using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class mainMenuManager : MonoBehaviour
{
    [SerializeField] Canvas settingscanvas;
    [SerializeField] Button settingsbutton;

    // Start is called before the first frame update
    void Start()
    {
        settingscanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleSettingsCanvas()
    {
       settingscanvas.enabled = !settingscanvas.enabled;
   
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Level Select");

    }
}
