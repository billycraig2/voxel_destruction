using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[System.Serializable]
public class Voxel
{
    public bool isActive;
    public Vector3 position; // position of the voxel in the grid
    public Color color;

    public Voxel(Vector3 position, Color color, bool isActive = true)
    {
        this.position = position;
        this.color = color;
        this.isActive = isActive;
    }
}
