using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VoxData {
    /// <summary> The 3D Mesh. 
    /// Assume that this mesh is correctly oriented and centered by this stage. </summary>
    public Mesh mesh;

    /// <summary> A 3D array of bools.
    /// This should be sized in the first step, and filled with 1 if the mesh intersects a cube. </summary>
    public bool[,,] matrix;

    /// <summary>  The edge length of a square.   
    /// The smaller this value is, the more precise results will be at the cost of speed. </summary>
    public float scale;
}
