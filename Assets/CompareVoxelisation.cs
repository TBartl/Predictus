using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompareVoxelisation : MonoBehaviour {

    public Voxelization before;
    public Voxelization after;

    public VoxSubprocessDebugDrawMatrix gainDraw;
    public VoxSubprocessDebugDrawMatrix lossDraw;

    public int ignoreDrawDepth = 3;

    void Awake() {
        before.voxelizeOnStart = false;
        after.voxelizeOnStart = false;
    }

    void Start() {
        StartCoroutine(Compare());
    }

    IEnumerator Compare() {
        yield return null;
        yield return before.Voxelize();
        yield return after.Voxelize();

        int[,] beforeDepths = GetDepths(ref before.voxData);
        int[,] afterDepths = GetDepths(ref after.voxData);

        int[,] gains = new int[
            Mathf.Min(beforeDepths.GetLength(0), afterDepths.GetLength(0)),
            Mathf.Min(beforeDepths.GetLength(1), afterDepths.GetLength(1))];

        int maxLoss = 0;
        int maxGain = 0;

        int beforeAdjustedDepth =  before.voxData.matrix.GetLength(2);
        int afterAdjustedDepth = after.voxData.matrix.GetLength(2);

        for (int x = 0; x < gains.GetLength(0); x++) {
            // Matrices are different sizes, need to adjust where we sample
            int beforeAdjustedX = x + (beforeDepths.GetLength(0) - gains.GetLength(0)) / 2;
            int afterAdjustedX = x + (afterDepths.GetLength(0) - gains.GetLength(0)) / 2;
            for (int y = 0; y < gains.GetLength(1); y++) {
                int beforeAdjustedY = y + (beforeDepths.GetLength(1) - gains.GetLength(1)) / 2;
                int afterAdjustedY =  y + (afterDepths.GetLength(1)  - gains.GetLength(1)) / 2;

                int beforeDepth = beforeDepths[beforeAdjustedX, beforeAdjustedY];
                int afterDepth = afterDepths[afterAdjustedX, afterAdjustedY];

                if (beforeDepth == -1 || afterDepth == -1) {
                    continue;
                }

                beforeDepth -= beforeAdjustedDepth;
                afterDepth -= afterAdjustedDepth;

                int gain = afterDepth - beforeDepth;
                maxLoss = Mathf.Min(maxLoss, gain);
                maxGain = Mathf.Max(maxGain, gain);
                gains[x, y] = gain;
            }
        }

        IntVector3 drawBox = new IntVector3(
            gains.GetLength(0), 
            gains.GetLength(1), 
            2 * Mathf.Max(Mathf.Abs(maxLoss), maxGain) + 2 + 1);

        VoxData gainDrawData = new VoxData();
        gainDrawData.transform = this.transform;
        gainDrawData.scale = before.voxData.scale;
        gainDrawData.SetMatrixSize(drawBox);
        for (int x = 0; x < drawBox.x; x++) {
            for (int y = 0; y < drawBox.y; y++) {
                for (int gain = ignoreDrawDepth; gain <= gains[x, y]; gain++) {
                    gainDrawData.matrix[x, y, drawBox.z / 2 + 1 - gain] = true;
                }
            }
        }
        gainDraw.Execute(ref gainDrawData);

        VoxData lossDrawData = new VoxData();
        lossDrawData.transform = this.transform;
        lossDrawData.scale = before.voxData.scale;
        lossDrawData.SetMatrixSize(drawBox);
        for (int x = 0; x < drawBox.x; x++) {
            for (int y = 0; y < drawBox.y; y++) {
                for (int loss = -ignoreDrawDepth; loss >= gains[x, y]; loss--) {
                    lossDrawData.matrix[x, y, drawBox.z / 2 + 1 - loss] = true;
                }
            }
        }
        lossDraw.Execute(ref lossDrawData);
    }

    int[,] GetDepths(ref VoxData voxData) {
        IntVector3 box = new IntVector3(
            voxData.matrix.GetLength(0),
            voxData.matrix.GetLength(1),
            voxData.matrix.GetLength(2));

        int[,] depths = new int[box.x, box.y];
        for (int x = 0; x < box.x; x++) {
            for (int y = 0; y < box.y; y++) {
                int depth = -1; // Negative one means that there wasn't any blocks at that point
                for (int z = 0; z < box.z; z++) {
                    if (voxData.matrix[x,y,z] == true) {
                        depth = z;
                        break;
                    }
                }
                depths[x, y] = depth;
            }
        }
        return depths; //TODO see if we should be doing this by reference (is this array passed by val or reference)
    }
}
