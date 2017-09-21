using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VoxSubprocessDrawMesh : VoxSubProcess {

    public Material material;

    public override void Execute(ref VoxData voxData) {
        GameObject model = GameObject.CreatePrimitive(PrimitiveType.Cube);
        model.GetComponent<MeshFilter>().mesh = voxData.mesh;
        if (material != null)
            model.GetComponent<MeshRenderer>().material = material;
        if (voxData.transform != null) {
            model.transform.parent = voxData.transform;
            model.transform.localPosition = Vector3.zero;
        }
    }
}
