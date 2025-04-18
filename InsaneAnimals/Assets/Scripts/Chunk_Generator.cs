using UnityEngine;
using System.Collections.Generic;

public class ChunkGenerator : MonoBehaviour
{
    [Header("References")]
    public List<GameObject> chunkPrefabs = new List<GameObject>();
    public Transform player;
    public GameObject initialChunk;

    [Header("Chunk Settings")]
    public float chunkHeight = 5f;
    public float borderThreshold = 2f;
    public int maxChunks = 5;
    public float spawnDistance = 10f;
    [Tooltip("How many chunks ahead to generate when the player nears the edge.")]
    public int chunksAhead = 1;

    private Queue<GameObject> activeChunks = new Queue<GameObject>();

    void Start()
    {
        if (chunkPrefabs == null || chunkPrefabs.Count == 0)
        {
            Debug.LogError("No chunk prefabs assigned in the Inspector!");
            return;
        }
        if (initialChunk == null)
        {
            Debug.LogError("Initial chunk prefab is not assigned in the Inspector!");
            return;
        }
        GameObject firstChunk = Instantiate(initialChunk, Vector3.zero, Quaternion.identity);
        activeChunks.Enqueue(firstChunk);
    }

    void Update()
    {
        GameObject currentChunk = GetCurrentChunk();
        if (currentChunk == null) return;

        float chunkCenterY = currentChunk.transform.position.y;
        float chunkTop = chunkCenterY + (chunkHeight / 2f);

        if (player.position.y > chunkTop - borderThreshold)
        {
            for (int i = 1; i <= chunksAhead; i++)
            {
                Vector3 spawnPos = currentChunk.transform.position + Vector3.up * spawnDistance * i;
                if (!ChunkExistsAt(spawnPos))
                {
                    SpawnChunk(spawnPos);
                }
            }
        }
    }

    private void SpawnChunk(Vector3 spawnPosition)
    {
        GameObject prefabToSpawn = chunkPrefabs[Random.Range(0, chunkPrefabs.Count)];
        GameObject newChunk = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        activeChunks.Enqueue(newChunk);
        if (activeChunks.Count > maxChunks)
        {
            GameObject oldChunk = activeChunks.Dequeue();
            if (oldChunk != null) Destroy(oldChunk);
        }
    }

    private bool ChunkExistsAt(Vector3 position)
    {
        foreach (GameObject chunk in activeChunks)
        {
            if (Vector3.Distance(chunk.transform.position, position) < 0.1f)
                return true;
        }
        return false;
    }

    private GameObject GetCurrentChunk()
    {
        if (activeChunks.Count == 0) return null;
        GameObject closest = null;
        float closestDist = float.MaxValue;
        foreach (GameObject chunk in activeChunks)
        {
            float dist = Mathf.Abs(player.position.y - chunk.transform.position.y);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = chunk;
            }
        }
        return closest;
    }
}
