using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{

    public VoxelGrid voxelGrid;

    private Mesh mesh;
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uvs = new List<Vector2>(); // texture mapping
    
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        
        CreateMesh();
    }

    void CreateMesh()
    {
        vertices.Clear();
        triangles.Clear();
        uvs.Clear(); // clear previous mesh data
        
        for (int x = 0; x < voxelGrid.size; x++)
        {
            for (int y = 0; y < voxelGrid.size; y++)
            {
                for (int z = 0; z < voxelGrid.size; z++)
                {
                    if (voxelGrid.voxels[x,y,z].isActive)
                    {
                        AddCube(x,y,z);
                    }
                }
            }
        }
        
        UpdateMesh();
    }

    void AddCube(int x, int y, int z)
    {
        // Example for adding a single cube face (e.g., the top face)
        Vector3 baseStart = new Vector3(x, y, z);

        // Add vertices for the top face
        vertices.Add(baseStart + new Vector3(0, 1, 0));
        vertices.Add(baseStart + new Vector3(1, 1, 0));
        vertices.Add(baseStart + new Vector3(1, 1, 1));
        vertices.Add(baseStart + new Vector3(0, 1, 1));

        // Each face uses the last 4 vertices added
        int vCount = vertices.Count;
        triangles.Add(vCount - 4);
        triangles.Add(vCount - 3);
        triangles.Add(vCount - 2);
        triangles.Add(vCount - 4);
        triangles.Add(vCount - 2);
        triangles.Add(vCount - 1);

        // Optional: Add UVs for the vertices here
    }
    
    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
    }
    
}
