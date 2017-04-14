using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public delegate void UpdateCount(int num, int outOf);
[System.Serializable] public delegate void ReturnEntries(List<DepthMatrixData> entries);
[System.Serializable] public delegate void ReturnWeights(List<float> weights);


public class PredictAndModifyMesh : MonoBehaviour, Resettable{

    

    public LibraryContent library;
    MeshFilter meshFilter;

    List<DepthMatrixData> entries;

    void Awake() {
        meshFilter = this.GetComponent<MeshFilter>();
    }

    // Old composite solution
    //public void Reset() {
    //    DepthMatrixData toApply = DepthMatrixData.Composite(library.GetAllEntries());
    //    meshFilter.mesh = UtilityApplyDepthMatrixToMesh.Apply(meshFilter.mesh, toApply);
    //}

    // Weighted solution
    public void Reset() {
        StartCoroutine(library.GetAllEntriesCoroutine(OnCountUpdated, OnRecievedEntries));
    }

    public void OnCountUpdated(int num, int outOf) {
        int realOutOf = 2 * outOf + 1; // n entries + n weights + 1 apply
        Debug.Log(num + "out of" + realOutOf);

    }

    public void OnRecievedEntries(List<DepthMatrixData> entries) {
        this.entries = entries;
        StartCoroutine(library.GetWeights(meshFilter.mesh, OnCountUpdated, OnRecievedWeights));
    }

    public void OnRecievedWeights(List<float> weights) {
        DepthMatrixData toApply = DepthMatrixData.GetWeighted(entries, weights);
        meshFilter.mesh = UtilityApplyDepthMatrixToMesh.Apply(meshFilter.mesh, toApply);
    }
}
