using UnityEngine;

[System.Serializable]
public class Voxel
{
    public bool isActive;
    
    // other voxel properties

    public Voxel(bool _isActive)
    {
        isActive = _isActive;
    }
}
