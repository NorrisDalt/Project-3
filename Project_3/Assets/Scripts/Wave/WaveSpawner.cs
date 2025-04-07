using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public TempPlayerDmg temp;
    public OrbMovement orb;

    public Wave[] waves;
    public Transform[] spawnPoints;

    private int currentWaveIndex = 0;
    
    void Start()
    {
        //temp = GetComponent<TempPlayerDmg>();
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while(currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];
            Debug.Log("Wave " + currentWaveIndex);

            int enemyCount = currentWave.GetEnemyCount(currentWaveIndex);


            for(int i = 0; i < enemyCount; i++)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; // Sets spawnPoint to a random point from the spawnPoints array
                GameObject clone = Instantiate(currentWave.enemyPrefab, spawnPoint.position, spawnPoint.rotation);// Spawns at spawnPoint
                
                orb.allEnemiesList.Add(clone); // Adds enemy to list

                yield return new WaitForSeconds(currentWave.spawnDelay);
                Debug.Log("Total enemies added to list: " + orb.allEnemiesList.Count);
            }

            while(orb.allEnemiesList.Count > 0)
            {
                yield return null;
            }

            currentWaveIndex++;
            //yield return new WaitForSeconds(currentWave.waveDelay);


        }
        Debug.Log("All waves completed!");
    }


   
}
