using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign the enemy prefab in the inspector
    public float spawnRate = 2f; // Time interval between spawns
    public float spawnRangeYMin = -4.22f; // Minimum Y position for spawning enemies
    public float spawnRangeYMax = 4.22f; // Maximum Y position for spawning enemies
    public float spawnX = 12f; // X position where enemies spawn

    private float spawnTimer;

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnRate)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    void SpawnEnemy()
    {
        // Randomize vertical spawn position within the desired range
        float spawnY = Random.Range(spawnRangeYMin, spawnRangeYMax);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        // Instantiate the enemy at the spawn position
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
