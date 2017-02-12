using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VoxSubprocessFill : VoxSubProcess {

    public override void Execute(ref VoxData voxData) {
        // This is one of the few tasks that DOESN'T look at voxData.mesh
        // Instead, you have acess to voxData.matrix which will (hopefully) be a shell of the volume.
        // You will need to figure out which false bools in this matrix are on the inside and set them to true
        // Hint: Think about something like the flood fill tool in MSPaint.
        //   To figure out if something should be filled it continually expands unless it hits something
        //   that is a different color. The end result is that the shape will be filled. Try doing
        //   something like this except as if paint were a 3D editor.

    }
}
