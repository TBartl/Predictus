using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voxelization : MonoBehaviour {
    public VoxData voxData;
    public List<VoxSubProcess> subProcesses;
    
	void Start () {
        foreach (VoxSubProcess subProcess in subProcesses) {
            subProcess.Execute(ref voxData);
        }        
	}
}
