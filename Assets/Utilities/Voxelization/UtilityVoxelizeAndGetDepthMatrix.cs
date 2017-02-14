using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityVoxelizeAndGetDepthMatrix : MonoBehaviour {

    public static UtilityVoxelizeAndGetDepthMatrix S;
    public List<VoxSubProcess> subProcesses;

    void Awake() {
        S = this;
    }

    DepthMatrixData Process(Mesh mesh) {
        VoxData voxData = new VoxData();
        voxData.mesh = mesh;
        voxData.transform = this.transform;

        foreach (VoxSubProcess subProcess in subProcesses) {
            subProcess.Execute(ref voxData);
        }

        DepthMatrixData depthMatrix = new DepthMatrixData();


        //The mesh voxelization matrix and depth matrix have the same X/Z sizes
        for (int x = 0; x < voxData.matrix.GetLength(0); x++) {
            for (int y = 0; y < voxData.matrix.GetLength(1); y++) {
                int depth = -1; // Negative one means that there wasn't any blocks at that point
                for (int z = 0; z < voxData.matrix.GetLength(1); z++) {
                    if (voxData.matrix[x, y, z] == true) {
                        depth = z;
                        break;
                    }
                }
                depthMatrix.depths[x, y] = depth;
            }
        }
        return depthMatrix;
    }
}
