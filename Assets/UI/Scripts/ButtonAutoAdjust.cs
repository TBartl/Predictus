using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAutoAdjust : MonoBehaviour {
    public OrientInputMesh orientInputMesh;
    MeshFilter mf;

    void Awake() {
        mf = orientInputMesh.gameObject.GetComponent<MeshFilter>();
    }

    public void OnClick() {
        if (ScreenManager.S.IsTransitioning())
            return;
        SoundManager.SM.PlayButtonSound();

        mf.mesh.RecalculateBounds();
        Vector3 boundsSize = mf.mesh.bounds.size;
        Debug.Log(boundsSize.x);
        Debug.Log(boundsSize.y);
        Debug.Log(boundsSize.z);
        // Rotate to be on the right axis
        if (boundsSize.x < boundsSize.y && boundsSize.y < boundsSize.z)//x<y<z
            orientInputMesh.transform.Rotate(90, 0, 90);
        else if (boundsSize.x < boundsSize.z && boundsSize.z < boundsSize.y)//x<z<y
            orientInputMesh.transform.Rotate(0, 90, 0);
        else if (boundsSize.y < boundsSize.x && boundsSize.x < boundsSize.z)//y<x<z
            orientInputMesh.transform.Rotate(90, 0, 0);
        else if (boundsSize.y < boundsSize.z && boundsSize.z < boundsSize.x)//y<z<x
            orientInputMesh.transform.Rotate(0, 90, 90);
        else if (boundsSize.z < boundsSize.x && boundsSize.x < boundsSize.y)//z<x<y
            orientInputMesh.transform.Rotate(0, 0, 0); // Correct orientation already
        else if (boundsSize.z < boundsSize.y && boundsSize.y < boundsSize.x)//z<y<x
            orientInputMesh.transform.Rotate(0, 0, 90);
        orientInputMesh.ApplyMeshTranslationAndRotation();

        float centerOfY = 0;
        List<Vector3> verts = new List<Vector3>(mf.mesh.vertices);
        foreach (Vector3 vert in verts) {
            centerOfY += vert.y * Mathf.Abs(vert.x);
        }
        if (centerOfY > 0) {
            orientInputMesh.transform.Rotate(0, 0, 180);
            orientInputMesh.ApplyMeshTranslationAndRotation();
        }

        float centerOfZ = 0;
        verts = new List<Vector3>(mf.mesh.vertices);
        foreach (Vector3 vert in verts) {
            centerOfZ += vert.z * Mathf.Abs(vert.y);
        }
        if (centerOfZ > 0) {
            orientInputMesh.transform.Rotate(0, 180, 0);
            orientInputMesh.ApplyMeshTranslationAndRotation();
        }

    }
}
