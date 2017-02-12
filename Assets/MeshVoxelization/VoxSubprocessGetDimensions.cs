using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VoxSubprocessGetDimensions : VoxSubProcess {

    public override void Execute(ref VoxData voxData) {

        Vector3 boundingBoxSize = Vector3.zero;

        foreach (Vector3 vertex in voxData.mesh.vertices) {
            if (Mathf.Abs(vertex.x) > boundingBoxSize.x)
                boundingBoxSize.x = Mathf.Abs(vertex.x);

            if (Mathf.Abs(vertex.y) > boundingBoxSize.y)
                boundingBoxSize.y = Mathf.Abs(vertex.y);

            if (Mathf.Abs(vertex.z) > boundingBoxSize.z)
                boundingBoxSize.z = Mathf.Abs(vertex.z);
        }
        IntVector3 matrixSize = new IntVector3(
            2 * Mathf.CeilToInt(boundingBoxSize.x / VoxData.scale) + 1,
            2 * Mathf.CeilToInt(boundingBoxSize.y / VoxData.scale) + 1,
            2 * Mathf.CeilToInt(boundingBoxSize.z / VoxData.scale) + 1);
        voxData.SetMatrixSize(matrixSize);
    }


}
