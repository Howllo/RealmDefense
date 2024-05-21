using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    public GameObject ui;
    public GameObject livesUI;
    public GameObject moneyUI;
    public GameObject timerUI;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            toggle();
        }
    }

    public void toggle()
    {
        ui.SetActive(!ui.activeSelf);

        if(ui.activeSelf)
        {
            livesUI.SetActive(false);
            moneyUI.SetActive(false);
            timerUI.SetActive(false);
            Time.timeScale = 0f;
        }
        else
        {
            livesUI.SetActive(true);
            moneyUI.SetActive(true);
            timerUI.SetActive(true);
            Time.timeScale = 1f;
        }
    }

   public void retry()
    {
        toggle();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void menu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
