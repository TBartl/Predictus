using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonProcessAndUpdateMesh : MonoBehaviour {
    public Mesh debugAfterMesh;

    public MeshFilter from;
    public MeshFilter to;
    public MeshFilter toOriginal;

    public void OnClick() {
        
        if (debugAfterMesh) {

        }

        toOriginal.mesh = from.mesh;
        to.mesh = from.mesh;
    }
}
