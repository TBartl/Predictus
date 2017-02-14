using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityCompareDepthMatrices : MonoBehaviour {

    public static UtilityCompareDepthMatrices S;

    void Awake() {
        S = this;
    }

    public DepthMatrixData Compare(DepthMatrixData from, DepthMatrixData to) {
        //to.depthsp[0, 0] == -1;
        return null;
    }
}
