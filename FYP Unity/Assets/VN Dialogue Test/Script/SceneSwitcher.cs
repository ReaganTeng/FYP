using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void SwitchScene(string name)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(name);
    }
}
