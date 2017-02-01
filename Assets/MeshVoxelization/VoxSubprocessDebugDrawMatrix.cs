using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VoxSubprocessDebugDrawMatrix: VoxSubProcess {

    public override void Execute(ref VoxData voxData) {

        Vector3 boxScale = Vector3.one * voxData.scale;
        Vector3 offset = (-new Vector3(voxData.matrix.GetLength(0), voxData.matrix.GetLength(1), voxData.matrix.GetLength(2)) + Vector3.one) * voxData.scale / 2f;


        for (int z = 0; z < voxData.matrix.GetLength(2); z++) {
            for (int y = 0; y < voxData.matrix.GetLength(1); y++) {
                for (int x = 0; x < voxData.matrix.GetLength(0); x++) {
                    if (voxData.matrix[x, y, z]) {
                        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        box.transform.localScale = boxScale;
                        box.transform.position = new Vector3(x, y, z) * voxData.scale + offset;
                    }

                }
            }
        }

        
    }
}
