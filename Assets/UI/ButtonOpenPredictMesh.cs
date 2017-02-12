﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOpenPredictMesh : MonoBehaviour, Resettable {

    public Mesh debugOverrideMesh;
    public MeshFilter meshToDrawTo;
    public GameObject continueButton;

    public void OnClick() {
        Mesh mesh = UtilityOpenOBJ.S.OpenOBJ();
        if (debugOverrideMesh) {
            mesh = debugOverrideMesh;
            Debug.LogWarning("Using a debug override mesh");
        }

        meshToDrawTo.mesh = mesh;
        continueButton.SetActive(true);
    }

    public void Reset() {
        meshToDrawTo.mesh = null;
        continueButton.SetActive(false);
    }
}