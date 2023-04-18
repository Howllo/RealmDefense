using System;
using System.Collections;
using System.Collections.Generic;
using EnemyScripts;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpawnSystem
{
    public class WaveSpawner : MonoBehaviour
    {
        [Header("Default Information")]
        [Tooltip("Each new element of Wave is a new round. Each elements within wave is the spawn.")]
        [SerializeField] private List<Wave> roundPlan;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float timeBetweenWaves = 5f;
        [SerializeField] private float countdown = 2f;
        [SerializeField] private TextMeshProUGUI waveCountdownText;

        [Header("Pooling")]
        [SerializeField] private float maxSecondsBetweenSpawn;
        [SerializeField] private GameObject childObject;
        [SerializeField] private int totalEnemyType1;
        [SerializeField] private List<Enemy> poolEnemy = new List<Enemy>();

        [Header("Debug")]
        [SerializeField] private List<GameObject> enemiesLeft;
        [SerializeField] private int waveIndex = 0;

        private void Awake()
        {
            foreach (var plan in roundPlan)
            {
                plan.Awake();
            }
        }

        private void Start()
        {
            for (int i = 0; i < totalEnemyType1; i++)
            {
                InstantiateNewEnemy(EnemyType.Normal);
            }
        }

        void Update()
        {
            if (enemiesLeft.Count == 0)
            {
                waveCountdownText.enabled = true;
                if (countdown <= 0)
                {
                    StartCoroutine(SpawnWave());
                    countdown = timeBetweenWaves;
                }
                countdown -= Time.deltaTime;
                waveCountdownText.SetText(Mathf.Round(countdown).ToString());
            }
        }

        /// <summary> Deals with the spawning of the waves. </summary>
        /// TODO: Write a algorithm that randomly create waves without the need of Wave object. 
        /// <returns>Yield the thread until certain time has been meet.</returns>
        private IEnumerator SpawnWave()
        {
            // Checks if the wave is over or not.
            if (waveIndex >= roundPlan.Count)
            {
                GameFinished();
                yield break;
            }
            
            // Keeps track of the enemy that have Spawned
            Dictionary<EnemyType, int> spawnTracker = new Dictionary<EnemyType, int>();
            waveCountdownText.enabled = false;
            List<int> totalEnemy = roundPlan[waveIndex].totalEnemyPerPrefab;
            int total = 0;

            // Get Total NPC per wave.
            foreach (var totalNpc in totalEnemy)
            {
                total += totalNpc;
            }
            
            // Set the size of the List.
            enemiesLeft = new List<GameObject>(total);
            int totalEnemySpawned = 0;

            // Spawns every single enemy that is possible in the wave until every enemy has been spawn.
            while (totalEnemySpawned != total)
            {
                int randomEnemy = Random.Range(0, roundPlan[waveIndex].prefabs.Count);
                EnemyType type = TrackWaveSpawn(randomEnemy, spawnTracker);
                
                if (type != EnemyType.None)
                {
                    SpawnEnemy(type);
                    totalEnemySpawned++;
                }
                
                yield return new WaitForSeconds(Random.Range(0.5f, maxSecondsBetweenSpawn));
            }
            
            // Increment the wave once it is over.
            waveIndex++;
        }

        /// <summary>
        /// Tracker is used to make sure that the spawning does not go over the total
        /// allowed spawns per creature.
        /// </summary>
        /// <param name="element">Takes in a index to access the enemy.</param>
        /// <param name="spawnTracker">Takes in the spawn tracker Dictionary.</param>
        /// <returns>Returns a EnemyType enum, if cannot spawn, return None.</returns>
        private EnemyType TrackWaveSpawn(int element, Dictionary<EnemyType, int> spawnTracker)
        {
            int totalEnemy = roundPlan[waveIndex].totalEnemyPerPrefab[element];
            Enemy enemyScript = roundPlan[waveIndex].enemyHashStore[roundPlan[waveIndex].prefabs[element]];
            EnemyType type = enemyScript.GetEnemyType();
            
            // Check see if the key exist or not.
            // If it does not exist assume that it brand new and return the type.
            if (!spawnTracker.ContainsKey(type))
            {
                spawnTracker.Add(type, 1);
                return type;
            }
            spawnTracker.TryGetValue(type, out var amount);
            
            // Check the amount and total allowed. If it less than total allowed
            // Increment spawn tracker and return the type.
            if (amount < totalEnemy)
            {
                spawnTracker.TryGetValue(type, out var currentCount);
                spawnTracker[type] = currentCount;
                return type;
            }
            
            // If the enemy cannot be spawned, return none.
            return EnemyType.None;
        }

        /// <summary>
        /// Used to figure out what enemies should be selected to be spawn.
        /// If cannot find any create new ones.
        /// </summary>
        /// <param name="type">Takes in a enum type to be compared, or used to spawn.</param>
        private void SpawnEnemy(EnemyType type)
        {
            foreach (var enemy in poolEnemy)
            {
                if (enemy.GetEnemyType() == type && enemy.GetState() == EnemyState.Dead)
                {
                    // Starts the enemy.
                    enemy.EnemyStart();
                    
                    // Subscribes to the event.
                    enemy.eventDeathDisable += RemoveEnemy;
        
                    // Add to list keep track of how many enemy left in the wave
                    enemiesLeft.Add(enemy.gameObject);
                    
                    return;
                }
            }
            
            // If failed to find a enemy that is available create new enemy.
            InstantiateNewEnemy(type);
        }

        /// <summary>
        /// Used to instantiate at new enemy to be used for the waves.
        /// </summary>
        /// <param name="type">Take in a enemy enum type for the switch statement.</param>
        private void InstantiateNewEnemy(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Normal:
                    if (waveIndex <= roundPlan.Count)
                    {
                        GameObject temp = roundPlan[waveIndex].GetEnemyTypePrefab(type);
                        Enemy enemyScript = roundPlan[waveIndex].GetEnemyScript(temp);
                        enemyScript.SetStartLocation(spawnPoint);
                        var enemy = Instantiate(enemyScript);
                        poolEnemy.Add(enemy);
                        enemy.gameObject.transform.parent = childObject.transform;
                    }
                    return;
                case EnemyType.None:
                    return;
            }
        }
        
        /// <summary>
        /// Used to watch what enemies are left in the wave. 
        /// </summary>
        /// <param name="obj">Takes in a GameObject to be removed.</param>
        private void RemoveEnemy(GameObject obj)
        {
            enemiesLeft.Remove(obj);
        }

        private void GameFinished()
        {
            print("TODO: Game UI game finish function.");
        }
    }

    [Serializable]
    public class Wave
    {
        public List<GameObject> prefabs = new List<GameObject>();
        public List<int> totalEnemyPerPrefab = new List<int>();
        public Dictionary<EnemyType, GameObject> prefabHash = new Dictionary<EnemyType, GameObject>();
        public Dictionary<GameObject, Enemy> enemyHashStore = new Dictionary<GameObject, Enemy>();

        public void Awake()
        {
            foreach (var prefab in prefabs)
            {
                Enemy tempEnemy = prefab.GetComponent<Enemy>();
                prefabHash.Add(tempEnemy.GetEnemyType(), prefab);
                enemyHashStore.Add(prefab, tempEnemy);
            }
        }

        public GameObject GetEnemyTypePrefab(EnemyType type)
        {
            if (prefabHash.ContainsKey(type))
                return prefabHash[type];
            return null;
        }

        public Enemy GetEnemyScript(GameObject obj)
        {
            if (enemyHashStore.ContainsKey(obj))
                return enemyHashStore[obj];
            return null;
        }
    }
}