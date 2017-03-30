using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictAndModifyMesh : MonoBehaviour, Resettable{
    public LibraryContent library;
    MeshFilter meshFilter;

    void Awake() {
        meshFilter = this.GetComponent<MeshFilter>();
    }

    public void Reset() {
        //TODO
        DepthMatrixData toApply =  library.GetCompositeOfAllLibraryEntries();
        meshFilter.mesh = UtilityApplyDepthMatrixToMesh.Apply(meshFilter.mesh, toApply);
    }
}
