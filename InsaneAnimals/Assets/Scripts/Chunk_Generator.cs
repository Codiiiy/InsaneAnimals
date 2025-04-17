using UnityEngine;
using System.Collections.Generic;

public class ChunkGenerator : MonoBehaviour
{
    [Header("References")]
    [Tooltip("List of tilemap chunk prefabs to randomly choose from.")]
    public List<GameObject> chunkPrefabs = new List<GameObject>();

    [Tooltip("Reference to the player's Transform.")]
    public Transform player;

    [Tooltip("Assign the specific prefab to use for the initial chunk.")]
    public GameObject initialChunk;

    [Header("Chunk Settings")]
    [Tooltip("The height of each chunk in world units.")]
    public float chunkHeight = 5f;

    [Tooltip("How close to the edge the player must get before spawning the next chunk.")]
    public float borderThreshold = 2f;

    [Tooltip("Maximum number of chunks to keep in memory.")]
    public int maxChunks = 5;

    [Tooltip("Distance between chunk centers when spawning.")]
    public int spawnDistance = 10;

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
        float chunkBottom = chunkCenterY - (chunkHeight / 2f);

        Vector3 abovePos = currentChunk.transform.position + new Vector3(0f, spawnDistance, 0f);
        Vector3 belowPos = currentChunk.transform.position - new Vector3(0f, spawnDistance, 0f); // fixed

        if (player.position.y > chunkTop - borderThreshold && !ChunkExistsAt(abovePos))
        {
            SpawnChunk(abovePos);
        }
        else if (player.position.y < chunkBottom + borderThreshold && !ChunkExistsAt(belowPos))
        {
            SpawnChunk(belowPos);
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

            if (oldChunk != null)
            {
                Destroy(oldChunk);
            }
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
