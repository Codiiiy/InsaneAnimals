using UnityEngine;
using System.Collections.Generic;

public class ChunkGenerator : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag your Tilemap Chunk prefab here (the one with Tilemap & Tilemap Renderer).")]
    public GameObject tilemapChunkPrefab;

    [Tooltip("Reference to the player's Transform.")]
    public Transform player;

    [Header("Chunk Settings")]
    [Tooltip("The height of each chunk in world units.")]
    public float chunkHeight = 5f;

    [Tooltip("How close to the edge the player must get before spawning the next chunk.")]
    public float borderThreshold = 2f;

    private List<GameObject> activeChunks = new List<GameObject>();

    void Start()
    {
        if (tilemapChunkPrefab == null)
        {
            Debug.LogError("tilemapChunkPrefab is not assigned in the Inspector!");
            return;
        }
        SpawnChunk(Vector3.zero);
    }


    void Update()
    {
        GameObject currentChunk = GetCurrentChunk();
        if (currentChunk != null)
        {
            float chunkCenterY = currentChunk.transform.position.y;
            float chunkTop = chunkCenterY + (chunkHeight / 2f);
            float chunkBottom = chunkCenterY - (chunkHeight / 2f);

            if (player.position.y > chunkTop - borderThreshold)
            {
                SpawnChunk(currentChunk.transform.position + new Vector3(0f, chunkHeight, 0f));
            }
            else if (player.position.y < chunkBottom + borderThreshold)
            {
                SpawnChunk(currentChunk.transform.position - new Vector3(0f, chunkHeight, 0f));
            }
        }
    }


    private void SpawnChunk(Vector3 spawnPosition)
    {

        GameObject newChunk = Instantiate(tilemapChunkPrefab, spawnPosition, Quaternion.identity);
        activeChunks.Add(newChunk);
    }


    private GameObject GetCurrentChunk()
    {
        if (activeChunks.Count == 0)
            return null;

        GameObject closestChunk = activeChunks[0];
        float minDistance = Mathf.Abs(player.position.y - closestChunk.transform.position.y);

        foreach (GameObject chunk in activeChunks)
        {
            float dist = Mathf.Abs(player.position.y - chunk.transform.position.y);
            if (dist < minDistance)
            {
                closestChunk = chunk;
                minDistance = dist;
            }
        }
        return closestChunk;
    }
}
