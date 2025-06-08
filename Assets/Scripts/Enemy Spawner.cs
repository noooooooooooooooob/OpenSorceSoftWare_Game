using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class EnemySpawner : MonoBehaviour
{
    public float enemySpawnTime;
    public List<GameObject> enemyPrefabs;
    public List<Transform> spawnPoints;

    void Start()
    {
        StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            int randomSpawnPointIndex = Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[randomSpawnPointIndex];
            GameObject enemy = Instantiate(enemyPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(enemySpawnTime);
        }
    } 
}