using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[System.Serializable]
public class Voxel
{
    public bool isActive;
    public bool isDestructible;
    public Vector3 position; // position of the voxel in the grid
    public float mass; // mass of the voxel



    public Voxel(Vector3 position, Color color, bool isActive = true)
    {
        this.position = position;
        this.isActive = isActive;
        this.isDestructible = isDestructible;
        this.mass = mass;
    }
}
