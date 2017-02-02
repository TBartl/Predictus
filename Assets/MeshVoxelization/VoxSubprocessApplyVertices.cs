using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VoxSubprocessApplyVertices : VoxSubProcess {

    public override void Execute(ref VoxData voxData) {
        // Traverse all the vertices in voxData.mesh.vertices
        // By using an offset (see DebugDrawMatrix) and voxData.scale, you will be able
        //   to find the cube each vertice corresponds to.
        // Your goal here is to be given a point in 3D space and see where it fits into the matrix
        // This shouldn't be very long script (<30 lines), and likely won't even be used in the final
        //   version (as ApplyEdges will likely apply to the same cubes) but it will be super helpful 
        //   for whoever writes ApplyEdges to use as a reference.

        foreach (Vector3 v in voxData.mesh.vertices) {
            voxData.ApplyVector(v);
        }

    }

}
