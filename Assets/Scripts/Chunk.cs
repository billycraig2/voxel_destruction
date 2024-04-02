using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public GameObject voxelHighlight;
    private GameObject voxelHighlightInstance;
    private Quaternion lastCameraRotation;

    private Voxel[,,] voxels;
    private int chunkSize = 16;
    private Color gizmoColor;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uvs = new List<Vector2>();

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    [SerializeField]
    private GameObject voxelPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (voxelHighlight != null)
        {
            voxelHighlightInstance = Instantiate(voxelHighlight, Vector3.zero, Quaternion.identity);
            voxelHighlightInstance.SetActive(false);
        }
        
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshCollider = gameObject.AddComponent<MeshCollider>();

        GenerateMesh();
    }

    void Update()
    {
        HighlightVoxel();
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 hitPointInsideVoxel = hit.point - hit.normal * 0.01f; 
                Vector3 hitVoxelPosition = hitPointInsideVoxel - transform.position;

                int x = Mathf.FloorToInt(hitVoxelPosition.x);
                int y = Mathf.FloorToInt(hitVoxelPosition.y);
                int z = Mathf.FloorToInt(hitVoxelPosition.z);
                
                if (x >= 0 && x < chunkSize && y >= 0 && y < chunkSize && z >= 0 && z < chunkSize)
                {
                    voxels[x, y, z].isActive = false;
                    GenerateMesh();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 hitPoint = hit.point;
                Vector3 neighbourPosition = hitPoint - transform.position + hit.normal * 0.5f;

                int x = Mathf.FloorToInt(neighbourPosition.x);
                int y = Mathf.FloorToInt(neighbourPosition.y);
                int z = Mathf.FloorToInt(neighbourPosition.z);
                
                if (x >= 0 && x < chunkSize && y >= 0 && y < chunkSize && z >= 0 && z < chunkSize)
                {
                    voxels[x, y, z].isActive = true;
                    GenerateMesh();
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerExplosion();
        }
        
    }

    private void InitializeVoxels()
    {
        float worldX, worldZ, height;
        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                // Convert local chunk coordinates to world coordinates
                worldX = x + transform.position.x;
                worldZ = z + transform.position.z;

                // Use Perlin noise to determine the height at this x,z coordinate
                height = Mathf.PerlinNoise(worldX * World.Instance.noiseScale, worldZ * World.Instance.noiseScale) * World.Instance.heightScale;

                for (int y = 0; y < chunkSize; y++)
                {
                    // Convert the local y coordinate to a world y coordinate
                    float worldY = y + transform.position.y;

                    // Activate the voxel if its world y coordinate is less than the height determined by Perlin noise
                    bool isActive = worldY <= height;

                    voxels[x, y, z] = new Voxel(transform.position + new Vector3(x, y, z), Color.white, isActive);
                    if (isActive)
                    {
                        gizmoColor = new Color(Random.value, Random.value, Random.value, 0.4f);
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (voxels != null)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawCube(transform.position + new Vector3(chunkSize / 2, chunkSize / 2, chunkSize / 2),
                new Vector3(chunkSize, chunkSize, chunkSize));
        }
    }

    public void Initialize(int size)
    {
        this.chunkSize = size;
        voxels = new Voxel[size, size, size];

        InitializeVoxels();

    }

    public void IterateVoxels()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    if (voxels[x, y, z].isActive)
                    {
                        ProcessVoxel(x, y, z);
                    }
                }
            }
        }
    }

    private void ProcessVoxel(int x, int y, int z)
    {
        // Check if the voxels array is initialized and the indices are within bounds
        if (voxels == null || x < 0 || x >= voxels.GetLength(0) ||
            y < 0 || y >= voxels.GetLength(1) || z < 0 || z >= voxels.GetLength(2))
        {
            return; // Skip processing if the array is not initialized or indices are out of bounds
        }

        Voxel voxel = voxels[x, y, z];
        if (voxel.isActive)
        {
            // Check each face of the voxel for visibility
            bool[] facesVisible = new bool[6];

            // Check visibility for each face
            facesVisible[0] = IsFaceVisible(x, y + 1, z); // Top
            facesVisible[1] = IsFaceVisible(x, y - 1, z); // Bottom
            facesVisible[2] = IsFaceVisible(x - 1, y, z); // Left
            facesVisible[3] = IsFaceVisible(x + 1, y, z); // Right
            facesVisible[4] = IsFaceVisible(x, y, z + 1); // Front
            facesVisible[5] = IsFaceVisible(x, y, z - 1); // Back

            for (int i = 0; i < facesVisible.Length; i++)
            {
                if (facesVisible[i])
                    AddFaceData(x, y, z, i); // Method to add mesh data for the visible face
            }
        }
    }

    private bool IsFaceVisible(int x, int y, int z)
    {
        Vector3 globalPos = transform.position + new Vector3(x, y, z);
        
        // Check if the voxel is hidden in the chunk and in the world
        return IsVoxelHiddenInChunk(x, y, z) && IsVoxelHiddenInWorld(globalPos);
    }

    private void AddFaceData(int x, int y, int z, int faceIndex)
    {
        // Based on faceIndex, determine vertices and triangles
        // Add vertices and triangles for the visible face
        // Calculate and add corresponding UVs

        if (faceIndex == 0) // Top Face
        {
            vertices.Add(new Vector3(x, y + 1, z));
            vertices.Add(new Vector3(x, y + 1, z + 1));
            vertices.Add(new Vector3(x + 1, y + 1, z + 1));
            vertices.Add(new Vector3(x + 1, y + 1, z));
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(0, 1));
        }

        if (faceIndex == 1) // Bottom Face
        {
            vertices.Add(new Vector3(x, y, z));
            vertices.Add(new Vector3(x + 1, y, z));
            vertices.Add(new Vector3(x + 1, y, z + 1));
            vertices.Add(new Vector3(x, y, z + 1));
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 0));
        }

        if (faceIndex == 2) // Left Face
        {
            vertices.Add(new Vector3(x, y, z));
            vertices.Add(new Vector3(x, y, z + 1));
            vertices.Add(new Vector3(x, y + 1, z + 1));
            vertices.Add(new Vector3(x, y + 1, z));
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(0, 1));
        }

        if (faceIndex == 3) // Right Face
        {
            vertices.Add(new Vector3(x + 1, y, z + 1));
            vertices.Add(new Vector3(x + 1, y, z));
            vertices.Add(new Vector3(x + 1, y + 1, z));
            vertices.Add(new Vector3(x + 1, y + 1, z + 1));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 0));
        }

        if (faceIndex == 4) // Front Face
        {
            vertices.Add(new Vector3(x, y, z + 1));
            vertices.Add(new Vector3(x + 1, y, z + 1));
            vertices.Add(new Vector3(x + 1, y + 1, z + 1));
            vertices.Add(new Vector3(x, y + 1, z + 1));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 1));
        }

        if (faceIndex == 5) // Back Face
        {
            vertices.Add(new Vector3(x + 1, y, z));
            vertices.Add(new Vector3(x, y, z));
            vertices.Add(new Vector3(x, y + 1, z));
            vertices.Add(new Vector3(x + 1, y + 1, z));
            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(0, 0));

        }

        AddTriangleIndices();
    }

    private void AddTriangleIndices()
    {
        int vertCount = vertices.Count;

        // First triangle
        triangles.Add(vertCount - 4);
        triangles.Add(vertCount - 3);
        triangles.Add(vertCount - 2);

        // Second triangle
        triangles.Add(vertCount - 4);
        triangles.Add(vertCount - 2);
        triangles.Add(vertCount - 1);
    }

    private void GenerateMesh()
    {
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();
        
        IterateVoxels();

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        // Apply a material to the mesh
        meshRenderer.material = World.Instance.VoxelMaterial;
    }

    private bool IsVoxelHiddenInChunk(int x, int y, int z)
    {
        if (x < 0 || x >= chunkSize || y < 0 || y >= chunkSize || z < 0 || z >= chunkSize)
            return true;
        return !voxels[x, y, z].isActive;
    }
    
    private bool IsVoxelHiddenInWorld(Vector3 globalPos)
    {
        Chunk neighborChunk = World.Instance.GetChunkAt(globalPos);
        if (neighborChunk == null)
        {
            return true;
        }

        Vector3 localPos = neighborChunk.transform.InverseTransformPoint(globalPos);

        return !neighborChunk.IsVoxelActiveAt(localPos);
        
        
    }
    
    public bool IsVoxelActiveAt(Vector3 localPosition)
    {
        // Round the local position to get the nearest voxel index
        int x = Mathf.RoundToInt(localPosition.x);
        int y = Mathf.RoundToInt(localPosition.y);
        int z = Mathf.RoundToInt(localPosition.z);

        // Check if the indices are within the bounds of the voxel array
        if (x >= 0 && x < chunkSize && y >= 0 && y < chunkSize && z >= 0 && z < chunkSize)
        {
            // Return the active state of the voxel at these indices
            return voxels[x, y, z].isActive;
        }

        // If out of bounds, consider the voxel inactive
        return false;
    }

    
    private void HighlightVoxel()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f)) // Adjust the max distance as needed
        {
            Vector3 hitPoint = hit.point - hit.normal * 0.01f; // Nudge into the voxel
            Vector3 hitVoxelPosition = hitPoint - transform.position;

            int x = Mathf.FloorToInt(hitVoxelPosition.x);
            int y = Mathf.FloorToInt(hitVoxelPosition.y);
            int z = Mathf.FloorToInt(hitVoxelPosition.z);

            if (x >= 0 && x < chunkSize && y >= 0 && y < chunkSize && z >= 0 && z < chunkSize)
            {
                if (voxelHighlightInstance != null)
                {
                    voxelHighlightInstance.SetActive(true);
                    voxelHighlightInstance.transform.position = transform.position + new Vector3(x + 0.5f, y + 0.5f, z + 0.5f); // Center on voxel
                }
            }
        }
        else
        {
            if (voxelHighlightInstance != null)
            { 
                voxelHighlightInstance.SetActive(false);
            }
        }
    }

    private void TriggerExplosion()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Cast a ray from the camera to the mouse position
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f)) // Arbitrary distance of 100 units
        {
            Vector3 explosionCenter = hit.point; // The exact point of impact
            float explosionRadius = 5f; // Define the radius of your explosion

            // Convert the global hit point to local chunk coordinates
            Vector3 localHitPoint = this.transform.InverseTransformPoint(explosionCenter);

            // Explode the voxels in the chunk
            ExplodeVoxels(localHitPoint, explosionRadius);
        }
    }

    public void ExplodeVoxels(Vector3 explosionCenter, float explosionRadius)
    {
    
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    Vector3 voxelPosition = new Vector3(x, y, z);
                    float distance = Vector3.Distance(voxelPosition, explosionCenter);

                    if (distance <= explosionRadius)
                    {
                        voxels[x, y, z].isActive = false;
                    }
                }
            }
        }

        GenerateMesh();
    }
}
    
    


