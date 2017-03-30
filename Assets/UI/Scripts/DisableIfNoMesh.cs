using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIfNoMesh : MonoBehaviour {

    public MeshFilter meshFilter;
    void OnEnable() {
        if (meshFilter.mesh == null)
            this.gameObject.SetActive(false);
    }
}
