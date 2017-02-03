using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VoxSubprocessDebugDrawBoundingBox : VoxSubProcess {

    public Material wireframeMaterial;

    public override void Execute(ref VoxData voxData) {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.transform.localScale = new Vector3(voxData.matrix.GetLength(0), voxData.matrix.GetLength(1), voxData.matrix.GetLength(2)) * voxData.scale;
        if (wireframeMaterial != null)
            box.GetComponent<MeshRenderer>().material = wireframeMaterial;
        if (voxData.transform != null) {
            box.transform.parent = voxData.transform;
            box.transform.localPosition = Vector3.zero;
        }

    }
}
