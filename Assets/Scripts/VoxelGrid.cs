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
}
