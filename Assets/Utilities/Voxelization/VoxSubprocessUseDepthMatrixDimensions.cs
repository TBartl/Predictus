using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VoxSubprocessUseDepthMatrixDimensions : VoxSubProcess {

    public override void Execute(ref VoxData voxData) {

        IntVector3 matrixSize = new IntVector3(
            2 * DepthMatrixData.size.x + 1,
            2 * DepthMatrixData.size.y + 1,
            2 * DepthMatrixData.size.z + 1);

        voxData.SetMatrixSize(matrixSize);
    }


}
