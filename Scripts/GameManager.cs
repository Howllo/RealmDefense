using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameIsOver;
    public GameObject gameOverUI;
    public GameObject completeLevelUI;
    public GameObject livesUI;
    public GameObject moneyUI;
    public GameObject timerUI;

    private void Start()
    {
        gameIsOver = false;
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameIsOver)
        {
            return;
        }
        
        if(PlayerStats.lives <= 0 )
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        gameIsOver = true;
        gameOverUI.SetActive(true);
        livesUI.SetActive(false);
        moneyUI.SetActive(false);
        timerUI.SetActive(false);
        Time.timeScale = 0.0f;
    }

    public void winLevel()
    {
        gameIsOver = true;
        completeLevelUI.SetActive(true);
        livesUI.SetActive(false);
        moneyUI.SetActive(false);
        timerUI.SetActive(false);
    }
}
