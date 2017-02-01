using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VoxSubprocessApplyEdges : VoxSubProcess {

    public override void Execute(ref VoxData voxData) {
        // The toughest piece of the voxelization process.
        // Unlike ApplyVertices, you'll now also be looking at voxData.mesh.GetIndices(0)
        // This containers of integers is formatted with triples of ints.
        // The first three ints correspond to the indices (in voxData.mesh.vertices) of the first triangle
        // The next three ints correspond to the indices of the second triangle
        
        // The goal with this is that you'll be able to update voxData.matrix with the faces of indices
        // You'll probably want to think about this in two steps
        
        // 1) Fill the cubes alongside an edge.
        //    A simple way to do this is to just Vector3.Lerp between the vertices, and fill them at intervals
        //    along the way the same way that ApplyVertices does for each point.
        //    This interval is going to be the tricky part. Infact, you may want to think about it in terms of when
        //    the point crosses a cube boundrary on either of the 3 axis.

        // 2) Fill lines from point A across line BC.
        //    You can reuse a lot of code from (1) for this. If you're hitting all of the cubes along BC,
        //    you'll also be hitting all the points along the narrower piece of the triangle. Talk to Thomas
        //    if you need more clarified about this.

    }


}
