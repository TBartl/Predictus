using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxelization : MonoBehaviour {
    public VoxData voxData;
    public List<VoxSubProcess> subProcesses;
    public bool voxelizeOnStart = true;


    void Start() {
        if (voxelizeOnStart)
            StartCoroutine(Voxelize());
    }

    IEnumerator Voxelize() {
        foreach (VoxSubProcess subProcess in subProcesses) {
            subProcess.Execute(ref voxData);
            yield return null;
        }

    }

}
