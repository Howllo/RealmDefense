using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public static int enemiesAlive = 0;
    public Wave[] waves;
    public Transform spawnPoint;
    public float timeBetweenWaves = 5f;
    public int moneyPerCompletedWave = 100;
    private float countdown = 2f;
    public TextMeshProUGUI waveCountdownText;
    private int waveIndex = 0;
    public GameManager gameManager;
    private bool isWaveOver;
    
    private void Start()
    {
        enemiesAlive = 0;
    }

    void Update()
    {
        if(enemiesAlive > 0)
        {
            waveCountdownText.gameObject.SetActive(false);
            return;
        }

        if (enemiesAlive == 0)
        {
            if (Math.Abs(countdown - 5.0f) < Single.Epsilon && !isWaveOver)
            {
                isWaveOver = true;
            }
            
            // Reward for completing round.
            if (waveIndex > 0 && isWaveOver)
            {
                PlayerStats.Money += moneyPerCompletedWave;
                isWaveOver = false;
            }
        }

        if (waveIndex == waves.Length && PlayerStats.lives != 0)
        {
            gameManager.winLevel();
            enabled = false;
        }

        if (countdown <= 0)
        {
            StartCoroutine(SpawnWave());
            waveCountdownText.gameObject.SetActive(true);
            countdown = timeBetweenWaves;
            return;
        }
        
        waveCountdownText.gameObject.SetActive(true);
        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        waveCountdownText.SetText(string.Format("{0:00.00}", countdown));
    }

    IEnumerator SpawnWave()
    {
        PlayerStats.rounds++;
        Wave wave = waves[waveIndex];
        enemiesAlive = wave.count;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.spawnRate);
        }
        waveIndex++;
    }

    void SpawnEnemy(GameObject enemy)
    {
        Vector3 spawnPointSpawn = new Vector3(spawnPoint.position.x, 0.5f, spawnPoint.position.z);
        Instantiate(enemy, spawnPointSpawn, enemy.transform.rotation);
    }
}
