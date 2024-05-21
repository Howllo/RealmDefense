using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public string levelToLoad = "LevelSelect";
    public int _levelToUnlock = 1;

    public void play()
    {
        SceneManager.LoadScene(levelToLoad);  
    }

    public void quit()
    {
        Debug.Log("Exiting...");
        Application.Quit();
    }

    public void resetLevels()
    {
        PlayerPrefs.SetInt("levelReached", _levelToUnlock);
    }
}
