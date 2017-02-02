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

    /// <summary>  The offset used to calculate where a point is in space.
    /// Updated using the UpdateOffset function whenever matrix is setup/changed</summary>
    Vector3 offset = Vector3.zero;

    /// <summary>  Updates the offset variable used to calculate where a point is in space.
    /// Must be called whenever matrix is setup/changed</summary>
    public void UpdateOffset() {
        offset = (-new Vector3(matrix.GetLength(0), matrix.GetLength(1), matrix.GetLength(2)) + Vector3.one) * scale / 2f;
    }

    /// <summary> Transforms a vector in 3D space to the cube it's associated with</summary>
    public IntVector3 TransformToIntVector(Vector3 point) { 
        Vector3 testPoint = point - offset;
        testPoint /= scale;        
        return new IntVector3(Mathf.RoundToInt(testPoint.x), Mathf.RoundToInt(testPoint.y), Mathf.RoundToInt(testPoint.z));
    }

    /// <summary> Fills the square at a point. </summary>
    public void ApplyVector(Vector3 point) {
        IntVector3 associatedSquare = TransformToIntVector(point);
        matrix[associatedSquare.x, associatedSquare.y, associatedSquare.z] = true;
    }
}
