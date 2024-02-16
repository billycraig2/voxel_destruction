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
        Vector3Int voxelPosition = new Vector3Int(x,y,z);
        
        // top face
        if (!voxelGrid.HasActiveNeighbour(voxelPosition, Vector3Int.up))
        {
            AddTopFace(x, y, z);
        }
        // bottom face
        if (!voxelGrid.HasActiveNeighbour(voxelPosition, Vector3Int.down))
        {
            AddBottomFace(x, y, z);
        }
        // left face
        if (!voxelGrid.HasActiveNeighbour(voxelPosition, Vector3Int.left))
        {
            AddLeftFace(x, y, z);
        }
        // right face
        if (!voxelGrid.HasActiveNeighbour(voxelPosition, Vector3Int.right))
        {
            AddRightFace(x, y, z);
        }
        // front face
        if (!voxelGrid.HasActiveNeighbour(voxelPosition, Vector3Int.forward))
        {
            AddFrontFace(x, y, z);
        }
        // back face
        if (!voxelGrid.HasActiveNeighbour(voxelPosition, Vector3Int.back))
        {
            AddBackFace(x, y, z);
        }
        
    }

    void AddTopFace(int x, int y, int z)
    {
        Vector3 baseStart = new Vector3(x, y + 1, z);
        float offset = 0.001f; // to avoid z-fighting
        
        vertices.Add(baseStart + new Vector3(0, -offset, 1));
        vertices.Add(baseStart + new Vector3(1, -offset, 1));
        vertices.Add(baseStart + new Vector3(1, -offset, 0));
        vertices.Add(baseStart + new Vector3(0, -offset, 0));
        
        int vCount = vertices.Count;
        triangles.Add(vCount - 4);
        triangles.Add(vCount - 3);
        triangles.Add(vCount - 2);
        triangles.Add(vCount - 4);
        triangles.Add(vCount - 2);
        triangles.Add(vCount - 1);
    }

    void AddBottomFace(int x, int y, int z)
    {
        Vector3 baseStart = new Vector3(x, y, z);
        float offset = 0.001f;
        
        vertices.Add(baseStart + new Vector3(0, +offset, 0));
        vertices.Add(baseStart + new Vector3(1, +offset, 0));
        vertices.Add(baseStart + new Vector3(1, +offset, 1));
        vertices.Add(baseStart + new Vector3(0, +offset, 1));
        
        int vCount = vertices.Count;
        triangles.Add(vCount - 4);
        triangles.Add(vCount - 3);
        triangles.Add(vCount - 2);
        triangles.Add(vCount - 4);
        triangles.Add(vCount - 2);
        triangles.Add(vCount - 1);
    }
    
    void AddLeftFace(int x, int y, int z)
    {
        Vector3 baseStart = new Vector3(x, y, z);
        float offset = 0.001f;
        
        vertices.Add(baseStart + new Vector3(-offset, 0, 0));
        vertices.Add(baseStart + new Vector3(-offset, 0, 1));
        vertices.Add(baseStart + new Vector3(-offset, 1, 1));
        vertices.Add(baseStart + new Vector3(-offset, 1, 0));
        
        int vCount = vertices.Count;
        triangles.Add(vCount - 4);
        triangles.Add(vCount - 3);
        triangles.Add(vCount - 2);
        triangles.Add(vCount - 4);
        triangles.Add(vCount - 2);
        triangles.Add(vCount - 1);
    }
    
    void AddRightFace(int x, int y, int z)
    {
        Vector3 baseStart = new Vector3(x + 1, y, z);
        float offset = 0.001f;
        
        vertices.Add(baseStart + new Vector3(+offset, 0, 1));
        vertices.Add(baseStart + new Vector3(+offset, 0, 0));
        vertices.Add(baseStart + new Vector3(+offset, 1, 0));
        vertices.Add(baseStart + new Vector3(+offset, 1, 1));
        
        int vCount = vertices.Count;
        triangles.Add(vCount - 4);
        triangles.Add(vCount - 3);
        triangles.Add(vCount - 2);
        triangles.Add(vCount - 4);
        triangles.Add(vCount - 2);
        triangles.Add(vCount - 1);
    }
    
    void AddFrontFace(int x, int y, int z)
    {
        Vector3 baseStart = new Vector3(x, y, z);
        float offset = 0.001f;
        
        vertices.Add(baseStart + new Vector3(0, 0, -offset));
        vertices.Add(baseStart + new Vector3(1, 0, -offset));
        vertices.Add(baseStart + new Vector3(1, 1, -offset));
        vertices.Add(baseStart + new Vector3(0, 1, -offset));
        
        int vCount = vertices.Count;
        triangles.Add(vCount - 4);
        triangles.Add(vCount - 3);
        triangles.Add(vCount - 2);
        triangles.Add(vCount - 4);
        triangles.Add(vCount - 2);
        triangles.Add(vCount - 1);
    }
    
    void AddBackFace(int x, int y, int z)
    {
        Vector3 baseStart = new Vector3(x, y, z + 1);
        float offset = 0.001f;
        
        vertices.Add(baseStart + new Vector3(1, 0, +offset));
        vertices.Add(baseStart + new Vector3(0, 0, +offset));
        vertices.Add(baseStart + new Vector3(0, 1, +offset));
        vertices.Add(baseStart + new Vector3(1, 1, +offset));
        
        int vCount = vertices.Count;
        triangles.Add(vCount - 4);
        triangles.Add(vCount - 3);
        triangles.Add(vCount - 2);
        triangles.Add(vCount - 4);
        triangles.Add(vCount - 2);
        triangles.Add(vCount - 1);
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
