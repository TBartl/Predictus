using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictAndModifyMesh : MonoBehaviour, Resettable{
    public LibraryContent library;
    MeshFilter meshFilter;

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
        DepthMatrixData toApply = DepthMatrixData.GetWeighted(library.GetAllEntries(), library.GetWeights(meshFilter.mesh));
        meshFilter.mesh = UtilityApplyDepthMatrixToMesh.Apply(meshFilter.mesh, toApply);
    }
}
