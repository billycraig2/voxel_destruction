using System;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public int worldSize = 5; // Size of the world in number of chunks
    private int chunkSize = 16; // Assuming chunk size is 16x16x16

    private Dictionary<Vector3, Chunk> chunks;
    
    public static World Instance { get; private set; }
    
    public Material VoxelMaterial;
    
    public float noiseScale = 0.1f;
    public int heightScale = 10;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        chunks = new Dictionary<Vector3, Chunk>();

        GenerateWorld();
    }

    private void GenerateWorld()
    {
        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                for (int z = 0; z < worldSize; z++)
                {
                    Vector3 chunkPosition = new Vector3(x * chunkSize, y * chunkSize, z * chunkSize);
                    GameObject newChunkObject = new GameObject($"Chunk_{x}_{y}_{z}");
                    newChunkObject.transform.position = chunkPosition;
                    newChunkObject.transform.parent = this.transform;

                    Chunk newChunk = newChunkObject.AddComponent<Chunk>();
                    newChunk.Initialize(chunkSize);
                    chunks.Add(chunkPosition, newChunk);
                }
            }
        }
    }
    
    public Chunk GetChunkAt(Vector3 globalPosition)
    {
        // Calculate the position of the chunk containing the global position
        Vector3Int chunkCoordinates = new Vector3Int(
            Mathf.FloorToInt(globalPosition.x / chunkSize) * chunkSize,
            Mathf.FloorToInt(globalPosition.y / chunkSize) * chunkSize,
            Mathf.FloorToInt(globalPosition.z / chunkSize) * chunkSize
        );

        // Return the chunk in calculated position
        if (chunks.TryGetValue(chunkCoordinates, out Chunk chunk))
        {
            return chunk;
        }
        
        return null;
    }

    // Additional methods for managing chunks, like loading and unloading, can be added here
    
}