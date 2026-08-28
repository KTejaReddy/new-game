using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public Transform playerTransform;

    [Header("Spawning Settings")]
    public float spawnDistanceAhead = 150f;
    public float destroyDistanceBehind = 20f;
    public float spawnInterval = 0.5f;
    
    [Header("Obstacle Settings")]
    public float xySpawnRange = 15f;
    public float minObstacleScale = 1f;
    public float maxObstacleScale = 5f;

    [Header("Collectible Settings")]
    [Range(0f, 1f)]
    public float collectibleSpawnChance = 0.3f;

    private float nextSpawnZ;

    private void Start()
    {
        if (playerTransform != null)
        {
            nextSpawnZ = playerTransform.position.z + 50f; // Initial buffer
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameActive || playerTransform == null) return;

        // Spawn objects ahead
        if (playerTransform.position.z + spawnDistanceAhead > nextSpawnZ)
        {
            SpawnObjects();
            nextSpawnZ += spawnInterval * (GameManager.Instance.CurrentSpeed / GameManager.Instance.baseSpeed); // Scale density based on speed
        }

        // Cleanup is handled inherently if pool items are reused,
        // but we can also manually hide objects that pass behind the player
        // to free them up earlier.
        CleanupBehindPlayer();
    }

    private void SpawnObjects()
    {
        float randomX = Random.Range(-xySpawnRange, xySpawnRange);
        float randomY = Random.Range(-xySpawnRange, xySpawnRange);
        Vector3 spawnPos = new Vector3(randomX, randomY, nextSpawnZ);

        // Determine if we spawn a collectible or an obstacle
        if (Random.value < collectibleSpawnChance)
        {
            // Spawn Collectible
            ObjectPoolManager.Instance.SpawnFromPool("Collectible", spawnPos, Quaternion.identity);
        }
        else
        {
            // Spawn Obstacle
            Quaternion randomRot = Random.rotation;
            GameObject obstacle = ObjectPoolManager.Instance.SpawnFromPool("Obstacle", spawnPos, randomRot);
            
            if (obstacle != null)
            {
                float randomScale = Random.Range(minObstacleScale, maxObstacleScale);
                obstacle.transform.localScale = Vector3.one * randomScale;
            }
        }
    }

    private void CleanupBehindPlayer()
    {
        // To properly cleanup, we would need to track active objects.
        // A simple approach for object pools is to attach a small script to pooled objects
        // that checks distance to player and deactivates itself if too far behind.
    }
}
