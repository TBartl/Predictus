using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// !NOTE THIS SCRIPT IS NO LONGER USED IN THE ACTUAL PROGRAM AND IS FOR DEMO PURPOSES ONLY!

public class Voxelization : MonoBehaviour {
    public VoxData voxData;
    public List<VoxSubProcess> subProcesses;
    public bool voxelizeOnStart = true;


    void Start() {
        if (voxelizeOnStart)
            StartCoroutine(Voxelize());
    }

    public IEnumerator Voxelize() {
        foreach (VoxSubProcess subProcess in subProcesses) {
            subProcess.Execute(ref voxData);
            yield return null;
        }
    }

}
