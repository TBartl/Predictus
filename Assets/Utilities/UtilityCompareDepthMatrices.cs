using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityCompareDepthMatrices : MonoBehaviour {

    public static UtilityCompareDepthMatrices S;

    void Awake() {
        S = this;
    }

    public DepthMatrixData Compare(DepthMatrixData from, DepthMatrixData to) {
        //to.depths[0, 0] == -1 means no blocks

		//two rectangles, do substraction to find out the diff
		int width = from.depths.GetLength(0);
		int height = from.depths.GetLength(1);
		DepthMatrixData diff = new DepthMatrixData ();
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				//if any one of the depth value is -1, assign 0 to diff
				if (from.depths [x, y] == -1 || to.depths [x, y] == -1) {
					diff.depths [x, y] = 0;
					continue;
				}
				diff.depths[x, y] = from.depths [x, y] - to.depths [x, y];
			}
		}
        return diff;
    }
}
