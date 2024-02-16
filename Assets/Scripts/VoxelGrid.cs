using UnityEngine;

public class VoxelGrid : MonoBehaviour
{
    public int size = 100;
    public Voxel[,,] voxels;

    void Awake()
    {
        InitializeVoxelGrid();
    }
    
    void InitializeVoxelGrid()
    {
        voxels = new Voxel[size,size,size];
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    voxels[x,y,z] = new Voxel(true);
                }
            }
        }
    }

    public bool IsVoxelActive(Vector3Int position)
    {
        if (position.x < 0 || position.x >= size ||
            position.y < 0 || position.y >= size ||
            position.z < 0 || position.z >= size)
        {
            return false;
        }
        return voxels[position.x, position.y, position.z].isActive;
    }

    public bool HasActiveNeighbour(Vector3Int voxelPosition, Vector3Int direction)
    {
        Vector3Int neighbourPosition = voxelPosition + direction;
        return IsVoxelActive(neighbourPosition);
    }
}
