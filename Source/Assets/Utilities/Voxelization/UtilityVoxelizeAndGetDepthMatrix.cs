using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityVoxelizeAndGetDepthMatrix : MonoBehaviour {

    public static UtilityVoxelizeAndGetDepthMatrix S;
    public List<VoxSubProcess> subProcesses;

    void Awake() {
        S = this;
    }

    public DepthMatrixData Process(Mesh mesh) {
        VoxData voxData = new VoxData();
        voxData.mesh = mesh;
        voxData.transform = this.transform;

        foreach (VoxSubProcess subProcess in subProcesses) {
            subProcess.Execute(ref voxData);
        }

        DepthMatrixData depthMatrix = new DepthMatrixData(ref voxData);
        return depthMatrix;
    }
}
