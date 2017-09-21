using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonProcessAndUpdateMesh : MonoBehaviour {

    public MeshFilter fromScreen;
    public MeshFilter toPredictedAfter;
    public MeshFilter toBefore;

    public void OnClick() {
        DepthMatrixData toApply = null;
		SoundManager.SM.PlayTransformSound ();

        //if (debugAfterMesh) {
        //    DepthMatrixData fromDepths = UtilityVoxelizeAndGetDepthMatrix.S.Process(fromScreen.mesh);
        //    fromDepths.SaveAsPNG("from");
        //    DepthMatrixData toDepths =   UtilityVoxelizeAndGetDepthMatrix.S.Process(debugAfterMesh);
        //    toDepths.SaveAsPNG("to");
        //    toApply = UtilityCompareDepthMatrices.Compare(fromDepths, toDepths);
        //    toApply.SaveAsPNG("diff");
        //}

        toBefore.mesh = fromScreen.mesh;
        toPredictedAfter.mesh = UtilityApplyDepthMatrixToMesh.Apply(fromScreen.mesh, toApply);
    }

    
}
